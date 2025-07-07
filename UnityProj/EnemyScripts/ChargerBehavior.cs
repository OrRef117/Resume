using UnityEngine;

public class ChargerEnemy : MonoBehaviour
{
    [SerializeField] private ParticleSystem explosionEffect;
    public float rotationSpeed = 100f;    // Speed of rotation
    public float chargeSpeed = 5f;        // Speed of the charge downward
    public float health = 4;              // Health of the charger
    public float damage = 1;              // Damage dealt to player on hit
    public Vector2 startPosition;         // Starting position of the charger
    public float aboveCameraHeight = 10f; // Height above the camera to move before coming back

    public float Exp= 21f;
    public float gold = 15f;

    private bool isCharging = false;      // Is charging downward
    private bool isRotating = true;       // Is rotating
    private bool isShaking = false;       // Is shaking the enemy
    private bool isReturningToStart = false; // Is returning to the start position

    private float rotationTimer = 0f;     // Timer to track rotation time
    private float shakeTimer = 0f;        // Timer to track shake time

    private float rotationDuration = 1.5f; // Duration for each rotation direction (left/right)
    private float shakeDuration = 1.5f;   // Shake duration
    private float shakeMagnitude = 0.01f; // Magnitude of the shake (scaled down by 10x)

    void Start()
    {
        // Initialize the charger to start from the top position
        startPosition = transform.position;
        
    }

    void Update()
    {
        if (health <= 0)
        {
            Destroy(gameObject);  // Destroy the charger if health reaches 0
            return;
        }

        // Perform rotation, shake, or charge based on the current state
        if (isRotating)
        {
            RotateEnemy();
        }
        else if (isShaking)
        {
            ShakeEffect();
        }
        else if (isCharging)
        {
            ChargeDownwards();
        }
        else if (isReturningToStart)
        {
            ReturnToStartPosition();
        }
    }

    void RotateEnemy()
    {
        rotationTimer += Time.deltaTime;

        if (rotationTimer < rotationDuration)
        {
            // Rotate to the left
            transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);
        }
        else if (rotationTimer >= rotationDuration && rotationTimer < rotationDuration * 2)
        {
            // Rotate to the right
            transform.Rotate(Vector3.back * rotationSpeed * Time.deltaTime);
        }
        else if (rotationTimer >= rotationDuration * 2)
        {
            // After rotating left and right, start shaking
            isRotating = false;
            isShaking = true;
            rotationTimer = 0f; // Reset the rotation timer
        }
    }

    void ShakeEffect()
    {
        shakeTimer += Time.deltaTime;

        if (shakeTimer < shakeDuration)
        {
            // Subtle shake effect
            transform.position += new Vector3(Mathf.Sin(Time.time * 10f) * shakeMagnitude, Mathf.Cos(Time.time * 10f) * shakeMagnitude, 0f);
        }
        else
        {
            // Stop shaking and start charging
            isShaking = false;
            isCharging = true;
            shakeTimer = 0f; // Reset the shake timer
        }
    }

    void ChargeDownwards()
    {
        // Move the charger downward until it exits the camera view
        transform.position += Vector3.down * chargeSpeed * Time.deltaTime;

        // If charger goes below camera view, reset it to above the camera range
        if (transform.position.y <= -Camera.main.orthographicSize * 2)  // Below the camera's bottom edge
        {
            MoveAboveCameraAndReturn();
        }
    }

    void MoveAboveCameraAndReturn()
    {
        // Move the charger above the camera's range (higher than original position)
        transform.position = new Vector3(transform.position.x, Camera.main.orthographicSize + aboveCameraHeight, transform.position.z);

        // Start returning to the start position using half the charge speed
        isCharging = false;
        isReturningToStart = true;
    }

    void ReturnToStartPosition()
    {
        // Move the charger back to its starting position using half the charge speed
        transform.position = Vector3.MoveTowards(transform.position, startPosition, chargeSpeed * 0.5f * Time.deltaTime);

        // When it reaches the starting position, restart the charging sequence
        if (Vector3.Distance(transform.position, startPosition) <= 0.1f)
        {
            transform.position = startPosition;  // Ensure it is exactly at the start position
            isReturningToStart = false; // Stop returning
            isRotating = true;  // Restart rotation sequence
        }
    }

    // Collision detection for player damage
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            // Handle damage to both the charger and the player
            // Assuming the player has a health script, you can apply damage here
            //collision.gameObject.GetComponent<PlayerHealth>().TakeDamage(damage);  
            TakeDamage(1);  // Charger takes damage on collision
            ReturnToStartPosition();
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
            GameManager.Instance.AddExperience(Exp*GameManager.Instance.currentTierMultiplyer);
            GameManager.Instance.AddGold((int)(gold * GameManager.Instance.currentTierMultiplyer));
            SpawnManager.Instance.EnemyDestroyed(this.gameObject);
            Destroy(gameObject);  // Destroy the charger when health reaches 0
        }
    }



    // Collision detection for player damage
   
}
