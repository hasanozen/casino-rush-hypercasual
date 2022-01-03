using Game.CharacterSystem.Base;
using Game.ChipSystem.Base;
using Game.ChipSystem.Managers;
using Game.LevelSystem.Managers;
using Game.PoolingSystem;
using UnityEngine;
using Zenject;

namespace Installers
{
    public class GameInstaller : MonoInstaller
    {
        [SerializeField] private CharacterBase mainCharacter;
        [SerializeField] private ObjectPooler objectPooler;
        [SerializeField] private ChipManager chipManager;
        [SerializeField] private LevelGenerator levelGenerator;

        public override void InstallBindings()
        {
            Container.BindInstance(mainCharacter).AsSingle().NonLazy();
            Container.BindInstance(objectPooler).AsSingle().NonLazy();
            Container.BindInstance(chipManager).AsSingle().NonLazy();
            Container.BindInstance(levelGenerator).AsSingle().NonLazy();

            Container.Bind<AssetManager>().AsSingle().NonLazy();
        }
    }
}
