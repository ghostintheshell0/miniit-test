using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using VContainer;
using VContainer.Unity;

namespace miniIT.Arcanoid
{
    public class LevelController : IDisposable
    {
        public event Action Started = default;
        public event Action Completed = default;
        public event Action Failed = default;
        public event Action<Ball> BallLost = default;
        public event Action<Brick> BrickDestroyed = default;
        public event Action<float> BallsSpeedChanged = default;

        [SerializeField]
        private float ballsSpeed = 4f;

        [SerializeField]
        private float ballsSize = 1f;


        [SerializeField]
        private LevelData levelData = default;
        
        private Platform playerPlatform = default;

        private List<Brick> destructableBricks = default;
        private List<Ball> balls = default;
        private List<BaseBonus> bonuses = default;

        private IObjectResolver resolver = default;
        private GameController gameController = default;
        private PlayerInput playerInput = default;
        private Player player = default;
        private VFXSpawner vfxSpawner = default;
        private AudioSystem audioSystem = default;
        private MusicPlayer musicPlayer = default;

        private bool isStarted = false;

        [Inject]
        public void Inject(IObjectResolver resolver)
        {
            this.resolver = resolver;
        }

        public void Start()
        {
            gameController = resolver.Resolve<GameController>();
            levelData = resolver.Resolve<LevelData>();
            playerInput = resolver.Resolve<PlayerInput>();
            vfxSpawner = resolver.Resolve<VFXSpawner>();
            musicPlayer = resolver.Resolve<MusicPlayer>();
            audioSystem = resolver.Resolve<AudioSystem>();
            musicPlayer.Play(levelData.LevelValues.playlist);
            HUD hud = resolver.Resolve<HUD>();
            hud.Show();

            AddInputListeners(playerInput);

            balls = new List<Ball>();
            bonuses = new List<BaseBonus>();
            player = resolver.Resolve<PlayerService>().GetPlayer();

            InitBricks();
            SpawnPlatform(player);
            SpawnBall(player.InitData.startBallPrefab);
            levelData.CameraFitter.Fit(levelData.MainCamera, levelData);

            Time.timeScale = 1f;

            levelData.KillZone.ObjectEntered += KillZoneEnterListener;
        }

        private void InitBricks()
        {
            destructableBricks = new List<Brick>();

            foreach(Brick brick in levelData.bricks)
            {
                if(!brick.canBeIgnored)
                {
                    destructableBricks.Add(brick);
                    brick.Dead += BrickDestroyListener;
                    brick.Init(resolver);
                }
            }
        }

        private void AddInputListeners(PlayerInput playerInput)
        {
            this.playerInput = playerInput;
            playerInput.OnFire += Fire;
            playerInput.OnPointerPosition += MovePlayer;
        }

        private void RemoveInputListeners()
        {
            playerInput.OnFire -= Fire;
            playerInput.OnPointerPosition -= MovePlayer;
        }

        private void KillZoneEnterListener(Collider2D collider)
        {
            if(collider.TryGetComponent(out Ball ball))
            {
                balls.Remove(ball);
                BallLost?.Invoke(ball);
                CheckLose();
            }
        }

        private void BrickDestroyListener(Brick brick)
        {
            brick.Dead -= BrickDestroyListener;
            player.Scores += brick.pointPerKill;
            destructableBricks.Remove(brick);
            
            BrickDestroyed?.Invoke(brick);
            CheckWin();
        }

        public void SpawnPlatform(Player player)
        {
            playerPlatform = resolver.Instantiate(player.InitData.platformPrefab, levelData.SpawnPoint.position, levelData.SpawnPoint.rotation);
            playerPlatform.Size = levelData.LevelValues.startPlatformSize;
        }

        public void SpawnBall(Ball ballPrefab)
        {
            Ball ball = playerPlatform.SpawnBall(ballPrefab);
            ball.Speed = ballsSpeed;
            balls.Add(ball);
        }

        private void Respawn()
        {
            SpawnBall(player.InitData.startBallPrefab);
            ResetValues();
        }

        private void ResetValues()
        {
            BallsSize = levelData.LevelValues.startBallsSize;
            BallsSpeed = levelData.LevelValues.startBallsSpeed;
            playerPlatform.Size = levelData.LevelValues.startPlatformSize;
        }
        
        private void Fire()
        {
            if(playerPlatform == default)
            {
                return;
            }

            if(!isStarted)
            {
                isStarted = true;
                playerPlatform.LaunchBalls();
                Started?.Invoke();
            }
            else
            {
                playerPlatform.LaunchBalls();
            }
        }

        private void MovePlayer(Vector2 screenPosition)
        {
            Vector3 position = levelData.MainCamera.ScreenToWorldPoint(screenPosition);
            playerPlatform.MoveTo(position);
        }

        private void CheckLose()
        {
            if(balls.Count == 0)
            {
                player.Lifes--;
                if(player.Lifes >= 1)
                {
                    Respawn();
                }
                else
                {
                    audioSystem.Play(levelData.LevelValues.loseSound);
                    isStarted = false;
                    Failed?.Invoke();
                }
            }
        }

        public float BallsSize
        {
            get => ballsSize;
            set
            {
                ballsSize = Mathf.Clamp(value, levelData.LevelValues.minBallsSize, levelData.LevelValues.maxBallsSize);
                for(int i = 0; i < balls.Count; i++)
                {
                    balls[i].Scale = ballsSize;
                }
            }
        }


        private void CheckWin()
        {
            if(destructableBricks.Count == 0)
            {
                isStarted = false;
                audioSystem.Play(levelData.LevelValues.winSound);
                Completed?.Invoke();
            }
        }

        public void Dispose()
        {
            RemoveInputListeners();
            vfxSpawner.ReturnAll();
        }

        public float BallsSpeed
        {
            get => ballsSpeed;
            set
            {
                ballsSpeed = Mathf.Clamp(value, levelData.LevelValues.minBallsSpeed, levelData.LevelValues.maxBallsSpeed);
                for(int i = 0; i < balls.Count; i++)
                {
                    balls[i].Speed = ballsSpeed;
                }
                BallsSpeedChanged?.Invoke(ballsSpeed);
            }
        }

        public Platform PlayerPlatform => playerPlatform;
        public Player Player => player;
        public IList<Ball> Balls => balls;
        public bool IsStarted => isStarted;
    }
}