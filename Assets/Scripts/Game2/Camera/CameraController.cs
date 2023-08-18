using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Zenject;
public class CameraController : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera _followCamera;
    [SerializeField] private CinemachineVirtualCamera _rotationCamera;
    private Character _character;
    [Inject]
    public  void Construct(Character character ) 
    {
        this._character = character;
    }
    public void ActivateFollowCamera()
    {   

        _followCamera.Priority = 11;
        _rotationCamera.Priority = 10;
        _character.CameraRotatingObject.DisableRotation();
    }

    public void ActivateRotationCamera()
    {
        _followCamera.Priority = 10;
        _rotationCamera.Priority = 11;
        _character.CameraRotatingObject.StartRotation();
    }
}
