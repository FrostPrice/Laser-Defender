using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Configuration Parameters
    [Header("Player Status")] // You can use Header to better organize the inspector menu
    [SerializeField] float moveSpeed = 10f;
    [SerializeField] float padding = 1f;
    [SerializeField] float health = 200;

    [Header("Player's Projectlie")]
    [SerializeField] GameObject laserPrefab;
    [SerializeField] float projectileSpeed = 10f;
    [SerializeField] float projectileFiringPeriod = 0.1f;

    [Header("Player's VFX")]
    [SerializeField] AudioClip playerShootSound;
    [SerializeField] AudioClip playerDeathSound;
    [SerializeField] [Range(0,1)] float soundVolumeShoot = 0.5f;
    [SerializeField] [Range(0,1)] float soundVolumeDeath = 1f;

    Coroutine firingCoroutine;

    PowerUps powerUps;

    // State Variables
    float xMin;
    float xMax;
    float yMin;
    float yMax;

    // Start is called before the first frame update
    void Start()
    {
        SetUpMoveBoundaries();
    }

    // Update is called once per frame
    void Update()   
    {
        Move();
        Fire();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        DamageDealer damageDealer = collision.GetComponent<DamageDealer>();
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
        FindObjectOfType<Level>().LoadGameOver();
        Destroy(gameObject);
        AudioSource.PlayClipAtPoint(playerDeathSound, Camera.main.transform.position, soundVolumeDeath);
    }

    private void Fire()
    {
        if (Input.GetButtonDown("Fire1")) // To use the GetButtonDown() Method, go to Edit/Project Settings/Input Manager, and there you'll have access to all input and its configurations 
        {
            if (firingCoroutine != null)
            {
                StopCoroutine(firingCoroutine);
                firingCoroutine = StartCoroutine(FireContinuously());
            }
            else if (firingCoroutine == null)
            {
                firingCoroutine = StartCoroutine(FireContinuously());
            }
        }
        if (Input.GetButtonUp("Fire1"))
        {
            StopCoroutine(firingCoroutine);
        }
    }

    IEnumerator FireContinuously()
    {
        // The While loop keeps going if a condition is meet
        while(true) // While the FireContinuously() keeps doing the code bellow
        {
            Vector2 laserOffsetPosition = new Vector2(transform.position.x, transform.position.y + 0.6f);

            GameObject laser = Instantiate(
                laserPrefab,
                laserOffsetPosition,
                Quaternion.identity) as GameObject; // Quaternion.identity will leave the rotarion as it is, basically means no rotation
            laser.GetComponent<Rigidbody2D>().velocity = new Vector2(0, projectileSpeed);
            AudioSource.PlayClipAtPoint(playerShootSound, Camera.main.transform.position, soundVolumeShoot);
            yield return new WaitForSeconds(projectileFiringPeriod);
        }
    }

    private void Move()
    {
        var deltaX = Input.GetAxis("Horizontal") * Time.deltaTime * moveSpeed; // To use the GetAxis() Method, go to Edit/Project Settings/Input Manager, and there you'll have access to all input and its configurations 
        var deltaY = Input.GetAxis("Vertical") * Time.deltaTime * moveSpeed;

        var newXPos = Mathf.Clamp(transform.position.x + deltaX, xMin, xMax);
        var newYPos = Mathf.Clamp(transform.position.y + deltaY, yMin, yMax);
        transform.position = new Vector2(newXPos, newYPos);
    }

    private void SetUpMoveBoundaries()
    {
        Camera gameCamera = Camera.main;
        xMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).x + padding;
        xMax = gameCamera.ViewportToWorldPoint(new Vector3(1, 0, 0)).x - padding;
        yMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).y + padding;
        yMax = gameCamera.ViewportToWorldPoint(new Vector3(0, 1, 0)).y - padding;
    }

    public float GetPlayerHealth()
    {
        return health;
    }

    public void SetHealth(float newHealth)
    {
        if(newHealth == 1000)
        {
            health = newHealth;
        } 
        else
        {
            health += newHealth;
        }
    }
}
