using VContainer;

namespace miniIT.Arcanoid
{
    public class PlayerService
    {
        [Inject]
        private InitPlayerData initPlayerData = default;

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

        }
    }
}