using Enemy;
using UnityEngine;
using UnityEngine.UIElements;

namespace Player
{
    public class GUIManager : MonoBehaviour
    {
        [Header("References")]
        private PlayerController playerController;
        private ShipSpawner shipSpawner;
        private VisualElement rootVisualElement;

        private Label hpLabel;
        private Label goldLabel;
        private Label waveLabel;
        private Label timerLabel;
        private ProgressBar timerProgress;

        private void Start()
        {
            playerController = FindObjectOfType<PlayerController>();
            shipSpawner = FindObjectOfType<ShipSpawner>();

            UpdateHp();
            UpdateScore();
            UpdateGold();
            UpdateWave();
        }

        private void OnEnable()
        {
            rootVisualElement = GetComponent<UIDocument>().rootVisualElement;

            hpLabel = rootVisualElement.Q<Label>("HP_Label");
            goldLabel = rootVisualElement.Q<Label>("Gold_Label");
            waveLabel = rootVisualElement.Q<Label>("Wave_Label");
            timerLabel = rootVisualElement.Q<Label>("TimerLabel");
            timerProgress = rootVisualElement.Q<ProgressBar>("TimerProgress");
        }

        public void UpdateHp()
        {
            hpLabel.text = "HP: " + playerController.GetHealthPoints();
        }

        public void UpdateScore()
        {
            //scoreLabel.text = "Score: " + playerManager.GetScore();
        }

        public void UpdateGold()
        {
            goldLabel.text = "Gold: " + playerController.GetGold();
        }
    
        public void UpdateWave()
        {
            waveLabel.text = "Wave: " + shipSpawner.GetCurrentWave();
        }

        public void UpdateTimer(int currentTime, float percentageOfMax)
        {
            timerLabel.text = "Next wave in " + currentTime;
            timerProgress.value = percentageOfMax;
        }

        public void ShowTimer()
        {
            timerLabel.visible = true;
            timerProgress.visible = true;
        }

        public void HideTimer()
        {
            timerLabel.visible = false;
            timerProgress.visible = false;
        }
    }
}