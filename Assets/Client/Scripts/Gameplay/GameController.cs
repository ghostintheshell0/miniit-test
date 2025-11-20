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
        public event Action<int> Lost;
        public event Action<int> Won;

        [SerializeField]
        private Player playerPrefab = default;

        [SerializeField]
        private Transform spawnPoint = default;

        [SerializeField]
        private Ball startBallPrefab = default;

        [SerializeField]
        private PlayerInput playerInput = default;

        [SerializeField]
        private Camera mainCamera = default;

        [SerializeField]
        private KillZone killZone = default;

        [SerializeField]
        private Level level = default;
        [SerializeField]
        private int scores = 0;

        [SerializeField][Scene]
        private string MenuScene;

        private Player player = default;
        private List<Ball> balls = default;
        private List<Brick> bricks = default;


        private void Start()
        {
            StartGame();
            killZone.ObjectEntered += KillZoneEnterListener;
            playerInput.Fire.performed += OnFire;
            playerInput.Fire.Enable();
            Time.timeScale = 1f;
        }

        private void OnFire(InputAction.CallbackContext context)
        {
            player.LaunchBalls();
        }

        private void StartGame()
        {
            balls = new List<Ball>();
            player = Instantiate(playerPrefab, spawnPoint.position, playerPrefab.transform.rotation);
            ICollection<Ball> newBalls = player.SpawnBalls(startBallPrefab);
            balls.AddRange(newBalls);

            bricks = new List<Brick>(level.bricks);
            for(int i = 0; i < bricks.Count; i++)
            {
                bricks[i].Dead += BrickDestroyListener;
            }

        }

        private void Update()
        {
            Vector3 position = mainCamera.ScreenToWorldPoint(playerInput.PointerPosition);
            player.MoveTo(position);
        }

        private void KillZoneEnterListener(Collider2D collider)
        {
            if(collider.TryGetComponent<Ball>(out Ball ball))
            {
                balls.Remove(ball);
                CheckLose();
            }
        }

        private void BrickDestroyListener(Brick brick)
        {
            brick.Dead -= BrickDestroyListener;
            bricks.Remove(brick);
            CheckWin();
        }

        private void CheckLose()
        {
            if(balls.Count == 0)
            {
                Lose();
            }
        }

        private void Lose()
        {
            Pause();
            Lost?.Invoke(scores);
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
            Won?.Invoke(scores);
            Debug.Log("Win");
        }

        private void Pause()
        {
            Time.timeScale = 0f;
        }

        public void ToMenu()
        {
            SceneManager.LoadScene(MenuScene);
        }
        public void NextLevel()
        {
            SceneManager.LoadScene(MenuScene);
        }
    }
}