using System.Collections.Generic;
using UnityEngine;

namespace miniIT.Arcanoid
{
    [CreateAssetMenu(menuName = "Game/Bonuses list")]
    public class BonusesList : ScriptableObject
    {
        public List<GameObject> list;
        public GameObject GetRandomBonus()
        {
            return list[Random.Range(0, list.Count)];
        }
    }
}