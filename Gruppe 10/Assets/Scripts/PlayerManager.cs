using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{

    private int goldCount = 10;
    private int score = 0;
    private int currentWave = 0;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void addGold(int number)
    {
        goldCount = goldCount + number;
    }

    public void removeGold(int number)
    {
        if(goldCount - number >= 0)
        {
            goldCount = goldCount - number;
        }
    }
}
