using UnityEngine;
using System.Collections;

public class TeleporterBehavior : MonoBehaviour
{
    [SerializeField] private ParticleSystem portalEffect;
    [SerializeField] private ParticleSystem explosionEffect;

    public float Exp = 85f;
    public float gold = 60f;
    public float health = 20;

    public Transform firingPos;
    public GameObject projectilePrefab;  // The projectile prefab to shoot
    public float hoverSpeed = 0.5f;  // Speed of hovering
    public Vector2 teleportBoundsMin = new Vector2(0, 0);  // Min X, Y bounds (0, 0)
    public Vector2 teleportBoundsMax = new Vector2(9, 6);  // Max X, Y bounds (9, 9)
    public float teleportDelay = 3f;  // Time between teleporting

    private int teleportCount = 0;
    private bool isTeleporting = false;

    private void Start()
    {
        // Start the teleportation behavior
        StartCoroutine(TeleportCoroutine());
    }


    // Teleporting and shooting behavior
    private IEnumerator TeleportCoroutine()
    {
        while (true)
        {
            if (!isTeleporting)
            {

                // Start the teleporting process
                isTeleporting = true;
                teleportCount = 0;

                // Repeat teleportation and shooting 3 times
                while (teleportCount < 3)
                {
                    spawnPortal();
                    yield return new WaitForSeconds(0.1f);
                    Vector3 vec = CalcPosition();
                    spawnPortal(vec);
                    yield return new WaitForSeconds(0.1f);
                    TeleportToRandomPosition(vec);
                    yield return new WaitForSeconds(0.5f);
                    ShootProjectile();
                    teleportCount++;
                    yield return new WaitForSeconds(1f);  // Wait 1 second between teleporting and shooting
                }

                // After 3 teleports, wait for 3 seconds rest
                yield return new WaitForSeconds(3f);
                isTeleporting = false;
            }

            // Keep the loop going for the next teleport
            yield return null;
        }
    }

    // Function to teleport the teleporter to a random position within bounds
    private void TeleportToRandomPosition(Vector3 vec)
    {
        
        transform.position = vec;
    }

    private Vector3 CalcPosition()
    {
        float randomX = Random.Range(teleportBoundsMin.x, teleportBoundsMax.x);
        float randomY = Random.Range(teleportBoundsMin.y, teleportBoundsMax.y);

        Vector3 vec = new Vector3(randomX, randomY, transform.position.z); 
        return vec;
    }

    // Function to shoot a projectile from the current position
    private void ShootProjectile()
    {
        if (projectilePrefab != null)
        {
            Instantiate(projectilePrefab, firingPos.position, Quaternion.identity);  // Instantiate projectile at teleporter's position
        }
    }

    private void spawnPortal()
    {
        ParticleSystem ps = Instantiate(portalEffect, transform.position, Quaternion.identity);
        Destroy(ps.gameObject, 3f);
        ps.Play();
    }
    private void spawnPortal(Vector3 pos)
    {
        ParticleSystem ps = Instantiate(portalEffect,pos, Quaternion.identity);
        Destroy(ps.gameObject, 3f);
        ps.Play();
    }




    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        { 
            TakeDamage(1); 
        }
        else if (collider.gameObject.CompareTag("PlayerProjectile"))
        {
            // Take damage from projectiles
            TakeDamage(collider.gameObject.GetComponent<Projectile>().dmg);
            Debug.Log("hit" + collider.gameObject.GetComponent<Projectile>().dmg);
        }
    }

    // Method to handle damage taken by the charger
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
            GameManager.Instance.AddExperience(Exp * GameManager.Instance.playerTier);
            GameManager.Instance.AddGold((int)(gold*GameManager.Instance.playerTier));
            SpawnManager.Instance.EnemyDestroyed(this.gameObject);
            Destroy(gameObject);  // Destroy the charger when health reaches 0
        }
    }
}
