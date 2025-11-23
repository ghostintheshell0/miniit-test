using UnityEngine;
using VContainer;

namespace miniIT.Arcanoid
{
    public class ScaleBallBonus : BaseBonus
    {
        public float Multipler = 1f;

        private LevelController levelController = default;
        
        [Inject]
        public override void Inject(IObjectResolver resolver)
        {
            base.Inject(resolver);
            levelController = resolver.Resolve<LevelController>();
        }

        protected override void Apply(Platform platform)
        {
            levelController.BallsSize *= Multipler;
        }
    }
}