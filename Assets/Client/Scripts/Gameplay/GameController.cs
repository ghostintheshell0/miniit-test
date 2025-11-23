using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using VContainer;

namespace miniIT.Arcanoid
{
    public class GameController
    {
        public event Action<int> lost = default;
        public event Action<int> won = default;
        public event Action started = default;
        public event Action lostBalls = default;
        public event Action paused = default;
        public event Action resumed = default;


        private GameConfig gameConfig = default;
        private WinScreen winScreen = default;
        private LoseScreen loseScreen = default;
        private HUD hud = default;

        private Player player = default;
        private LevelController levelController = default;
        private IObjectResolver resolver = default;

        [Inject]
        public void Inject(IObjectResolver resolver)
        {
            InitPlayerData initPlayerData = resolver.Resolve<InitPlayerData>();
            player = new Player(initPlayerData);
            this.resolver = resolver;
            hud = resolver.Resolve<HUD>();
            hud.Bind(this);
            hud.SetPlayer(player);
            gameConfig = resolver.Resolve<GameConfig>();
        }

        public void Bind(LevelController levelController)
        {
            this.levelController = levelController;
            levelController.Completed += ShowWinScreen;
            levelController.Failed += ShowLoseScreen;
        }

        private void ShowWinScreen()
        {
            Time.timeScale = 0f;
            GameController gameController = resolver.Resolve<GameController>();
            winScreen = resolver.Resolve<WinScreen>();
            winScreen.Bind(gameController);
            winScreen.Show(player.Scores);
        }

        private void ShowLoseScreen()
        {
            Time.timeScale = 0f;
            GameController gameController = resolver.Resolve<GameController>();
            loseScreen = resolver.Resolve<LoseScreen>();
            loseScreen.Bind(gameController);
            loseScreen.Show(player.Scores);
        }

        /*
       private void Win()
       {
           Pause();
           won?.Invoke(player.Scores);
           Debug.Log("Win");
       }*/

        public void Pause()
        {
            if(levelController.IsStarted)
            {
                Time.timeScale = 0f;
                paused?.Invoke();
            }
        }

        public void Resume()
        {
            if(levelController.IsStarted)
            {
                Time.timeScale = 1f;
                resumed?.Invoke();
            }
        }

        public void ToMenu()
        {
            SceneManager.LoadScene(gameConfig.menuSceneName);
        }

        public void NextLevel()
        {
            SceneManager.LoadScene(gameConfig.menuSceneName);
        }

        public Player Player => player;
    }
}