using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverScreen : MonoBehaviour
{
    public Text scoreNumber;

    public void Setup(int score)
    {
        Debug.Log("2 Step works");
        gameObject.SetActive(true);
        scoreNumber.text = "SCORE: " + score;
    }

    public void RetryButton()
    {
        SceneManager.LoadScene("Game");
        Debug.Log("Retry");
    }

    public void QuitButton()
    {
        Application.Quit();
        Debug.Log("Main");
    }
}