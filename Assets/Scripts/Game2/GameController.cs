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
        await Task.Delay(Mathf.FloorToInt(1000)); 
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public async void StartNextLevel()
    {
        _cameraController.ActivateFollowCamera();
        await Task.Delay(Mathf.FloorToInt(1500));
        _character.StartMovement();
        await Task.Delay(Mathf.FloorToInt(300));
        _stackController.CreateNewStartPlatfom();
        _stackController.CreateNewStack();
        _tapController.EnableTap();
       
    }
    
    public void LevelSuccess()
    {
        _stackController.CreateNewLevelsPlatform();
        _character.SetLevelSuccessAction();
        _cameraController.ActivateRotationCamera();
        _gameUiController.ShowLevelWin();
    }

    public void LevelLost()
    {
       _gameUiController.ShowLevelLost();
    }
}
