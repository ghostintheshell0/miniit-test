using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace miniIT.Arcanoid
{
    public class SceneScope : LifetimeScope
    {
        public PlayerInput inputPrefab = default;
        public Camera mainCamera = default;
        public BallsSpeedChanger ballsSpeedChanger = default;
        public LevelData level = default;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterComponent(level);
            builder.RegisterComponent(mainCamera);
            builder.RegisterComponent(ballsSpeedChanger);
            builder.RegisterComponentInNewPrefab(inputPrefab, Lifetime.Scoped);
            builder.Register<LevelController>(Lifetime.Scoped);

            builder.RegisterBuildCallback(OnBuild);
        }

        private void OnBuild(IObjectResolver resolver)
        {
            GameController gameController = resolver.Resolve<GameController>();
            LevelController levelController = resolver.Resolve<LevelController>();
            HUD hud = resolver.Resolve<HUD>();
            hud.SetLevelController(levelController);
            gameController.Bind(levelController);
            levelController.Start();
        }
    }
}