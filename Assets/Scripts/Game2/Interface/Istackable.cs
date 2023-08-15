using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public  interface Istackable
{
    void OnStack(Vector3 postion);
    void SetMaterialColor(Color color);
    void SetScale(Vector3 scale);
    Transform StackableTransform{get;}

}
