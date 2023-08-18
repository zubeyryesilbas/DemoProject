using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using DG.Tweening;
using Unity.Mathematics;
using Zenject;
using System;
using UnityEditor.Experimental.GraphView;

public class StackController : MonoBehaviour
{
    private Tween _stackTween;
    public Action OnStackHappened;
    private DG.Tweening.Sequence _stackSequence;
    private Istackable _currentStackable;
    private Vector3 _currentScale;
    public Vector3 CurrentStackPosition;
    public Vector3 NextStackPosition;
    private Color _currentColor;
    private float _distanceBetweenStartAndEndPoint;
    [SerializeField] private Transform _startPointTransform , _endPointTransform;
    [SerializeField] private Stack _stackPrefab;
    [SerializeField] private float _tolaranceValue;
    [SerializeField] private float _minWalkDistance;
    [SerializeField] private float _animationDuration;
    private TapController _tapController;
    private StackSoundController _stackSoundController;
    private StackColorManager _stackColorManager;
    private int _stackCount;
    private int _maxStackCount = 20;
    [Inject]
    public  void Construct(TapController tapController , StackSoundController stackSoundController , StackColorManager stackColorManager) 
    {
        this._tapController = tapController;
        this._stackSoundController = stackSoundController;
        this._stackColorManager = stackColorManager;
    }
    void OnEnable()
    {
        _tapController.OnTap += OnTap;
    }
    void OnDisable()
    {
        _tapController.OnTap -= OnTap;
    }

    public void Start()
    {   
        CurrentStackPosition = _startPointTransform.position + _currentScale.z * Vector3.right;
        _currentScale = _stackPrefab.transform.localScale; 
        _distanceBetweenStartAndEndPoint = _endPointTransform.position.z - _startPointTransform.position.z; 
        var endPointy = 0;
        var endPointPosition =  CurrentStackPosition + _maxStackCount * Vector3.forward * _stackPrefab.transform.localScale.z;
        _endPointTransform.position = endPointPosition;
        _stackCount = 1;
        CreateNewStack();
        _endPointTransform.position = new Vector3(_endPointTransform.position.x , 0f , _endPointTransform.position.z);
    }    
    public void CreateNewStack()
    {   
        var stackInstance = Instantiate(_stackPrefab , CurrentStackPosition , quaternion.identity);
        _currentStackable = stackInstance.GetComponent<Istackable>();
        _currentColor = _stackColorManager.GetMaterial().color;
        _currentStackable.SetMaterialColor(_currentColor);
        _currentStackable.SetScale(_currentScale);
        CurrentStackPosition += Vector3.forward * _currentScale.z;
        NextStackPosition = CurrentStackPosition + Vector3.forward * _currentScale.z;
        var positionRight = CurrentStackPosition + _currentScale.x * Vector3.right;
        var positionLeft = CurrentStackPosition + _currentScale.x * Vector3.left;
        _currentStackable.StackableTransform.position = positionRight;
        _stackTween = _currentStackable.StackableTransform.DOMove(positionLeft , _animationDuration)
        .SetLoops(-1 , LoopType.Yoyo).SetEase(Ease.Linear);
        OnStackHappened.Invoke();
    }

    private void OnTap()
    {   
        if(_stackTween != null)
        {
            _stackTween.Kill();
            ComparePositions(_currentStackable.StackableTransform.position);
        }    
    }
    public void CreateNewLevelsPlatform()
    {
        _startPointTransform.position = new Vector3(_endPointTransform.position.x , _startPointTransform.position.y , _endPointTransform.position.z) ;
        CurrentStackPosition = _startPointTransform.position  ;
        var _endY = 0f;
        _currentScale = _stackPrefab.transform.localScale; 
        var spawnPos = _startPointTransform.position + Vector3.forward * _distanceBetweenStartAndEndPoint + Vector3.forward * _stackPrefab.transform.localScale.z;
        spawnPos.y = _endY;
        var newEndPoint = Instantiate(_endPointTransform.gameObject , _endPointTransform.position , quaternion.identity);
        _endPointTransform.position = spawnPos;
        _startPointTransform.gameObject.SetActive(false);
        _stackCount = 0;
    }
    private void ComparePositions(Vector3 stackablePos)
    {
        var difference = stackablePos.x - CurrentStackPosition.x;
        var differenceAbs = Mathf.Abs(difference);
        var sign = Mathf.Sign(difference); 
        var newSizeX = _currentScale.x -differenceAbs;
        if(newSizeX < _minWalkDistance)
        {   
            Debug.Log("final");
            _currentStackable.IsWalkable = false;
        }
        if(differenceAbs <= _tolaranceValue)
        {   Debug.Log("Perfect");
            _stackSoundController.PlayWithPitch();
        }
       
        
        var fallingBlockSize = Mathf.Abs(_currentStackable.StackableTransform.localScale.x - newSizeX);
        var newPositionX = _currentStackable.StackableTransform.position.x - (difference /2);
        _currentStackable.SetScale(new Vector3(newSizeX , _stackPrefab.transform.localScale.y , _stackPrefab.transform.localScale.z));
        _currentStackable.StackableTransform.position = new Vector3(newPositionX , _currentStackable.StackableTransform.position.y , _currentStackable.StackableTransform.position.z);

        var blockEdge = _currentStackable.StackableTransform.position.x + (newSizeX /2f * sign);
        var fallingBlockPos = blockEdge + fallingBlockSize / 2f * sign;
         _currentScale = _currentStackable.StackableTransform.localScale;
        DropStack(fallingBlockPos , fallingBlockSize);
        _stackCount += 1;
        if(_stackCount < _maxStackCount)
        {
            CreateNewStack();
        }
        else
        {
            _tapController.DisableTap();
        }
    }

    private void DropStack(float fallingBlockXPos , float fallingBlockSize)
    {
        var stackable = Instantiate(_stackPrefab).GetComponent<Istackable>();
        stackable.SetMaterialColor(_currentColor);
        stackable.SetScale(new Vector3(fallingBlockSize , stackable.StackableTransform.localScale.y , stackable.StackableTransform.localScale.z));
        stackable.StackableTransform.position = new Vector3( fallingBlockXPos , _currentStackable.StackableTransform.position.y , _currentStackable.StackableTransform.position.z);
        CurrentStackPosition = _currentStackable.StackableTransform.position;
        stackable.SetUnKinematic();
    }
}
