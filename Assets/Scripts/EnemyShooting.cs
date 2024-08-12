using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooting : MonoBehaviour
{
    public GameObject bulletPrefab; // Prefab of the bullet to shoot
    public Transform firePoint;     // Point from where the bullet will be instantiated
    public float shootInterval = 5f; // Interval between each shot
    private float bulletSpeed = 15f;
    private GameManager gameManager;
    void Start()
    {
        gameManager = GameManager.Instance;
        // Start the shooting coroutine
        StartCoroutine(ShootContinuously());
    }

    IEnumerator ShootContinuously()
    {
        while (true)
        {

            // Calculate shoot direction along the local Y-axis of firePoint
            Vector2 shootDirection = firePoint.up;
            // Instantiate a bullet at the fire point
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);

            // Get the bullet's rigidbody (assuming the bullet has a Rigidbody2D component)
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();

            // Shoot the bullet in the calculated direction
            rb.velocity = shootDirection.normalized * bulletSpeed; // Adjust the speed as needed

            StartCoroutine(DestroyBulletDelayed(bullet));

            // Wait for shootInterval seconds before shooting again
            yield return new WaitForSeconds(shootInterval);
        }
    }

    IEnumerator DestroyBulletDelayed(GameObject bullet)
    {
        GameObject childObject = bullet.transform.Find("child")?.gameObject; 
        // Wait for 3 seconds
        yield return new WaitForSeconds(0.5f);

        // Check if the bullet still exists (it might have been destroyed by other means)
        if (bullet != null)
        {
            childObject.SetActive(true);
            // Destroy the bullet after 4 seconds from the time it was instantiated
            Destroy(bullet, 0.5f);
        }
    }

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if(collision.gameObject.CompareTag("Obstacle"))
    //    {
    //        GameObject childObject = collision.transform.Find("child")?.gameObject;
    //        childObject.SetActive(true);
    //        Debug.Log("knocked");
    //        Destroy(collision.gameObject, 1f);
    //    }
    //}

}
