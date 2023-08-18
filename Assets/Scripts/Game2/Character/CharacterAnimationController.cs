using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimationController : MonoBehaviour
{
    private Animator _anim;
    private int _dancingHash;
    private int _runningHash;
    private void Awake()
    {
        _anim = GetComponent<Animator>();
        _dancingHash = Animator.StringToHash("isDancing");
        _runningHash = Animator.StringToHash("isRunning");
    }

    public void SetDance()
    {
        _anim.SetBool(_dancingHash , true);
        _anim.SetBool(_runningHash , false);
    }

    public void SetRun()
    {
        _anim.SetBool(_dancingHash , false);
        _anim.SetBool(_runningHash , true);
    }
}
