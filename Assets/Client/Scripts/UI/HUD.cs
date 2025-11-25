using System;
using Doozy.Runtime.UIManager;
using Doozy.Runtime.UIManager.Components;
using Doozy.Runtime.UIManager.Containers;
using TMPro;
using UnityEngine;

namespace miniIT.Arcanoid
{
    public class HUD : MonoBehaviour
    {
        [SerializeField]
        private UIView screen = default;
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
        [SerializeField]
        private UIButton resume = default;


        public void Bind(GameController controller)
        {
            controller.paused += OnPause;
            controller.resumed += OnResume;
            AddPauseClickListener(controller.Pause);
            AddResumeClickListener(controller.Resume);
        }

        public void Unbind()
        {
            RemoveListeners();
        }

        public void SetPlayer(Player player)
        {
            player.ScoresChanged += ShowScores;
            ShowScores(player.Scores);
            player.LifesChanged += ShowLifes;
            ShowLifes(player.Lifes);
        }

        public void SetLevelController(LevelController levelController)
        {
            levelController.BallsSpeedChanged += ShowSpeed;
            ShowSpeed(levelController.BallsSpeed);
        }

        public void ShowScores(int scores)
        {
            this.scores.text = scores.ToString();
        }

        public void Show()
        {
            screen.Show();
        }

        public void Hide()
        {
            screen.Hide();
        }

        public void OnPause()
        {
            pause.gameObject.SetActive(false);
            resume.gameObject.SetActive(true);
        }

        public void OnResume()
        {
            pause.gameObject.SetActive(true);
            resume.gameObject.SetActive(false);
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

        public void AddResumeClickListener(Action action)
        {
            UIBehaviour b = resume.behaviours.AddBehaviour(UIBehaviour.Name.PointerClick);
            b.Event = new UnityEngine.Events.UnityEvent();
            b.Event.AddListener(action.Invoke);
        }

        private void RemoveListeners()
        {
            pause.behaviours.RemoveBehaviour(UIBehaviour.Name.PointerClick);
            resume.behaviours.RemoveBehaviour(UIBehaviour.Name.PointerClick);
        }

    }
}