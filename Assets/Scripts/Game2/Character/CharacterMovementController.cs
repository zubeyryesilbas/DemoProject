using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Zenject;
using DG.Tweening;
using Unity.Mathematics;

public class CharacterMovementController : MonoBehaviour
{
    private Rigidbody _rb;
    private StackController _stackController;
    private DG.Tweening.Sequence _moveSequence;
    private Tween _moveHorizontalTween;
    private Tween _moveForwardTween;
    private TapController _tapController;
    private Collider _collider;
    private Coroutine _colliderCheckCO;
    private bool _canRun = true;
    
     [Inject]
    public  void Construct(StackController stackController , TapController tapController) 
    {
        this._stackController = stackController;
        this._tapController = tapController;
        _stackController.OnStackHappened += MoveToTarget;
    }

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.isKinematic = true;
    }
    private void MoveToTarget()
    {    
        if(!_canRun)
            return;
            
        if(_moveForwardTween != null) _moveForwardTween.Kill();

        if(_moveHorizontalTween != null) _moveHorizontalTween.Kill();

        _moveHorizontalTween =  transform.DOMoveX(_stackController.CurrentStackPosition.x , 0.7f).SetEase(Ease.Linear);
        var speed = Vector3.Distance(_stackController.CurrentStackPosition , transform.position) /2.6f;
        _moveForwardTween =   transform.DOMoveZ(_stackController.NextStackPosition.z , 8.5f / speed).SetEase(Ease.Linear);
    }
    public void ForceMovement()
    {   
        var centerPoint = new Vector3(_stackController.CurrentStackPosition.x , 0 , _stackController.CurrentStackPosition.z);
        transform.DOMove(centerPoint , 0.3f);
        StartMovement();
        MoveToTarget();
    }
    public void StopMovement()
    {
       _canRun = false;
    }
    public void StartMovement()
    {
        _canRun = true;
    }

    public void SetUnKinematic()
    {
        _rb.isKinematic = false;
    }

   private void OnTriggerStay(Collider collider)
   {    
        if(collider.CompareTag("stack"))
        {  
            _collider = collider;
            if(_colliderCheckCO != null)   
                StopCoroutine(_colliderCheckCO);

                
            var stackable = collider.GetComponent<Istackable>();
            if(stackable != null)
            {
                if(!stackable.IsWalkable)
                    SetUnKinematic();
            }
            
        }
   }

   private void OnTriggerExit(Collider collider)
   {    
        if(collider.CompareTag("stack"))
        {   
            _collider = null;
            StartCoroutine(ColliderCheck());   
        }
   }

   private IEnumerator ColliderCheck()
   {
        yield return new WaitForSecondsRealtime(0.05f);
        if(_collider == null)
        {
            SetUnKinematic();
        }
   }


}
