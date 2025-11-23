using System.ComponentModel;
using UnityEngine;
using UnityEngine.EventSystems;
using VContainer;
using VContainer.Unity;

namespace miniIT.Arcanoid
{
    public class ProjectScope : LifetimeScope
    {
        [SerializeField]
        private GameConfig gameConfig = default;
        [SerializeField]
        private InitPlayerData initPlayerData = default;
        [SerializeField]
        private EventSystem eventsSystemPrefab = default;
        [SerializeField]
        private WinScreen winScreenPrefab = default;
        [SerializeField]
        private LoseScreen loseScreenPrefab = default;
        [SerializeField]
        private HUD hudPrefab = default;

        protected override void Configure(IContainerBuilder builder)
        {
            Debug.Log("??");
            builder.RegisterComponentInNewPrefab(eventsSystemPrefab, Lifetime.Singleton).DontDestroyOnLoad();
            builder.RegisterComponentInNewPrefab(hudPrefab, Lifetime.Singleton).DontDestroyOnLoad();
            builder.RegisterComponentInNewPrefab(winScreenPrefab, Lifetime.Singleton).DontDestroyOnLoad();
            builder.RegisterComponentInNewPrefab(loseScreenPrefab, Lifetime.Singleton).DontDestroyOnLoad();
            builder.RegisterInstance(gameConfig);
            builder.RegisterInstance(initPlayerData);
            builder.Register<GameController>(Lifetime.Singleton);

            builder.RegisterBuildCallback(OnBuild);
        }

        private void OnBuild(IObjectResolver resolver)
        {
            resolver.Resolve<EventSystem>();
              //  var instance = resolver.Resolve<WinScreen>();//No such registration of type: with Key: 
                Debug.Log("Project scope builder");
        }
    }
}