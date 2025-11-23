using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace miniIT.Arcanoid
{
    public class BonusBrick : Brick
    {
        public BonusesList bonusesList;

        [Inject]
        public override void Init(IObjectResolver resolver)
        {
            base.Init(resolver);
        }

        protected override void Die()
        {
            GameObject prefab = bonusesList.GetRandomBonus();
            resolver.Instantiate(prefab, transform.position, prefab.transform.rotation);
            base.Die();
        }
    }
}