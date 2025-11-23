using Doozy.Runtime.UIManager;
using Doozy.Runtime.UIManager.Components;
using Doozy.Runtime.UIManager.Containers;
using TMPro;
using UnityEngine;
using VContainer;

namespace miniIT.Arcanoid
{
    public class LoseScreen : MonoBehaviour
    {
        [SerializeField]
        private UIView screen = default;
        [SerializeField]
        private TMP_Text scores = default;
        [SerializeField]
        private UIButton menu = default;

        private GameController gameController = default;

        public void Bind(GameController controller)
        {
            gameController = controller;
            UIBehaviour behaviour = menu.behaviours.AddBehaviour(UIBehaviour.Name.PointerClick);
            behaviour.Event = new UnityEngine.Events.UnityEvent();
            behaviour.Event.AddListener(ToMenu);
        }

        public void Show(int scores)
        {
            screen.Show();
            this.scores.text = scores.ToString();
        }


        private void ToMenu()
        {
            screen.InstantHide();
            gameController.ToMenu();
        }
    }
}