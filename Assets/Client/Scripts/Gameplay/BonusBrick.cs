namespace miniit.Arcanoid
{
    public class BonusBrick : Brick
    {
        public BonusesList bonusesList;

        protected override void Die()
        {
            var prefab = bonusesList.GetRandomBonus();
            Instantiate(prefab, transform.position, prefab.transform.rotation);
            base.Die();
        }
    }
}