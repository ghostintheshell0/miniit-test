using System;
using System.Collections.Generic;
using UnityEngine;

namespace miniit.Arcanoid
{
    [Serializable]
    public class Player
    {
        public event Action<int> ScoresChanged = default;
        public event Action<float> SpeedChanged = default;
        public event Action<int> LifesChanged = default;
        public event Action Lost = default;
        [SerializeField]
        private InitPlayerData initData = default;

        [SerializeField]
        private Platform platform = default;
        [SerializeField]
        private int scores = 0;
        private List<Ball> balls = default;
        [SerializeField]
        private int lifes = 3;
        [SerializeField]
        private float ballsSpeed = 4f;

        [SerializeField]
        private float ballsSize = 1f;

        public Player()
        {
            balls = new List<Ball>();
        }

        public void Init(InitPlayerData initData)
        {
            this.initData = initData;
            BallsSize = initData.startBallsSize;
            BallsSpeed = initData.startBallsSpeed;
            platform.Size = initData.startPlatformSize;
        }

        public float BallsSpeed
        {
            get => ballsSpeed;
            set
            {
                ballsSpeed = Mathf.Clamp(value, initData.minBallsSpeed, initData.maxBallsSpeed);
                for(int i = 0; i < balls.Count; i++)
                {
                    balls[i].Speed = ballsSpeed;
                }
                SpeedChanged?.Invoke(ballsSpeed);
            }
        }

        public int Scores
        {
            get => scores;
            set
            {
                scores = value;
                ScoresChanged?.Invoke(scores);
            }
        }

        public int Lifes
        {
            get => lifes;
            set
            {
                if(lifes == value)
                {
                    return;
                }

                lifes = value;
                if(lifes <= 0)
                {

                }

                LifesChanged?.Invoke(lifes);
                Lost?.Invoke();
            }
        }

        public float BallsSize
        {
            get => ballsSize;
            set
            {
                ballsSize = Mathf.Clamp(value, initData.minBallsSize, initData.maxBallsSize);
                for(int i = 0; i < balls.Count; i++)
                {
                    balls[i].Scale = ballsSize;
                }
            }
        }

        public Platform Platform
        {
            get => platform;
            set
            {
                platform = value;
            }
        }

        public IList<Ball> Balls => balls;

    }
}