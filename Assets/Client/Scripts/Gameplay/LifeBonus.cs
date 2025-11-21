namespace miniit.Arcanoid
{
    public class LifeBonus : BaseBonus
    {
        public int Bonus;

        protected override void Apply(Platform platform)
        {
            platform.player.Lifes += Bonus;
        }
    }
}