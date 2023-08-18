using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StackColorManager : MonoBehaviour
{
    [SerializeField] private List<Material> _stackMaterials;
    private int _stackIndex;
    
    public Material GetMaterial()
    {
        var mat = _stackMaterials[_stackIndex];
        _stackIndex +=1;
        if(_stackIndex >= _stackMaterials.Count)
            _stackIndex = 0;
        
        return mat;
    }
}
