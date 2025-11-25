using VContainer;

namespace miniIT.Arcanoid
{
    public class LifeBonus : BaseBonus
    {
        public int Bonus = 1;

        private LevelController levelController = default;
        
        [Inject]
        public override void Inject(IObjectResolver resolver)
        {
            base.Inject(resolver);
            levelController = resolver.Resolve<LevelController>();
        }

        protected override void Apply(Platform platform)
        {
            levelController.Player.Lifes += Bonus;
        }
    }
}