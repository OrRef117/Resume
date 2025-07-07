using UnityEngine;
using System.Collections;
public class TheKingBehavior : MonoBehaviour
{
    [SerializeField] private ParticleSystem explosionEffect;
    public GameObject orangeProjectilePrefab;          // The projectile prefab to instantiate
    public GameObject purpleProjectilePrefab;          // The projectile prefab to instantiate
    public Transform leftGunPivot;               // Pivot point of the left gun
    public Transform rightGunPivot;              // Pivot point of the right gun
    public Transform leftGunFiringPosition;      // Firing position of the left gun
    public Transform rightGunFiringPosition;     // Firing position of the right gun
    public float fireInterval = 0.5f;            // Time between each shot (in seconds)
    public int numberOfProjectiles = 10;         // Number of projectiles to shoot in automatic mode
    public float rotationSpeed = 0.5f;            // Rotation speed of the guns (degrees per second)
    public float vibrationAmount = 0.01f;        // Amount of vibration when overheating
    public float vibrationDuration = 4f;         // Duration of the vibration during overheating
    public Transform midfirePos;
    
    public float health =  100f;
    public float exp =  213;
    public float gold =  150;
    


    void Start()
    {
        
        StartCoroutine(EnemyCycle());
    }

    void Update()
    {
        
    }

    // The cycle that the enemy will repeat until destroyed
    private IEnumerator EnemyCycle()
    {
        while (true)  // Infinite loop until destroyed
        {
            RotateGuns(0f);
            // Step 1: Move to the left and fire
            yield return StartCoroutine(MoveAndFire(Vector3.left));

            // Step 2: Move to the right and fire
            yield return StartCoroutine(MoveAndFire(Vector3.right));


            yield return StartCoroutine(MoveAndFire(Vector3.zero));

            // Step 3: Move to the middle position and fire projectiles
            yield return StartCoroutine(MoveToMiddleAndAutomaticFire());

            // Step 4: Overheating (vibration for 4 seconds)
            yield return StartCoroutine(Overheat());

            // Wait before repeating the cycle
            yield return null;  // Add a small wait here if needed, or let it repeat continuously
        }
    }

    // Move to the left/right and fire projectiles
    private IEnumerator MoveAndFire(Vector3 direction)
    {
        // Move the enemy in the specified direction
        Vector3 targetPosition = new Vector3(direction.x * 8f, transform.position.y, 0);
        Debug.Log(targetPosition);

        // Move speed
        float moveSpeed = 22f;

        // Desired rotation angle based on the target position
        float targetAngle = direction.x > 0 ? -8f : (direction.x < 0 ? 8f : 0f);
        while (Vector3.Distance(transform.position, targetPosition) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

            // Smoothly rotate the gun pivot
            float currentAngle = Mathf.LerpAngle(leftGunPivot.transform.localRotation.z, targetAngle,rotationSpeed);
            RotateKing(currentAngle);
            yield return null;
        }
          // Random angle between -30 and 30 degrees
        
        FireProjectiles(false);

        // Wait for the firing sequence to finish (optional: add a delay here if needed)
        yield return new WaitForSeconds(1f);
    }

    // Move to the middle and do automatic fire with recoil
    private IEnumerator MoveToMiddleAndAutomaticFire()
    {
        // Move to the middle position
        Vector3 targetPosition = new Vector3(0, transform.position.y, transform.position.z);  // Middle position
        float moveSpeed = 5f;


        while (Vector3.Distance(transform.position, targetPosition) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
            yield return null;
        }

        // Now enter automatic mode (guns rotate to 45° and -45°)
        //isInAutomaticMode = true;
        RotateGuns(60f);  // Rotate to full automatic mode

        // Fire projectiles automatically
        for (int i = 0; i < numberOfProjectiles; i++)
        {
            FireProjectiles(true);


            yield return new WaitForSeconds(0.2f);  // Delay between shots in automatic mode
        }

        // End automatic mode and stop recoil
       // isInAutomaticMode = false;
    }

    // Overheating (vibration effect)
    private IEnumerator Overheat()
    {
        float timeElapsed = 0f;

        while (timeElapsed < vibrationDuration)
        {
            // Vibrate slowly
            transform.position += new Vector3(Random.Range(-vibrationAmount, vibrationAmount),
                                              Random.Range(-vibrationAmount, vibrationAmount), 0f);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

    }

    // Method to rotate both gun pivots based on the current angle
    private void RotateGuns(float angle)
    {
        leftGunPivot.localRotation = Quaternion.Euler(0, 0, angle);  // Rotate the left gun
        rightGunPivot.localRotation = Quaternion.Euler(0, 0, -angle); // Rotate the right gun (opposite angle)
    }
    private void RotateKing(float angle)
    {
        transform.localRotation = Quaternion.Euler(0, 0, angle);  
    }

    // Method to fire projectiles at the current angles of the guns
    private void FireProjectiles(bool auto)
    {
        if (!auto)
        {
            for (int i = 0; i < numberOfProjectiles; i++)
            {
                // Calculate the angle for the current projectile
                float angle = Mathf.Lerp(-45f, 45f, (float)i / (numberOfProjectiles - 1));  // Calculate the angle between -45° and 45°

                // Calculate the direction based on the angle
                Vector3 direction = Quaternion.Euler(0, 0, angle) * Vector3.right; 

                // Instantiate projectiles at the left and right gun firing positions
                InstantiateProjectile(leftGunFiringPosition.position, direction,true);
                InstantiateProjectile(rightGunFiringPosition.position, direction,true);
            }
        }
        else
        {
            Vector3 direction = (rightGunPivot.position - leftGunPivot.position).normalized;

            InstantiateProjectile(midfirePos.position, direction,false);
           // InstantiateProjectile(rightGunFiringPosition.position, direction);
        }
    }

    // Method to instantiate a projectile and set its direction
    private void InstantiateProjectile(Vector3 position, Vector3 direction,bool proj)
    {
        
        // Instantiate the projectile and give it the correct rotation and position
        if (proj)
        {
            GameObject projectile = Instantiate(orangeProjectilePrefab, position, Quaternion.LookRotation(Vector3.forward, direction));
            RoyalProjectileOrange projectileScript = projectile.GetComponent<RoyalProjectileOrange>();
            if (projectileScript != null)
            {
                projectileScript.SetDirection(direction); // Pass the direction to the projectile
            }
        }
        else
        {
            GameObject projectile = Instantiate(purpleProjectilePrefab, position, Quaternion.LookRotation(Vector3.forward, direction));
            RoyalProjectilePurple projectileScript = projectile.GetComponent<RoyalProjectilePurple>();
            if (projectileScript != null)
            {
                projectileScript.SetDirection(direction); // Pass the direction to the projectile
            }
        }
        
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
            GameManager.Instance.AddExperience(exp * GameManager.Instance.currentTierMultiplyer);
            GameManager.Instance.AddGold((int)(gold * GameManager.Instance.currentTierMultiplyer));
            SpawnManager.Instance.EnemyDestroyed(this.gameObject);
            Destroy(gameObject);  // Destroy the charger when health reaches 0
        }
    }


}
