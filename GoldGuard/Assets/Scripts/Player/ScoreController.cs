using UnityEngine;

namespace Player
{
    public class ScoreController : MonoBehaviour
    {
        private static int score = 0;

        public static int GetScore()
        {
            return score;
        }

        public static void AddScore(int amount)
        {
            score += amount;
        }
    }
}