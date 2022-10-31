using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Enemy's Status")]
    [SerializeField] float health = 100;
    [SerializeField] int scoreValue = 150;

    [Header("Enemy's Bullet Config")]
    float shotCounter;
    [SerializeField] float minTimeBetweenShots = 0.2f;
    [SerializeField] float maxTimeBetweenShots = 3f;
    [SerializeField] GameObject enemyProjectile;
    [SerializeField] float enemyProjectileSpeed = 10f;

    [Header("Enemy's VFX")]
    [SerializeField] GameObject deathParticle;
    [SerializeField] float secondsToDie = 1f;

    [Header("Enemy's SFX")]
    [SerializeField] AudioClip enemyShootsSound;
    [SerializeField] AudioClip enemyDeathSound;
    [SerializeField] [Range(0,1)] float SoundVolume = 0.7f;

    [Header("Drop Power Ups")]
    [SerializeField] GameObject recoverHealth;
    [SerializeField] GameObject recoverFullHealth;

    PowerUps powerUps;

    // Start is called before the first frame update
    void Start()
    {
        powerUps = FindObjectOfType<PowerUps>();
        shotCounter = Random.Range(minTimeBetweenShots, maxTimeBetweenShots);
    }

    // Update is called once per frame
    void Update()
    {
        CountDownAndShoot();
    }

    private void CountDownAndShoot()
    {
        shotCounter -= Time.deltaTime;
        if (shotCounter <= 0f)
        {
            Fire();
            shotCounter = Random.Range(minTimeBetweenShots, maxTimeBetweenShots);
        } 
    }

    private void Fire()
    {
        Vector2 enemyProjectileOffset = new Vector2(transform.position.x, transform.position.y -0.5f);

        GameObject enemyFire = Instantiate
            (
                enemyProjectile, 
                enemyProjectileOffset, 
                Quaternion.identity
            ) as GameObject;
        enemyFire.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -enemyProjectileSpeed);
        AudioSource.PlayClipAtPoint(enemyShootsSound, Camera.main.transform.position, SoundVolume);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        DamageDealer damageDealer = other.gameObject.GetComponent<DamageDealer>();
        if (!damageDealer) { return; } // This is a Guard Clause
        ProcessHit(damageDealer);
    }

    private void ProcessHit(DamageDealer damageDealer)
    {
        health -= damageDealer.GetDamage();
        damageDealer.Hit();
        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        FindObjectOfType<GameSession>().AddToScore(scoreValue);
        Destroy(gameObject);
        GameObject explosion = Instantiate(deathParticle, transform.position, Quaternion.identity);
        Destroy(explosion, secondsToDie);
        AudioSource.PlayClipAtPoint(enemyDeathSound, Camera.main.transform.position, SoundVolume);
        if(powerUps.ChanceToDropPowerUp() > 75d)
        {
            Instantiate(recoverHealth, transform.position, Quaternion.identity);
        }
    }
}

// Edit/Project Settings/ Physics 2D... On the GameObject you can change the Layer of collision