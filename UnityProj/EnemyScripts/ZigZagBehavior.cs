using UnityEngine;
using System.Collections;
using static UnityEngine.RuleTile.TilingRuleOutput;
using Unity.VisualScripting;

public class ZigZagBehavior : MonoBehaviour
{
    [SerializeField] private ParticleSystem explosionEffect;
    public float moveSpeed = 15f;  // Speed of movement
    public float moveDistance = 5f;  // Distance to move before changing direction
    private float distanceTraveled = 0f;  // Track how far the object has moved
    public float health = 8;              // Health of the charger
    public float damage = 2;
    public float Exp = 64f;
    public float gold = 40f;
    private bool isRotating = false;  // Flag to check if the object is rotating
    private float targetRotation;  // The target rotation (either 75 or -75)
    private float rotationSpeed = 270f; // Speed of rotation
    private Vector3 startPos;
    private bool outOfBoundries = false;

    void Start()
    {
        startPos = transform.position;
        // Randomly choose between 75 and -75 degrees for initial rotation
        targetRotation = Random.Range(0f, 1f) > 0.5f ? 75f : -75f;
        isRotating = true;
        
    }

    void Update()
    {
        if(transform.position.y <= -Camera.main.orthographicSize * 2)
        {
            outOfBoundries = true;
            returnTop();
            
        }
        // If the object is rotating, do not move
        if (!outOfBoundries)
        {
            if (isRotating)
            {
                // Smoothly rotate towards the target rotation
                transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, 0, targetRotation), rotationSpeed * Time.deltaTime);
                // Check if the current rotation is either 75 or -75
                if (transform.rotation.eulerAngles.z == targetRotation || transform.rotation.eulerAngles.z == 360 + targetRotation)
                {
                    isRotating = false;  // Stop rotating, allow movement
                }
            }
            else
            {
                // Move the object in the direction it is currently facing
                transform.Translate(Vector3.down * moveSpeed * Time.deltaTime);

                // Track the distance traveled
                distanceTraveled += moveSpeed * Time.deltaTime;

                // When the object reaches the moveDistance, change direction
                if (distanceTraveled >= moveDistance)
                {
                    // Set the target rotation to the opposite direction (exactly 75 or -75)
                    targetRotation = (targetRotation == 75f) ? -75f : 75f;

                    // Start the rotation
                    isRotating = true;

                    // Reset the distance traveled for the next segment of movement
                    distanceTraveled = 0f;
                }
            }
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


    public void returnTop()
    {
        isRotating = false;
        transform.rotation = Quaternion.Euler(0, 0, 0);
        transform.position = new Vector3(transform.position.x, Camera.main.orthographicSize + 5f, transform.position.z);
        StartCoroutine(moveToStartPos());
    }


    IEnumerator moveToStartPos()
    {
        
        while(Vector3.Distance(transform.position, startPos) >= 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, startPos, moveSpeed * 0.2f * Time.deltaTime);
            yield return null;

        }
        transform.position = startPos;  // Ensure it is exactly at the start position
        
        yield return new WaitForSeconds(1.5f);
        isRotating = true;  // Restart rotation sequence
        outOfBoundries = false;
        distanceTraveled = 0;

    }
}
