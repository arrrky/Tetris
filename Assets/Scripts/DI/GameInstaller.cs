using Zenject;
using UnityEngine;
public class GameInstaller : MonoInstaller
{
    public override void InstallBindings()
    {        
        Container.Bind<Elements>().AsSingle();        
    }
}