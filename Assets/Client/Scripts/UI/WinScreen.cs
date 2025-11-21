using Doozy.Runtime.UIManager;
using Doozy.Runtime.UIManager.Components;
using Doozy.Runtime.UIManager.Containers;
using TMPro;
using UnityEngine;

namespace miniit.Arcanoid
{
    public class WinScreen : MonoBehaviour
    {
        [SerializeField]
        private GameController gameController = default;
        [SerializeField]
        private UIView screen = default;
        [SerializeField]
        private TMP_Text scores = default;
        [SerializeField]
        private UIButton menu = default;
        [SerializeField]
        private UIButton next = default;

        private void Start()
        {
            gameController.Won += Show;

            UIBehaviour behaviour = menu.behaviours.AddBehaviour(UIBehaviour.Name.PointerClick);
            behaviour.Event = new UnityEngine.Events.UnityEvent();
            behaviour.Event.AddListener(gameController.ToMenu);

            behaviour = menu.behaviours.AddBehaviour(UIBehaviour.Name.PointerClick);
            behaviour.Event = new UnityEngine.Events.UnityEvent();
            behaviour.Event.AddListener(gameController.NextLevel);
        }

        public void Show(int scores)
        {
            screen.Show();
            this.scores.text = scores.ToString();
        }
    }
}