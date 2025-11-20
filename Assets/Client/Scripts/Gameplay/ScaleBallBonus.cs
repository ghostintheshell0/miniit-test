using UnityEngine;

namespace miniit.Arcanoid
{
    public class ScaleBallBonus : BaseBonus
    {
        public float Multipler;

        protected override void Apply(Player player)
        {
            Debug.Log($"scale x{Multipler}");
        }
    }
}