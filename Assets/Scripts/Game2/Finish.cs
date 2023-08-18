using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Zenject;

public class Finish : MonoBehaviour
{   
    private GameController _gameController;
    private Character _character;
    private StackController _stackController;
    private CameraController _cameraController;
    private GameUiController _gameUiController;
    [Inject]
    public  void Construct(GameController gameController ) 
    {
        this._gameController = gameController;
    }  
    private void OnTriggerEnter(Collider collider)
    {
        if(collider.CompareTag("player") && _gameController != null)
        {   
            _gameController.LevelSuccess();      
        }
    }
}
