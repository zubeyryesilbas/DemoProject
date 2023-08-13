using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Rendering;
using UnityEngine;

public class GridController : MonoBehaviour
{
    [SerializeField] private Tile _tilePrefab;
    [SerializeField] private Tile[,] _tilesInGrid;
    private Transform _gridHolderTransform;

    private void Awake()
    {
        GenerateGrid(GridConstants.GridStartSize);
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
                var posX = i * GridConstants.TileSize;
                var posY = j * GridConstants.TileSize;
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
        var referencedNeighborChainList = GetThickedNeighboursOfATile(tile);
        Debug.Log(referencedNeighborChainList.Count);
    
        if (referencedNeighborChainList.Count >= GridConstants.NeighborDetectionNumber)
        {
            foreach (var neighbortile in referencedNeighborChainList)
            {
                neighbortile.OnUnTick();
            } 
        }
    }

    private HashSet<Tile> GetThickedNeighboursOfATile(Tile tile)
    {
        var neighborList = new HashSet<Tile>();
        var queue = new Queue<Tile>();

        var gridSize = _tilesInGrid.GetLength(0);
        queue.Enqueue(tile);

        while (queue.Count > 0)
        {
            var currentTile = queue.Dequeue();

            var directions = new Vector2Int[]
            {
                new Vector2Int(0, 1),  // Up
                new Vector2Int(0, -1), // Down
                new Vector2Int(-1, 0), // Left
                new Vector2Int(1, 0)   // Right
            };

            foreach (var direction in directions)
            {
                var neighborCoordinate = currentTile.Coordinate + direction;

                if (neighborCoordinate.x >= 0 && neighborCoordinate.x < gridSize &&
                    neighborCoordinate.y >= 0 && neighborCoordinate.y < gridSize)
                {
                    var neighborTile = _tilesInGrid[neighborCoordinate.x, neighborCoordinate.y];

                    if (neighborTile.IsThicked && neighborList.Add(neighborTile))
                    {
                        queue.Enqueue(neighborTile);
                    }
                }
            }
        }

        return neighborList;
    }
}
