using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageDealer : MonoBehaviour
{
    [SerializeField] int damage = 100;

    public int GetDamage()
    {
        return damage;
    }

    public void Hit()
    {
        Destroy(gameObject);
    }

    // Destroy the bullet when it collides with something
    private void OnTriggerEnter2D()
    {
        if(tag == "bullet")
        {
            Destroy(gameObject);
        }
    }
}
