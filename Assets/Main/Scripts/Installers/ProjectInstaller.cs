using UnityEngine;
using Zenject;

namespace BlobGame
{
    public class ProjectInstaller : MonoInstaller
    {
        [SerializeField]
        private GameConfig _gameConfig;
        
        public override void InstallBindings()
        {
            Container.Bind<GameConfig>().FromInstance(_gameConfig).AsSingle().NonLazy();

#if UNITY_EDITOR
            Container.Bind<IInputService>().To<StandaloneInputService>().AsSingle();
#else
            Container.Bind<IInputService>().To<MobileInputService>().AsSingle();
#endif

        }
    }
}

