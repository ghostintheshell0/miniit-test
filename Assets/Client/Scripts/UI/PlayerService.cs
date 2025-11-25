using UnityEngine;
using UnityEngine.AddressableAssets;
using VContainer;

namespace miniIT.Arcanoid
{
    public class PlayerService
    {
        [Inject]
        private InitPlayerData initPlayerData = default;

        [Inject]
        private GameConfig gameConfig = default;

        private Player player = default;

        public Player GetPlayer()
        {
            if(player == default)
            {
                player = CreatePlayer();
            }

            return player;
        }

        private Player CreatePlayer()
        {
            Player player = new Player()
            {
                Lifes = initPlayerData.lifes,
                Scores = initPlayerData.startScores,
                level = initPlayerData.startLevel,
                InitData = initPlayerData,
            };

            return player;
        }

        public void ResetPlayer()
        {
            if(player == default)
            {
                return;
            }
            player.Lifes = initPlayerData.lifes;
            player.Scores = initPlayerData.startScores;
            player.level = initPlayerData.startLevel;
        }

        public AssetReference GetLevel()
        {
            int id = Mathf.Min(GetPlayer().level, gameConfig.levels.Count);
            AssetReference levelRef = gameConfig.levels[id];
            return levelRef;
        }
    }
}