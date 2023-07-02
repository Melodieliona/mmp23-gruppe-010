using UnityEngine;
using UnityEngine.Events;

public class PlayerManager : MonoBehaviour
{
    [Header("Attributes")]
    private int score = 0;
    private int healthPoints = 5;
    private int goldCount = 500;

    [Header("Events")]
    private readonly UnityEvent scoreChangeEvent = new();
    private readonly UnityEvent healthChangeEvent = new();
    private readonly UnityEvent goldChangeEvent = new();

    [Header("Screens")]
    public GameOverScreen gameOverScreen;

    public void Start()
    {
        scoreChangeEvent.AddListener(FindObjectOfType<GUIManager>().UpdateScore);
        healthChangeEvent.AddListener(FindObjectOfType<GUIManager>().UpdateHp);
        goldChangeEvent.AddListener(FindObjectOfType<GUIManager>().UpdateGold);
    }

    public int GetHealthPoints()
    {
        return healthPoints;
    }

    public int GetScore()
    {
        return score;
    }

    public int GetGold()
    {
        return goldCount;
    }

    public void AddHealthPoints(int number)
    {
        healthPoints += number;
        healthChangeEvent.Invoke();
    }

    public void RemoveHealthPoints(int number)
    {
        if (healthPoints - number > 0)
        {
            healthPoints -= number;
            healthChangeEvent.Invoke();
        }
        else
        {
            //Game Over!
            gameOverScreen.Setup(GetScore());
        }
    }

    public void AddGold(int number)
    {
        goldCount += number;
        goldChangeEvent.Invoke();
    }

    public bool RemoveGold(int number)
    {
        if (goldCount - number >= 0)
        {
            goldCount -= number;
            goldChangeEvent.Invoke();
            return true;
        }
        else
        {
            //Show the user that he can't afford it
            return false;
        }
    }

    public void AddScore(int number)
    {
        score += number;
        scoreChangeEvent.Invoke();
    }
    
    public void RemoveHP(int number)
    {
        if (healthPoints - number > 0)
        {
            healthPoints = healthPoints - number;
        }
        else
        {
            //Game Over!
        }
    }
}