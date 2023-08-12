using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridController : MonoBehaviour
{   
    [SerializeField] private int _gridSizeX, _gridSizeY;
    [SerializeField] private float _tileSize;
    [SerializeField] private GameObject _tilePrefab;
    private Tile[,] _tilesInGrid;
    
    private void GenerateGrid()
    { 
        _tilesInGrid = new Tile [_gridSizeX , _gridSizeY];
        for (var i = 0; i < _gridSizeX; i++)
        {
            for (var j = 0 ; j < _gridSizeY; j++)
            {
                var tile = Instantiate(_tilePrefab, transform);
                var posX = i * _tileSize;
                var posY = j * _tileSize;

                tile.transform.position = new Vector2(posX, posY);
            }
        }
    }
}
