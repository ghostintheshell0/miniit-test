using System;
using System.Collections.Generic;
using TriInspector;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace miniit.Arcanoid
{
    public class GameController : MonoBehaviour
    {
        public event Action<int> Lost = default;
        public event Action<int> Won = default;

        [SerializeField]
        private InitPlayerData initPlayerData = default;


        [SerializeField]
        private PlayerInput playerInput = default;

        [SerializeField]
        private Camera mainCamera = default;
        [SerializeField][Scene]
        private string MenuScene = default;

        [SerializeField]
        private Level level = default;

        private List<Brick> bricks = default;

        [SerializeField]
        private HUD hud;
        [SerializeField]
        private WinScreen winScreen;
        [SerializeField]
        private LoseScreen loseScreen;

        [SerializeReference]
        private Player player = default;


        private void Start()
        {
            StartGame();
            level.KillZone.ObjectEntered += KillZoneEnterListener;
            playerInput.Fire.performed += OnFire;
            playerInput.Fire.Enable();
            Time.timeScale = 1f;

            InitHud();
        }


        private void StartGame()
        {
            player = new Player();
            player.Platform = Instantiate(initPlayerData.platformPrefab, level.SpawnPoint.position, initPlayerData.platformPrefab.transform.rotation);
            player.Platform.player = player;
            player.Scores = initPlayerData.startScores;
            player.Lifes = initPlayerData.lifes;
            player.LifesChanged += LifesChangedListener;

            bricks = new List<Brick>(level.bricks);
            for(int i = 0; i < bricks.Count; i++)
            {
                bricks[i].Dead += BrickDestroyListener;
            }
            
            Continue();
            Respawn();
        }

        private void InitHud()
        {
            player.ScoresChanged += hud.ShowScores;
            hud.ShowScores(player.Scores);
            player.SpeedChanged += hud.ShowSpeed;
            hud.ShowSpeed(player.BallsSpeed);
            player.LifesChanged += hud.ShowLifes;
            hud.ShowLifes(player.Lifes);
            hud.AddPauseClickListener(Pause);
        }

        private void OnFire(InputAction.CallbackContext context)
        {
            player.Platform.LaunchBalls();
        }
        private void Update()
        {
            Vector3 position = mainCamera.ScreenToWorldPoint(playerInput.PointerPosition);
            player.Platform.MoveTo(position);
        }

        private void KillZoneEnterListener(Collider2D collider)
        {
            if(collider.TryGetComponent(out Ball ball))
            {
                Player.Balls.Remove(ball);
                CheckLose();
            }
        }

        private void BrickDestroyListener(Brick brick)
        {
            brick.Dead -= BrickDestroyListener;
            bricks.Remove(brick);
            player.Scores += brick.pointPerKill;
            CheckWin();
        }

        private void CheckLose()
        {
            if(player.Balls.Count == 0)
            {
                player.Lifes--;
                if(player.Lifes >= 1)
                {
                    Respawn();
                }
            }
        }

        private void LifesChangedListener(int lifes)
        {
            if(lifes <= 0)
            {
                Lose();
            }
        }

        private void Respawn()
        {
            Ball newBall = player.Platform.SpawnBall(initPlayerData.startBallPrefab);
            player.Balls.Add(newBall);
            player.Init(initPlayerData);
        }

        private void Lose()
        {
            Pause();
            Lost?.Invoke(player.Scores);
            Debug.Log("Lose");
        }

        private void CheckWin()
        {
            if(bricks.Count == 0)
            {
                Win();
            }
        }

        private void Win()
        {
            Pause();
            Won?.Invoke(player.Scores);
            Debug.Log("Win");
        }

        private void Pause()
        {
            Time.timeScale = 0f;
        }

        private void Continue()
        {
            Time.timeScale = 1f;
        }

        public void ToMenu()
        {
            SceneManager.LoadScene(MenuScene);
        }

        public void NextLevel()
        {
            SceneManager.LoadScene(MenuScene);
        }

        public Player Player
        {
            get => player;
        }
    }
}