using UnityEngine;
using UnityEngine.Events;

public class PlayerManager : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField] private int score = 0;
    [SerializeField] private int healthPoints = 10;
    [SerializeField] private int goldCount = 50;

    [Header("Events")]
    private readonly UnityEvent scoreChangeEvent = new();
    private readonly UnityEvent healthChangeEvent = new();
    private readonly UnityEvent goldChangeEvent = new();

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
        }
    }

    public void AddGold(int number)
    {
        goldCount += number;
        goldChangeEvent.Invoke();
    }

    public void RemoveGold(int number)
    {
        if (goldCount - number >= 0)
        {
            goldCount -= number;
            goldChangeEvent.Invoke();
        }
        else
        {
            //Show the user that he can't afford it
        }
    }

    public void AddScore(int number)
    {
        score += number;
        scoreChangeEvent.Invoke();
    }
}