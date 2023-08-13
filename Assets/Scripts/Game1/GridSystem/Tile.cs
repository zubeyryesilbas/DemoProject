using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vector2 = System.Numerics.Vector2;

public class Tile : MonoBehaviour , ITickable
{
    [HideInInspector] public Vector2Int Coordinate;
    [SerializeField] private SpriteRenderer _crossSign;
    private GridController _gridController;

    public bool IsThicked { get; set; }

    private void OnMouseDown()
    {
        if(!IsThicked)
            OnTick();
    }

    private void Start()
    {
        _gridController = FindObjectOfType<GridController>();
    }

    public void OnTick()
    {
        _crossSign.gameObject.SetActive(true);
        IsThicked = true;
        _gridController.CheckMatchesOnGrid(this);
    }

    public void OnUnTick()
    {
        IsThicked = false;
        _crossSign.gameObject.SetActive(false);
    }
}
