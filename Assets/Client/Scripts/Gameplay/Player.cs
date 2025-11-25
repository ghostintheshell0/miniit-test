using System;

namespace miniIT.Arcanoid
{
    [Serializable]
    public class Player
    {
        public event Action<int> ScoresChanged = default;
        public event Action<int> LifesChanged = default;
        public event Action Lost = default;

        private InitPlayerData initData = default;

        private int lifes = 3;
        private int scores = 0;
        public int level = 0;

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
                    LifesChanged?.Invoke(lifes);
                    Lost?.Invoke();
                }
                else
                {
                    LifesChanged?.Invoke(lifes);
                }

            }
        }

        public InitPlayerData InitData
        {
            get => initData;
            set => initData = value;
        }
    }
}