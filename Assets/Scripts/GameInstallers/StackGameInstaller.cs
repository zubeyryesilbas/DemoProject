using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
public class StackGameInstaller : MonoInstaller
{   
    [SerializeField] private TapController _tapController;
    [SerializeField] private StackSoundController _stackSoundController;
    public override void InstallBindings()
    {
        Container.Bind<TapController>().FromInstance(_tapController);
        Container.Bind<StackSoundController>().FromInstance(_stackSoundController);
    }
}
