using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotherShipBehavior : MonoBehaviour
{
    [SerializeField] private ParticleSystem explosionEffect;
    public float Exp = 340f;
    public float gold = 200f;
    public enum BossState { Stage1, Stage2, Transitioning }
    public BossState currentState = BossState.Stage1;

    [Header("Components")]
    public GameObject helmet;
    public GameObject leftLaser;
    public GameObject rightLaser;
    public Transform leftArm;
    public Transform rightArm;
    

    [Header("Projectile")]
    public GameObject armProjectile;
    public Transform armFirePointLeft;
    public Transform armFirePointRight;
    public Transform armFirePointLeftStage2;
    public Transform armFirePointRightStage2;

    [Header("Health")]
    public float maxHealth = 100f;
    public float currentHealth;
    public float stage2Threshold = 50f;

    void Start()
    {
        currentHealth = maxHealth;
        StartCoroutine(Stage1Behavior());
    }

    void Update()
    {
        if (currentState == BossState.Stage1 && currentHealth <= stage2Threshold)
        {
            currentState = BossState.Transitioning;
            StopAllCoroutines();
            StartCoroutine(TransitionToStage2());
        }
    }

    // ------------------- STAGE 1 -------------------

    IEnumerator Stage1Behavior()
    {
        currentState = BossState.Stage1;

        while (currentState == BossState.Stage1)
        {
            yield return MoveToPosition(new Vector3(-5,7,0));
            yield return ShootingSequence();

            yield return MoveToPosition(new Vector3(5, 7, 0));
            yield return ShootingSequence();

            yield return MoveToPosition(new Vector3(0, 7, 0));
            yield return ShootingSequence();

            yield return ChargeLaserAttack();
        }
    }

    IEnumerator ShootingSequence()
    {
        float[] angles = { 20f, 10f, 0f, -10f, -20f };
        for (int i = 0; i < angles.Length; i++)
        {
            leftArm.localRotation = Quaternion.Euler(0, 0, angles[i]);
            rightArm.localRotation = Quaternion.Euler(0, 0, -angles[i]);

            Instantiate(armProjectile, armFirePointLeft.position, armFirePointLeft.rotation);
            Instantiate(armProjectile, armFirePointRight.position, armFirePointRight.rotation);

            yield return new WaitForSeconds(0.2f);
        }
    }

    IEnumerator ChargeLaserAttack()
    {
        // Optional: play VFX or SFX for charging here
        leftArm.localRotation = Quaternion.Euler(0, 0, 0);
        rightArm.localRotation = Quaternion.Euler(0, 0, 0);
        yield return new WaitForSeconds(1.5f);
        helmet.GetComponent<LaserShooter>().Fire();
        leftLaser.GetComponent<LaserShooter>().Fire();
        rightLaser.GetComponent<LaserShooter>().Fire();
        yield return new WaitForSeconds(1.5f);
    }

    IEnumerator MoveToPosition(Vector3 targetPos)
    {
        float speed = 10f;
        while (Vector3.Distance(transform.position, targetPos) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
            yield return null;
        }
    }

    // ------------------- TRANSITION -------------------

    IEnumerator TransitionToStage2()
    {
        // Remove helmet and turrets visually and disable logic
        Destroy(helmet);
        Destroy(leftLaser);
        Destroy(rightLaser);

        yield return new WaitForSeconds(1f);
        StartCoroutine(Stage2Behavior());
    }

    // ------------------- STAGE 2 -------------------

    public GameObject mirrorPrefab;
    private List<GameObject> activeMirrors = new List<GameObject>();

    IEnumerator Stage2Behavior()
    {
        currentState = BossState.Stage2;

        while (true)
        {
            yield return MoveToPosition(new Vector3(0,7,0));
            yield return SummonMirrors();

            yield return BounceShots();

            DestroyAllMirrors();

            yield return FrenzyShot();
            yield return Overcharge();
        }
    }

    IEnumerator SummonMirrors()
    {
        activeMirrors.Clear(); // clear previous mirrors if any

        int mirrorCount = 3;
        for (int i = 0; i < mirrorCount; i++)
        {
            float x = Random.Range(-9f, 9f);
            float y = Random.Range(-5f, 0f);
            Vector2 spawnPos = new Vector2(x, y);

            GameObject mirror = Instantiate(mirrorPrefab, spawnPos, Quaternion.identity);
            activeMirrors.Add(mirror);
        }

        yield return new WaitForSeconds(1f);
    }

    IEnumerator BounceShots()
    {
        int shots = 5;
        for (int i = 0; i < shots; i++)
        {
            FireBounceShot();
            yield return new WaitForSeconds(1f);
        }
    }

    void FireBounceShot()
    {
        activeMirrors.RemoveAll(m => m == null);

        if (activeMirrors.Count == 0)
        {
            Debug.Log("No active mirrors to shoot at.");
            return;
        }

        GameObject targetMirror = activeMirrors[Random.Range(0, activeMirrors.Count)];
        Vector2 mirrorPos = targetMirror.transform.position;

        Transform chosenFirePoint = mirrorPos.x < 0 ? armFirePointLeftStage2 : armFirePointRightStage2;
        Vector2 direction = (mirrorPos - (Vector2)chosenFirePoint.position).normalized;

        GameObject proj = Instantiate(armProjectile, chosenFirePoint.position, Quaternion.identity);
        proj.transform.up = -direction;
    }

    void DestroyAllMirrors()
    {
        foreach (GameObject mirror in GameObject.FindGameObjectsWithTag("EnemyMirror"))
        {
            Destroy(mirror);
        }
    }

    IEnumerator FrenzyShot()
    {
        float angleStep = 10f;
        float startAngle = -45f;
        float endAngle = 45f;

        float safeAngle = Random.Range(startAngle, endAngle); // Choose a random "safe spot"

        for (float angle = startAngle; angle <= endAngle; angle += angleStep)
        {
            if (Mathf.Abs(angle - safeAngle) < angleStep)
                continue; // skip this angle = safe zone

            Quaternion rotation = Quaternion.Euler(0, 0, angle);
            Instantiate(armProjectile, armFirePointLeftStage2.position, rotation);
            Instantiate(armProjectile, armFirePointRightStage2.position, rotation);
        }

        yield return new WaitForSeconds(2f);
    }

    IEnumerator Overcharge()
    {
        // VFX + Delay
        Debug.Log("OVERCHARGE!");
        yield return new WaitForSeconds(3f);
    }

    // ------------------- DAMAGE SYSTEM -------------------





    private void OnTriggerEnter2D(Collider2D collider)
    {

        if (collider.gameObject.CompareTag("PlayerProjectile"))
        {
            // Take damage from projectiles
            TakeDamage(collider.gameObject.GetComponent<Projectile>().dmg);
            Debug.Log("hit" + collider.gameObject.GetComponent<Projectile>().dmg);
        }
    }

    public void TakeDamage(float damageAmount)
    {
        currentHealth -= damageAmount;
        if (currentHealth <= 0)
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
