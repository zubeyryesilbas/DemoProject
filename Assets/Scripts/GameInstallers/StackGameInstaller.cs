using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
public class StackGameInstaller : MonoInstaller
{   
    [SerializeField] private TapController _tapController;
    public override void InstallBindings()
    {
        Container.Bind<TapController>().FromInstance(_tapController);
    }
}
