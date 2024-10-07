using UnityEngine;

public class CoinPickup : MonoBehaviour
{
    public float moveSpeed = 10f; // Speed at which the coin moves towards the player
    public Vector3 minSize = new Vector3(0.1f, 0.1f, 0.1f); // Minimum size of the coin

    private Transform playerTransform;
    private bool isMovingToPlayer = false;
    private Vector3 originalSize;

    void Start()
    {
        originalSize = transform.localScale; // Store the original size of the coin
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // Assuming the player has a tag "Player"
        {
            playerTransform = other.transform;
            isMovingToPlayer = true;
        }
    }

    void Update()
    {
        if (isMovingToPlayer)
        {
            MoveToPlayer();
        }
    }

    void MoveToPlayer()
    {
        if (playerTransform != null)
        {
            float step = moveSpeed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, playerTransform.position, step);

            // Reduce the size of the coin
            float distance = Vector3.Distance(transform.position, playerTransform.position);
            float scaleFactor = Mathf.Clamp01(distance / moveSpeed);
            transform.localScale = Vector3.Lerp(minSize, originalSize, scaleFactor);

            // Check if the coin has reached the player
            if (distance < 0.001f)
            {
                // Optionally, add code here to handle what happens when the coin reaches the player
                // For example, increase player's score, play a sound, etc.
                Destroy(gameObject); // Destroy the coin or deactivate it
            }
        }
    }
}
