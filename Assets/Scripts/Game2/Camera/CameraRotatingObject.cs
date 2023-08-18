using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CameraRotatingObject : MonoBehaviour
{   
    private bool _canRotate;
    public void StartRotation()
    {
        _canRotate = true;
        transform.rotation = Quaternion.Euler(0,0,0);
    }

    public void DisableRotation()
    {
        _canRotate = false;
    }
    private void RotateAround()
    {   
        transform.Rotate(Vector3.up, Time.deltaTime * 40f );
    }

    private void Update()
    {   
       if(_canRotate) 
            RotateAround();
    }
}
