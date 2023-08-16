using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using DG.Tweening;
using Unity.VisualScripting;

public class Stack : MonoBehaviour, Istackable
{
   [SerializeField] private MeshRenderer _renderer;
    private Rigidbody _rb;

    public Transform StackableTransform => transform;

    public void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.isKinematic = true;
    }
    
    public void SetUnKinematic()
    {
        _rb.isKinematic = false;
    }
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
