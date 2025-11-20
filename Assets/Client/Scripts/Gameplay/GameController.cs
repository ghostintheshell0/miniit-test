using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace miniit.Arcanoid
{
    public class GameController : MonoBehaviour
    {
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

        private Player player = default;
        private List<Ball> balls = default;


        private void Start()
        {
            StartGame();
            killZone.ObjectEntered += KillZoneEnterListener;
            playerInput.Fire.performed += OnFire;
            playerInput.Fire.Enable();
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

        private void CheckLose()
        {
            if(balls.Count == 0)
            {
                Debug.Log("Lose");
            }
        }
    }
}