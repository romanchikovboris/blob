using UnityEngine;
using Zenject;

namespace BlobGame
{
    public class ProjectInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Debug.Log("install");
            
#if UNITY_EDITOR
            Container.Bind<IInputService>().To<StandaloneInputService>().AsSingle();
#else
            Container.Bind<IInputService>().To<MobileInputService>().AsSingle();
#endif

        }
    }
}

