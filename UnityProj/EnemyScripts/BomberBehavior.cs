using UnityEngine;
using System.Collections;

public class BomberBehavior : MonoBehaviour
{
    [SerializeField] private ParticleSystem explosionEffect;
    public GameObject explosionPrefab;
    public Transform firingPos;
    public float health = 10f;
    public float maxHealth = 10f;
    public float moveSpeed = 11f;
    public float attackCooldown = 4f;
    public float damage = 1f;
    public Transform player;  // Reference to the player's transform
    public GameObject aoeProjectilePrefab;  // Reference to the AoE projectile prefab
    private Vector3 startingPosition;  // Bomber's starting position
    private bool isBelow30PercentHealth = false;
    private bool isMovingTowardsPlayer = false;
    private bool isReturningToStart = false;
    private float lastAttackTime;

    public float Exp = 77f;
    public float gold = 50f;

    private void Start()
    {
        startingPosition = transform.position;
        lastAttackTime = Time.time;
    }

    private void Update()
    {
        // Handle health change (below 30% logic)
        if (health <= maxHealth * 0.3f && !isBelow30PercentHealth)
        {
            isBelow30PercentHealth = true;
            StopShooting();
        }

        if (isBelow30PercentHealth)
        {
            HandleMovementTowardsPlayer();
        }
        else
        {
            HandleShooting();
        }
    }

    private void HandleShooting()
    {
        if (Time.time - lastAttackTime >= attackCooldown)
        {
            ShootAOEProjectile();
            lastAttackTime = Time.time;
        }
    }

    private void ShootAOEProjectile()
    {
        // Create and fire an AoE projectile (assuming a prefab is assigned)
        Instantiate(aoeProjectilePrefab, firingPos.position, Quaternion.identity);
    }

    private void HandleMovementTowardsPlayer()
    {
        // If the bomber is not currently moving towards the player, start moving
        if (!isMovingTowardsPlayer && !isReturningToStart)
        {
            StartCoroutine(MoveTowardsPlayer());
        }
    }

    private IEnumerator MoveTowardsPlayer()
    {
        isMovingTowardsPlayer = true;

        // Move towards the player's position
        while (Vector2.Distance(transform.position, player.position) > 0.2f)
        {
            // Move towards the player
            Vector2 direction = (player.position - transform.position).normalized;
            transform.position = Vector2.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);
            yield return null;
        }

        // If the bomber misses (after moving towards player), return to start position
        if (Vector2.Distance(transform.position, player.position) > 0.2f)
        {
            StartCoroutine(ReturnToStart());
        }
        else
        {
            isMovingTowardsPlayer = false; // Reached player
        }
    }

    private IEnumerator ReturnToStart()
    {
        isMovingTowardsPlayer = false;
        isReturningToStart = true;

        // Move back to starting position
        while (Vector2.Distance(transform.position, startingPosition) > 0.2f)
        {
            transform.position = Vector2.MoveTowards(transform.position, startingPosition, moveSpeed * Time.deltaTime);
            yield return null;
        }

        // Wait for 2 seconds before trying again
        yield return new WaitForSeconds(2f);
        isReturningToStart = false;
    }

    private void StopShooting()
    {
        // Stop shooting projectiles once below 30% health
        CancelInvoke("ShootAOEProjectile");
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            boom();
        }
        else if (collider.gameObject.CompareTag("PlayerProjectile"))
        {
            // Take damage from projectiles
            TakeDamage(collider.gameObject.GetComponent<Projectile>().dmg);
            Debug.Log("hit" + collider.gameObject.GetComponent<Projectile>().dmg);
        }
    }

    public void TakeDamage(float damageAmount)
    {
        health -= damageAmount;
        if (health <= 0)
        {
            if (explosionEffect != null)
            {
                ParticleSystem explosion = Instantiate(explosionEffect, transform.position, Quaternion.identity);
                explosion.Play();
                Destroy(explosion.gameObject, 3f);
            }
            GameManager.Instance.AddExperience(Exp * GameManager.Instance.currentTierMultiplyer);
            GameManager.Instance.AddGold((int)(gold * GameManager.Instance.currentTierMultiplyer));
            SpawnManager.Instance.EnemyDestroyed(this.gameObject);
            Destroy(gameObject);  // Destroy the charger when health reaches 0
        }
    }


    private void boom()
    {
        if (explosionPrefab != null)
        {
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            //ParticleSystem explosion = Instantiate(explosionEffect, transform.position, Quaternion.identity);
            //explosion.Play();
        }

        Destroy(gameObject);
    }
}
