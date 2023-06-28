using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField] private int score = 0;
    [SerializeField] private int healthPoints = 10;
    [SerializeField] private int goldCount = 50;

    public int GetHealthPoints()
    {
        return healthPoints;
    }

    public void AddHealthPoints(int number)
    {
        healthPoints += number;
    }

    public void RemoveHealthPoints(int number)
    {
        if (healthPoints - number > 0)
        {
            healthPoints -= number;
        }
        else
        {
            //Game Over!
        }
    }

    public void AddGold(int number)
    {
        goldCount += number;
    }

    public void RemoveGold(int number)
    {
        if (goldCount - number >= 0)
        {
            goldCount -= number;
        }
        else
        {
            //Show the user that he can't afford it
        }
    }

    public void AddHP(int number)
    {
        healthPoints = healthPoints + number;
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