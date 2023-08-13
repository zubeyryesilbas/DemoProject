using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Rendering;
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
        if (_gridHolderTransform == null)
        {
            _gridHolderTransform = new GameObject("Grid Holder").transform;
            _gridHolderTransform.SetParent(transform);
        }

        _tilesInGrid = new Tile [gridSize, gridSize];
        for (var i = 0; i < gridSize; i++)
        {
            for (var j = 0; j < gridSize; j++)
            {
                var tile = Instantiate(_tilePrefab, _gridHolderTransform);
                var posX = i * _tileSize;
                var posY = j * _tileSize;
                tile.Coordinate = new Vector2Int(i, j);
                tile.name = ("tile " + i + " ," + j);
                tile.transform.position = new Vector2(posX, posY);
                _tilesInGrid[i, j] = tile;
            }
        }
    }

    public void RefleshGrid(int gridSize)
    {
        foreach (var tile in _tilesInGrid)
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
        foreach (var tile in _tilesInGrid)
        {
            centerPos += tile.transform.position;
        }

        centerPos /= (_tilesInGrid.GetLength(0) * _tilesInGrid.GetLength(1));

        return centerPos;
    }

    public void CheckMatchesOnGrid(Tile tile)
    {
        var referencedNeighborChainList = new List<Tile>();
        GetThickedNeighboursOfATile(referencedNeighborChainList, tile);
        Debug.Log(referencedNeighborChainList.Count);
        if (referencedNeighborChainList.Count > 2)
        {
            foreach (var neighbortile in referencedNeighborChainList)
            {
                neighbortile.OnUnTick();
            } 
        }
            
    }

    private void GetThickedNeighboursOfATile(List<Tile> neighborList, Tile tile)
    {
        var gridSize = _tilesInGrid.GetLength(0);
        var upCoordinate = new Vector2Int(tile.Coordinate.x, tile.Coordinate.y + 1);
        var downCoordinate = new Vector2Int(tile.Coordinate.x, tile.Coordinate.y - 1);
        var leftCoordinate = new Vector2Int(tile.Coordinate.x - 1, tile.Coordinate.y);
        var rightCoordinate = new Vector2Int(tile.Coordinate.x + 1, tile.Coordinate.y);
        var hasNewNeighbor = false;
        var newNeighbors = new List<Tile>();
        if (upCoordinate.y < gridSize)
        {
            var upTile = _tilesInGrid[upCoordinate.x, upCoordinate.y];
            if (upTile.IsThicked && !neighborList.Contains(upTile))
            {
                hasNewNeighbor = true;
                neighborList.Add(upTile);
                newNeighbors.Add((upTile));
            }
        }

        if (downCoordinate.y >= 0)
        {
            var downTile = _tilesInGrid[downCoordinate.x, downCoordinate.y];
            if (downTile.IsThicked && !neighborList.Contains(downTile))
            {
                hasNewNeighbor = true;
                neighborList.Add(downTile);
                newNeighbors.Add(downTile);
            }
        }

        if (leftCoordinate.x >= 0)
        {
            var leftTile = _tilesInGrid[leftCoordinate.x, leftCoordinate.y];
            if (leftTile.IsThicked && !neighborList.Contains(leftTile))
            {
                hasNewNeighbor = true;
                neighborList.Add(leftTile);
                newNeighbors.Add(leftTile);
            }
        }

        if (rightCoordinate.x < gridSize)
        {
            var rightTile = _tilesInGrid[rightCoordinate.x, rightCoordinate.y];
            if (rightTile.IsThicked && !neighborList.Contains(rightTile))
            {
                hasNewNeighbor = true;
                neighborList.Add(rightTile);
                newNeighbors.Add(rightTile);
            }

            if (hasNewNeighbor)
            {
                foreach (var neighborTile in newNeighbors)
                {
                    GetThickedNeighboursOfATile(neighborList, neighborTile);
                }
            }
            else
            {
                return;
            }

            if (neighborList.Count > 0 && !neighborList.Contains(tile))
                neighborList.Add(tile);
        }
    }
}
