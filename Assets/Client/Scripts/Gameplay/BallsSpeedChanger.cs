using System.Collections;
using UnityEngine;
using VContainer;

namespace miniIT.Arcanoid
{
    public class BallsSpeedChanger : MonoBehaviour
    {
        [SerializeField]
        private LevelController levelController = default;
        public float delay = 3.5f;
        public float speedBonus = 0.1f;

        private bool isStarted = false;
        private Coroutine process = default;

        [Inject]
        public void AddListeners(LevelController controller)
        {
            levelController = controller;
            controller.Started += Start;
            controller.Failed += Stop;
        }

        private IEnumerator Process()
        {
            isStarted = true;

            while(isStarted)
            {
                yield return new WaitForSeconds(delay);
                levelController.BallsSpeed += speedBonus;
            }

            process = default;
        }

        public void Start()
        {
            if(process != default)
            {
                return;
            }
            process = StartCoroutine(Process());
        }

        public void Stop()
        {
            isStarted = false;
        }
    }
}