namespace miniIT.Arcanoid
{
    public class PlatformSizeBonus : BaseBonus
    {
        public float Bonus;

        protected override void Apply(Platform platform)
        {
            platform.Size += Bonus;
        }
    }
}