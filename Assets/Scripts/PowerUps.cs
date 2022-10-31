using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUps : MonoBehaviour
{
    [Header("Chance to Get power Up")]
    [SerializeField] float minChance = 50;
    [SerializeField] float maxChance = 100;

    [SerializeField] int recoverHealth = 400;
    [SerializeField] int recoverFullHealth = 1000;

    Player player;
    Enemy enemy;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<Player>();
        enemy = FindObjectOfType<Enemy>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        RecoverHealth();
    }

    public float ChanceToDropPowerUp()
    {
        return Random.Range(minChance, maxChance + 1);
    }

    public void RecoverHealth()
    {
        var currentPlayerHealth = player.GetPlayerHealth();
        currentPlayerHealth = recoverFullHealth;
    }
}