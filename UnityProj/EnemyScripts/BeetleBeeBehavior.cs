using UnityEngine;
using System.Collections;

public class BeetleBeeBehvior : MonoBehaviour
{
    [SerializeField] private ParticleSystem explosionEffect;
    public Transform firingPos;
    public GameObject projectilePrefab;  // Reference to the projectile prefab
    public float sideMovementSpeed = 0.4f; // Speed of side-to-side movement
    public float sideMovementAmount = 0.2f; // How far the ship moves left and right
    public float timeBetweenShots = 2f;  // Base time between shots (used for intervals)
    public float health = 10f;
    public float damage = 0.5f;

    public float Exp = 43f;
    public float gold = 30f;

    private Vector3 startPosition;
    private void Start()
    {
        startPosition = transform.position;
        StartCoroutine(fireBees());
    }

    private void Update()
    {
        SideToSideMovement();
    }

    void SideToSideMovement()
    {
        // Side-to-side movement using a sine wave for smooth oscillation
        float sideMovement = Mathf.Sin(Time.time * sideMovementSpeed) * sideMovementAmount;
        transform.position = new Vector3(startPosition.x + sideMovement, transform.position.y, transform.position.z);
    }

    IEnumerator fireBees()
    {
        while (true)
        {
            // Shoot 1 projectile
            ShootProjectiles();
            yield return new WaitForSeconds(timeBetweenShots);
        }
    }

    void ShootProjectiles()
    {
        Instantiate(projectilePrefab, firingPos.position, Quaternion.identity);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            TakeDamage(1);  // Charger takes damage on collision
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
}
