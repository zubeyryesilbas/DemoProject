using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Zenject;
public class CameraAutoAligner : MonoBehaviour
{   
    
    private Camera _mainCamera;
    private GridController _gridController;
    
    [Inject]
    public  void Construct(GridController gridController)
    {
        this._gridController = gridController;
    }
    private void Start()
    {
        _mainCamera = Camera.main;
    }

    public void AlignCamera()
    {
        _mainCamera.transform.position = _gridController.GetCenterPointOfGrid() - Vector3.forward;
        _mainCamera.orthographicSize = _gridController.GetSizeOfGrid();
    }

    private void Update()
    {   
        if(Input.GetKeyDown(KeyCode.Space))
            AlignCamera();
    }

}
