using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridController : MonoBehaviour
{   
    [SerializeField] private int _gridSize;
    [SerializeField] private float _tileSize;
    [SerializeField] private Tile _tilePrefab;
    [SerializeField] private Tile[,] _tilesInGrid;
    private Transform _gridHolderTransform;
    
    private void Start()
    {
        GenerateGrid(_gridSize);
    }

    private void GenerateGrid(int gridSize)
    {   
        if(_gridHolderTransform == null)
         {
            _gridHolderTransform = new GameObject("Grid Holder").transform;
            _gridHolderTransform.SetParent(transform);
         }

        _tilesInGrid = new Tile [gridSize, gridSize];
        for (var i = 0; i < gridSize; i++)
        {
            for (var j = 0 ; j < gridSize; j++)
            {
                var tile = Instantiate(_tilePrefab, _gridHolderTransform);
                var posX = i * _tileSize;
                var posY = j * _tileSize;

                tile.transform.position = new Vector2(posX, posY);
                _tilesInGrid [i , j] = tile;
            }
        }
    }

    public void RefleshGrid(int gridSize)
    {
        foreach(var tile in _tilesInGrid)
        {
            Destroy(tile.gameObject);
        }
        GenerateGrid(gridSize);   
    }
    public int GetSizeOfGrid()
    {
        return _tilesInGrid.GetLength(0);
    }
    public Vector3 GetCenterPointOfGrid()
    {
        var centerPos = new Vector3();
        foreach(var tile in _tilesInGrid)
        {
            centerPos += tile.transform.position;
        }

        centerPos /= (_tilesInGrid.GetLength(0) * _tilesInGrid.GetLength(1));

        return centerPos;
    }

    private void CheckMatchesOnGrid()
    {

    }

    /*private  List<Tile> Neighbours(Tile tile)
    {   
        var gridSize = _tilesInGrid.GetLength(0);
        var upCoordinate = new Vector2Int(tile.Coordinate.x  , tile.Coordinate.y + 1);
        var downtCoordinate = new Vector2Int(tile.Coordinate.x , tile.Coordinate.y - 1);
        var neighbors = new List<Tile>();

        if(tile.Coordinate.x > 0)
        {   
            var upTile = _tilesInGrid[upCoordinate.x , upCoordinate.y];
            if(tile.IsThicked)
                neighbors.Add(upTile);
        }

        if(tile.Coordinate.x < gridSize - 1)
        {   
            var downTile = _tilesInGrid[downtCoordinate.x , downtCoordinate.y];
            if(tile.IsThicked)
                neighbors.Add(downTile);
        }
        return neighbors;
    }*/

}
