using UnityEngine;

public class LaserShooter : MonoBehaviour
{
    public GameObject laserPrefab;
    public Transform firePoint;
    public void Fire()
    {
        float beamLength = 120f;
        Vector3 direction = Vector3.down;

        GameObject laser = Instantiate(laserPrefab, firePoint.position, Quaternion.identity);
        laser.transform.up = direction;

        // Stretch the visual
        Vector3 scale = laser.transform.localScale;
        scale.y = beamLength;
        laser.transform.localScale = scale;

        // Stretch the collider to match
        BoxCollider2D collider = laser.GetComponent<BoxCollider2D>();
        if (collider != null)
        {
            collider.size = new Vector2(collider.size.x, beamLength);
            collider.offset = new Vector2(0, beamLength / 2f); // Push it down so it stays in the stretched area
        }

        // Optional: destroy the laser after a bit
        Destroy(laser, 1.5f);
    }
}
