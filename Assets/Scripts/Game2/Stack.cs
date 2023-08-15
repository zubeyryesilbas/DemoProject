using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using DG.Tweening;
public class Stack : MonoBehaviour, Istackable
{
   [SerializeField] private MeshRenderer _renderer;

    public Transform StackableTransform => transform;

    public void OnStack(Vector3 postion)
    {
        throw new System.NotImplementedException();
    }

    public void SetMaterialColor(Color color)
    {
        _renderer.material.color = color;
    }

    public void SetScale(Vector3 scale)
    {
        transform.localScale = scale;
    }
}
