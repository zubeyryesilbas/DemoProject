using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vector2 = System.Numerics.Vector2;

public class Tile : MonoBehaviour , ITickable
{
    [HideInInspector] public Vector2Int Coordinate;
    [SerializeField] private SpriteRenderer _crossSign;
    private void OnMouseDown()
    {
        
    }

    public void OnTick()
    {
        _crossSign.gameObject.SetActive(true);
    }

    public void OnUnTick()
    {   
        _crossSign.gameObject.SetActive(false);
    }
}
