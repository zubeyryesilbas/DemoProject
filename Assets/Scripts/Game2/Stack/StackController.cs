using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using DG.Tweening;
using Unity.Mathematics;
using Zenject;
using System;

public class StackController : MonoBehaviour
{
    private Tween _stackTween;
    private DG.Tweening.Sequence _stackSequence;
    private Istackable _currentStackable;
    private Vector3 _currentScale;
    private Color _currentColor;
    private float _distanceBetweenStartAndEndPoint;
    private TapController _tapController;
    private StackSoundController _stackSoundController;
    private StackColorManager _stackColorManager;
    private int _stackCount;
    private int _stackSign;
    private int _maxStackCount = 20;
    private float _endPlatformSize;
    private float _stackZSize;
    
    [SerializeField] private Transform _startPointTransform , _endPointTransform;
    [SerializeField] private Stack _stackPrefab;
    [SerializeField] private float _tolaranceValue;
    [SerializeField] private float _minWalkDistance;
    [SerializeField] private float _animationDuration;
    [HideInInspector] public Vector3 CurrentStackPosition;
    [HideInInspector] public Vector3 NextStackPosition;
    public Action OnStackHappened;
   
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
        Initialize();
    }    

    private void Initialize()
    {   
        CurrentStackPosition = _startPointTransform.position;
        _currentScale = _stackPrefab.transform.localScale; 
        _stackZSize = _currentScale.z;
        _endPlatformSize = _endPointTransform.GetComponent<MeshRenderer>().bounds.size.z;
        _distanceBetweenStartAndEndPoint = _endPointTransform.position.z - _startPointTransform.position.z; 
        var endPointPosition =  CurrentStackPosition + (_maxStackCount -1) * Vector3.forward * _stackZSize + Vector3.forward * _endPlatformSize ;
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
        CurrentStackPosition += Vector3.forward * _stackZSize;
        var random = UnityEngine.Random.Range(0 , 2);
        if(random == 1)
            _stackSign = -1;
        else
            _stackSign = 1;
        NextStackPosition = CurrentStackPosition + Vector3.forward * _stackZSize;
        var positionRight = CurrentStackPosition + _currentScale.x * Vector3.right * _stackSign;
        var positionLeft = CurrentStackPosition + _currentScale.x * Vector3.left * _stackSign;
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
        var spawnPos = _startPointTransform.position + Vector3.forward * (_distanceBetweenStartAndEndPoint - _stackZSize+ _endPlatformSize) + Vector3.forward * _stackZSize;
        spawnPos.y = _endY;
        Instantiate(_endPointTransform.gameObject , _endPointTransform.position , quaternion.identity);
        _endPointTransform.position = spawnPos;
        _startPointTransform.gameObject.SetActive(false);
        _stackCount = 0;
    }

    public void CreateNewStartPlatfom()
    {   
        _stackCount +=1;
        CurrentStackPosition += Vector3.forward * _stackPrefab.transform.localScale.z;
        var stackInstance = Instantiate(_stackPrefab , CurrentStackPosition , quaternion.identity);
    }
    private void ComparePositions(Vector3 stackablePos)
    {
        var difference = stackablePos.x - CurrentStackPosition.x;
        var differenceAbs = Mathf.Abs(difference);
        var sign = Mathf.Sign(difference); 
        var newSizeX = _currentScale.x -differenceAbs;
        var perfectStack = false;
        if(newSizeX < _minWalkDistance)
        {   
            Debug.Log("final");
            _currentStackable.IsWalkable = false;
        }
        if(differenceAbs <= _tolaranceValue)
        {   Debug.Log("Perfect");
           
            _stackSoundController.PlayWithPitch();
            var pos = new Vector3(CurrentStackPosition.x , CurrentStackPosition.y  ,_currentStackable.StackableTransform.position.z);
            _currentStackable.StackableTransform.position = pos ;
        }
        else
        {
            var fallingBlockSize = Mathf.Abs(_currentStackable.StackableTransform.localScale.x - newSizeX);
            var newPositionX = _currentStackable.StackableTransform.position.x - (difference /2);
            _currentStackable.SetScale(new Vector3(newSizeX , _stackPrefab.transform.localScale.y , _stackZSize));
            _currentStackable.StackableTransform.position = new Vector3(newPositionX , _currentStackable.StackableTransform.position.y , _currentStackable.StackableTransform.position.z);
            var blockEdge = _currentStackable.StackableTransform.position.x + (newSizeX /2f * sign);
            var fallingBlockPos = blockEdge + fallingBlockSize / 2f * sign;
            _currentScale = _currentStackable.StackableTransform.localScale;
            DropStack(fallingBlockPos , fallingBlockSize);
        }
       
        CurrentStackPosition = _currentStackable.StackableTransform.position;
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
        stackable.SetScale(new Vector3(fallingBlockSize , stackable.StackableTransform.localScale.y , _stackZSize));
        stackable.StackableTransform.position = new Vector3( fallingBlockXPos , _currentStackable.StackableTransform.position.y , _currentStackable.StackableTransform.position.z);
        stackable.SetUnKinematic();
    }
}
