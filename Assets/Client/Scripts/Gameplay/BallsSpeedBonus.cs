namespace miniit.Arcanoid
{
    public class BallsSpeedBonus : BaseBonus
    {
        public float Bonus;

        protected override void Apply(Platform platform)
        {
            platform.player.BallsSpeed += Bonus;
        }
    }
}