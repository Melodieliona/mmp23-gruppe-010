using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{

    public int goldCount = 50;
    public int score = 0;
    public int currentWave = 0;
    public int healthPoints = 10;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
