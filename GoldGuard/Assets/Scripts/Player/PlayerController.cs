using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        private int healthPoints = 10;
        private int goldCount = 150;

        private readonly UnityEvent healthChangeEvent = new();
        private readonly UnityEvent goldChangeEvent = new();

        private void Start()
        {
            GUIManager guiManager = FindObjectOfType<GUIManager>();
            healthChangeEvent.AddListener(guiManager.UpdateHp);
            goldChangeEvent.AddListener(guiManager.UpdateGold);
        }

        public int GetHealthPoints()
        {
            return healthPoints;
        }

        public void RemoveHealthPoints(int hp)
        {
            if (healthPoints - hp > 0)
            {
                healthPoints -= hp;
                healthChangeEvent.Invoke();
            }
            else
            {
                SceneManager.LoadScene("GameOver");
            }
        }

        public int GetGold()
        {
            return goldCount;
        }

        public void AddGold(int gold)
        {
            goldCount += gold;
            goldChangeEvent.Invoke();
        }

        public bool RemoveGold(int gold)
        {
            if (goldCount - gold >= 0)
            {
                goldCount -= gold;
                goldChangeEvent.Invoke();
                return true;
            }
            else
            {
                //Show the user that he can't afford it
                return false;
            }
        }

        public void AddScore(int score)
        {
            ScoreController.AddScore(score);
        }
    }
}