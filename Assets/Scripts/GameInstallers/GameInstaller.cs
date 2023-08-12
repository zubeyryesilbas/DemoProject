using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class GameInstaller : MonoInstaller
{
    [SerializeField] private GridController _gridController;
    public override void InstallBindings()
    {
        Container.Bind<GridController>().FromInstance(_gridController);
    }
}
