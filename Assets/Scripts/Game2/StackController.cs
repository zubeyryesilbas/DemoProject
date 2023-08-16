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
    private DG.Tweening.Sequence _stackSequence;
    private Istackable _currentStackable;
    private Vector3 _currentScale;
    private Vector3 _currentStackPosition;
    [SerializeField] private Transform _startPointTransform;
    [SerializeField] private Stack _stackPrefab;
    [SerializeField] private float _tolaranceValue;
    [SerializeField] private float _minWalkDistance;
    [SerializeField] private float _animationDuration;
    private TapController _tapController;
    private StackSoundController _stackSoundController;
    [Inject]
    public  void Construct(TapController tapController , StackSoundController stackSoundController) 
    {
        this._tapController = tapController;
        this._stackSoundController = stackSoundController;
    }
    void OnEnable()
    {
        _tapController.OnTap += OnTap;
    }
    void OnDisable()
    {
        _tapController.OnTap -= OnTap;
    }

    private void Start()
    {   
        _currentStackPosition = _startPointTransform.position + _currentScale.z * Vector3.right;
        _currentScale = _stackPrefab.transform.localScale; 
        CreateNewStack();
    }    
    private void CreateNewStack()
    {   
        var stackInstance = Instantiate(_stackPrefab , _currentStackPosition , quaternion.identity);
        _currentStackable = stackInstance.GetComponent<Istackable>();
        _currentStackable.SetScale(_currentScale);
        _currentStackPosition += Vector3.forward * _currentScale.z;
        var positionRight = _currentStackPosition + _currentScale.x * Vector3.right;
        var positionLeft = _currentStackPosition + _currentScale.x * Vector3.left;
        _currentStackable.StackableTransform.position = positionRight;
        _stackTween = _currentStackable.StackableTransform.DOMove(positionLeft , _animationDuration)
        .SetLoops(-1 , LoopType.Yoyo).SetEase(Ease.Linear);

    }

    private void OnTap()
    {   
        if(_stackTween != null)
        {
            _stackTween.Kill();
            ComparePositions(_currentStackable.StackableTransform.position);
        }    
    }

    private void ComparePositions(Vector3 stackablePos)
    {
        var difference = stackablePos.x - _currentStackPosition.x;
        var differenceAbs = Mathf.Abs(difference);
        var sign = Mathf.Sign(difference); 
        if(differenceAbs <= _tolaranceValue)
        {   Debug.Log("Perfect");
            _stackSoundController.PlayWithPitch();
        }
        else if(differenceAbs <= _minWalkDistance)
        {

        }
        var newSizeX = _currentScale.x -differenceAbs;
        var fallingBlockSize = Mathf.Abs(_currentStackable.StackableTransform.localScale.x - newSizeX);
        var newPositionX = _currentStackable.StackableTransform.position.x - (difference /2);
        _currentStackable.SetScale(new Vector3(newSizeX , _currentStackable.StackableTransform.position.y , _currentStackable.StackableTransform.position.z));
        _currentStackable.StackableTransform.position = new Vector3(newPositionX , _currentStackable.StackableTransform.position.y , _currentStackable.StackableTransform.position.z);

        var blockEdge = _currentStackable.StackableTransform.position.x + (newSizeX /2f * sign);
        var fallingBlockPos = blockEdge + fallingBlockSize / 2f * sign;
        DropStack(fallingBlockPos , fallingBlockSize);
        CreateNewStack();

    }

    private void DropStack(float fallingBlockXPos , float fallingBlockSize)
    {
        var stack = Instantiate(_stackPrefab);
        stack.transform.localScale = new Vector3(fallingBlockSize , stack.transform.localScale.y , stack.transform.localScale.z);
        stack.transform.position = new Vector3( fallingBlockXPos , _currentStackable.StackableTransform.position.y , _currentStackable.StackableTransform.position.z);
        stack.GetComponent<Istackable>().SetUnKinematic();
        _currentStackPosition = _currentStackable.StackableTransform.position;
        _currentScale = _currentStackable.StackableTransform.localScale;
        _currentStackable = stack.GetComponent<Istackable>();

    }
}
