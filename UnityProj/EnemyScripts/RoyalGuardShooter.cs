using UnityEngine;
using System.Collections;

public class RoyalGuardShooter : MonoBehaviour
{
    [SerializeField] private ParticleSystem explosionEffect;
    public GameObject projectilePrefab;
    public float dashSpeed = 10f;
    public float dashCooldown = 0.5f;
    public float shootCooldown = 0.2f;
    public int dashCount = 5;

    public float health = 50;
    public float Exp = 106f;
    public float gold = 75f;

    private bool isResting = false;
    private Vector3 targetPosition;

    void Start()
    {
        StartCoroutine(DashAndShootRoutine());
    }

    IEnumerator DashAndShootRoutine()
    {
        while (true)
        {
            if (!isResting)
            {
                for (int i = 0; i < dashCount; i++)
                {
                    float targetX = Random.Range(-8f, 8f);
                    targetPosition = new Vector3(targetX, transform.position.y, 0);

                    // Dash
                    yield return StartCoroutine(DashToPosition(targetPosition));

                    // Shoot
                    GameObject projectile =  Instantiate(projectilePrefab, transform.position, Quaternion.identity);
                    RoyalProjectileOrange projectileScript = projectile.GetComponent<RoyalProjectileOrange>();
                    if (projectileScript != null)
                    {
                        projectileScript.SetDirection(-transform.up); // Pass the direction to the projectile
                    }
                    yield return new WaitForSeconds(shootCooldown);
                }

                // Rest after 5 dashes
                isResting = true;
                yield return new WaitForSeconds(2f);
                isResting = false;
            }

            yield return null;
        }
    }

    IEnumerator DashToPosition(Vector3 target)
    {
        while (Vector3.Distance(transform.position, target) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, dashSpeed * Time.deltaTime);
            yield return null;
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            // Handle damage to both the charger and the player
            // Assuming the player has a health script, you can apply damage here
            //collision.gameObject.GetComponent<PlayerHealth>().TakeDamage(damage);  
            TakeDamage(1);  // Charger takes damage on collision
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
            GameManager.Instance.AddExperience(Exp * GameManager.Instance.currentTierMultiplyer);
            GameManager.Instance.AddGold((int)(gold * GameManager.Instance.currentTierMultiplyer));
            SpawnManager.Instance.EnemyDestroyed(this.gameObject);
            Destroy(gameObject);  // Destroy the charger when health reaches 0
        }
    }
}
