using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Zenject;

public class Character : MonoBehaviour
{
    [SerializeField] private CharacterAnimationController _characterAnimController;
    [SerializeField] private CharacterMovementController _characterMovementController;
    public CameraRotatingObject CameraRotatingObject;

    public void SetLevelSuccessAction()
    {
        _characterMovementController.StopMovement();
        _characterAnimController.SetDance();
    }
    public void StartMovement()
    {   
        transform.rotation = quaternion.Euler(0 , 0 ,0);
        CameraRotatingObject.DisableRotation();
        _characterAnimController.SetRun();
        _characterMovementController.StartMovement();
    }
}
