using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITickable
{
    void OnTick();
    void OnUnTick();

    bool IsThicked {get; set;}
}
