using UnityEngine;

namespace miniit.Arcanoid
{
    public class ScaleBallBonus : BaseBonus
    {
        public float Multipler;

        protected override void Apply(Platform platform)
        {
            platform.player.BallsSize *= Multipler;
        }
    }
}