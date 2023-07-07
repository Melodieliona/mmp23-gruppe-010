using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        [Header("Attributes")]
        [SerializeField] private int healthPoints = 5;
        [SerializeField] private int goldCount = 500;

        [Header("Events")]
        private readonly UnityEvent scoreChangeEvent = new();
        private readonly UnityEvent healthChangeEvent = new();
        private readonly UnityEvent goldChangeEvent = new();

        private void Start()
        {
            scoreChangeEvent.AddListener(FindObjectOfType<GUIManager>().UpdateScore);
            healthChangeEvent.AddListener(FindObjectOfType<GUIManager>().UpdateHp);
            goldChangeEvent.AddListener(FindObjectOfType<GUIManager>().UpdateGold);
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
                // Game over
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

        public int GetScore()
        {
            return ScoreController.GetScore();
        }

        public void AddScore(int score)
        {
            ScoreController.AddScore(score);
            scoreChangeEvent.Invoke();
        }
    }
}