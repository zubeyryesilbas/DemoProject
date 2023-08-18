using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using Zenject;
public class GameController : MonoBehaviour
{   
    private Character _character;
    private StackController _stackController;
    private CameraController _cameraController;
    private GameUiController _gameUiController;
    private TapController _tapController;
    
    
    [Inject]
    public  void Construct(Character character , StackController stackController , CameraController cameraController , GameUiController gameUiController , TapController tapController) 
    {
        this._character = character;
        this._stackController = stackController;
        this._cameraController = cameraController;
        this._gameUiController = gameUiController;
        this._tapController = tapController;
    }
    public async void RestartLevel()
    {   
        _gameUiController.ShowLevelLost();
        await Task.Delay(Mathf.FloorToInt(4000)); 
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);
    }
    public async void StartNextLevel()
    {
        _cameraController.ActivateFollowCamera();
         _stackController.CreateNewStack();
         await Task.Delay(Mathf.FloorToInt(700));
        _character.StartMovement();
        _tapController.EnableTap();
       
    }
    
    public void LevelSuccess()
    {
        _stackController.CreateNewLevelsPlatform();
        _character.SetLevelSuccessAction();
        _cameraController.ActivateRotationCamera();
        _gameUiController.ShowLevelWin();
    }
}
