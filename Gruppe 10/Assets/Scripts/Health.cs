using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [Header("References")]
    private PlayerManager PlayerManager;

    [Header("Attributes")]
    [SerializeField] private int hitPoints = 2;

    private void Start()
    {
        PlayerManager = FindObjectOfType<PlayerManager>();
    }

    public void TakeDamage(int dmg) {
        hitPoints -= dmg;

        if(hitPoints <= 0) {
            ShipSpawner.onEnemyDestroy.Invoke();
            Destroy(gameObject);
            PlayerManager.AddScore(hitPoints);
            PlayerManager.AddGold(50);
        }
    }
}
