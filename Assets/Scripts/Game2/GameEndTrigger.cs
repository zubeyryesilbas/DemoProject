using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Zenject;

public class GameEndTrigger : MonoBehaviour
{   
    private GameController _gameController;
    [Inject]
    public  void Construct(GameController gameController) 
    {
        this._gameController = gameController;
    }
    private void OnTriggerEnter(Collider collider)
    {
        if(collider.CompareTag("player"))
        {
            _gameController.LevelLost();
        }
    }
}
