using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
public class StackGameInstaller : MonoInstaller
{   
    [SerializeField] private TapController _tapController;
    [SerializeField] private StackSoundController _stackSoundController;
    [SerializeField] private StackController _stackController;
    [SerializeField] private StackColorManager _stackColorManager;
    [SerializeField] private GameController _gameController;
    [SerializeField] private Character _character;
    [SerializeField] private CameraController _cameraController;
    [SerializeField] private GameUiController _gameUiController;
    public override void InstallBindings()
    {
        Container.Bind<TapController>().FromInstance(_tapController);
        Container.Bind<StackSoundController>().FromInstance(_stackSoundController);
        Container.Bind<StackController>().FromInstance(_stackController);
        Container.Bind<StackColorManager>().FromInstance(_stackColorManager);
        Container.Bind<GameController>().FromInstance(_gameController);
        Container.Bind<Character>().FromInstance(_character);
        Container.Bind<CameraController>().FromInstance(_cameraController);
        Container.Bind<GameUiController>().FromInstance(_gameUiController);
    }
}
