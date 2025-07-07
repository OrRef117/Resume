using UnityEngine;
using System.Collections;

public class ShielderBehavior : MonoBehaviour
{
    [SerializeField] private ParticleSystem explosionEffect;
    public GameObject shieldObj;
    public float inviDur = 3f;
    public bool isInvi;

    public float health = 10;

    public float Exp = 55f;
    public float gold = 40f;
    private void Start()
    {
        isInvi = false;
        StartCoroutine(defend());
    }

    IEnumerator defend()
    {
        while (true)
        {
            setInvi(true);
            yield return new WaitForSeconds(3f);
            setInvi(false);
            yield return new WaitForSeconds(3f);
        }

    }


    public void setInvi(bool shift)
    {
        isInvi = shift;
        shieldObj.SetActive(shift);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (isInvi)
            return;

        if (collider.gameObject.CompareTag("PlayerProjectile"))
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
