using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TapController : MonoBehaviour
{   
    public Action OnTap;
    private bool _canTap = true;
    void Update()
    {
        if(Input.GetMouseButtonDown(0) && _canTap)
        {
            OnTap.Invoke();
        }
    }

    public void DisableTap()
    {
        _canTap = false;
    }
    public void EnableTap()
    {
        _canTap = true;
    }
}
