using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using DG.Tweening;
using Unity.Mathematics;
using Zenject;

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
    [Inject]
    public  void Construct(TapController tapController)
    {
        this._tapController = tapController;
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
         var positionRight = _currentStackPosition + _currentScale.x * Vector3.right;
        var positionLeft = _currentStackPosition + _currentScale.x * Vector3.left;
        _currentStackable.StackableTransform.position = positionRight;
        _stackTween = _currentStackable.StackableTransform.DOMove(positionLeft , _animationDuration)
        .SetLoops(-1 , LoopType.Yoyo).SetEase(Ease.Linear);

    }

    private void OnTap()
    {   
        if(_stackTween != null)
            _stackTween.Kill();
    }

    private float ComparePositions(Vector3 stackablePos)
    {
        var difference = stackablePos.x - _currentStackPosition.x;
        var differenceAbs = Mathf.Abs(difference); 
        if(differenceAbs <= _tolaranceValue)
        {

        }
        else if(differenceAbs <= _minWalkDistance)
        {

        }
        return difference;
    }

    private void CutCurrentStack(float dif)
    {   
        _currentStackable.SetScale(_currentScale - Vector3.right * dif);
    }
}
