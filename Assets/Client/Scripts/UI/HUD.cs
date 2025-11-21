using System;
using Doozy.Runtime.UIManager;
using Doozy.Runtime.UIManager.Components;
using TMPro;
using UnityEngine;

namespace miniit.Arcanoid
{
    public class HUD : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text scores = default;
        [SerializeField]
        private string speedFormat = "{0:F2}";
        [SerializeField]
        private TMP_Text speed = default;
        [SerializeField]
        private TMP_Text lifes = default;
        [SerializeField]
        private UIButton pause = default;

        public void ShowScores(int scores)
        {
            this.scores.text = scores.ToString();
        }

        public void ShowSpeed(float speed)
        {
            this.speed.text = string.Format(speedFormat, speed);
        }

        public void ShowLifes(int lifes)
        {
            this.lifes.text = lifes.ToString();
        }

        public void AddPauseClickListener(Action action)
        {
            UIBehaviour b = pause.behaviours.AddBehaviour(UIBehaviour.Name.PointerClick);
            b.Event = new UnityEngine.Events.UnityEvent();
            b.Event.AddListener(action.Invoke);
        }
    }
}