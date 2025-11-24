using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace miniIT.Arcanoid
{
    public class VFXSpawner : MonoBehaviour
    {
        private Dictionary<VFXInstance, Queue<VFXInstance>> pools;
        private List<(VFXInstance instance, VFXInstance prefab)> activeEffects;
        private IObjectResolver resolver = default;
        private Coroutine checkLifetimeProcess = default;

        [Inject]
        public void Inject(IObjectResolver resolver)
        {
            this.resolver = resolver;
            pools = new Dictionary<VFXInstance, Queue<VFXInstance>>();
            activeEffects = new List<(VFXInstance instance, VFXInstance prefab)>();
        }

        public VFXInstance Spawn(VFXInstance prefab, Vector3 position)
        {
            VFXInstance instance = GetFromPool(prefab, position);
            instance.Play();

            activeEffects.Add((instance, prefab));
            if(checkLifetimeProcess == default)
            {
                checkLifetimeProcess = StartCoroutine(CheckLifetimeProcess());
            }
            return instance;
        }

        private VFXInstance GetFromPool(VFXInstance prefab, Vector3 position)
        {
            if (!pools.ContainsKey(prefab))
                pools[prefab] = new Queue<VFXInstance>();

            VFXInstance instance;
            if (pools[prefab].Count > 0)
            {
                instance = pools[prefab].Dequeue();
                instance.gameObject.SetActive(true);
            }
            else
            {
                instance = resolver.Instantiate(prefab);
            }

            instance.transform.position = position;
            return instance;
        }

        private IEnumerator CheckLifetimeProcess()
        {
            while(activeEffects.Count > 0)
            {
                for(int i = activeEffects.Count-1; i >= 0; i--)
                {
                    (VFXInstance instance, VFXInstance prefab) pair = activeEffects[i];

                    if(!pair.instance.IsAlive())
                    {
                        ReturnToPool(pair.instance, pair.prefab);
                    }
                }   
                
                yield return default;
            }

            checkLifetimeProcess = default;
        }


        private void ReturnToPool(VFXInstance instance, VFXInstance prefab)
        {
            instance.gameObject.SetActive(false);
            pools[prefab].Enqueue(instance);
            activeEffects.Remove((instance, prefab));
        }

        public void ReturnAll()
        {
            foreach ((VFXInstance instance, VFXInstance prefab) in activeEffects)
            {
                if (instance != null)
                {
                    instance.gameObject.SetActive(false);
                    pools[prefab].Enqueue(instance);
                }
            }
            activeEffects.Clear();
        }
    }
}