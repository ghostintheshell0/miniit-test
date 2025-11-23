using VContainer;

namespace miniIT.Arcanoid
{
    public class BallsSpeedBonus : BaseBonus
    {
        public float Bonus = 1f;

        private LevelController levelController = default;
        
        [Inject]
        public override void Inject(IObjectResolver resolver)
        {
            levelController = resolver.Resolve<LevelController>();
        }

        protected override void Apply(Platform platform)
        {
            levelController.BallsSpeed += Bonus;
        }
    }
}