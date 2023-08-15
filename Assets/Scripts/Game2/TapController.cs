using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TapController : MonoBehaviour
{   
    public Action OnTap;
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            OnTap.Invoke();
        }
    }
}
