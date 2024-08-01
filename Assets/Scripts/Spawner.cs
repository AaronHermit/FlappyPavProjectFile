using System.Collections;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public Pipes[] prefabs; // Array of pipe prefabs
    public Pipes slerpPrefabs; // Slerp-specific prefab
    public GameObject Enemy; // Enemy prefab
    public Transform startEnemy; // Transform where the enemy spawns
    public float enemySpawnRate = 5f; // Rate at which enemies spawn
    public float spawnRate = 1f; // Rate at which pipes spawn
    public float minHeight = -1f; // Minimum height for pipe spawn
    public float maxHeight = 2f; // Maximum height for pipe spawn
    public float verticalGap = 4.5f; // Gap between pipes
    public GameManager manager; // Reference to the game manager
    public float slerpSpawnProbability = 0.2f; // Probability of spawning slerp prefab
    public float enemySpawnProbability = 0.2f; // Probability of spawning enemy

    public bool canMainSpawnSlerp = false; // Cooldown flag for slerpPrefabs
    public bool canMainSpawnEnemy = false; // Cooldown flag for Enemy
    public bool canSpawnSlerp = true; // Cooldown flag for slerpPrefabs
    public bool canSpawnEnemy = true; // Cooldown flag for Enemy
    public float slerpCooldown = 5f; // Cooldown time for slerpPrefabs
    public float enemyCooldown = 5f; // Cooldown time for Enemy

    public float spawnIncreaseInterval = 60f; // Interval to increase spawn rates (1 minute)

    private void OnEnable()
    {
        InvokeRepeating(nameof(Spawn), spawnRate, spawnRate);
        StartCoroutine(IncreaseSpawnRates());
    }

    private void OnDisable()
    {
        CancelInvoke(nameof(Spawn));
        StopCoroutine(IncreaseSpawnRates());
    }

    private void Spawn()
    {
        if (manager.gameStart)
        {
            // Check if we should spawn the slerp prefab based on the defined probability
            bool spawnSlerp = Random.value < slerpSpawnProbability && canSpawnSlerp;

            Pipes prefabToSpawn;
            // Select a random pipe prefab from the array
            int randomIndex = Random.Range(0, prefabs.Length);
            if (spawnSlerp && canMainSpawnSlerp)
            {
               
                prefabToSpawn = slerpPrefabs;
                StartCoroutine(SlerpCooldown()); // Start the cooldown for slerpPrefabs
            }
            else
            {
                // Select a random pipe prefab from the array
                prefabToSpawn = prefabs[randomIndex];
            }

            Pipes pipes = Instantiate(prefabToSpawn, transform.position, Quaternion.identity);
            pipes.transform.position += Vector3.up * Random.Range(minHeight, maxHeight);
            pipes.gap = verticalGap;

            // Generate a random probability within the specified range
            bool spawnEnemy = Random.value < enemySpawnProbability && canSpawnEnemy;

            // Check if enemy meets the random probability condition
            if (spawnEnemy && canMainSpawnEnemy && !canSpawnSlerp)
            {
               
                // Instantiate the enemy prefab next to the right side of pipes
                GameObject newEnemy = Instantiate(Enemy, pipes.transform.position + new Vector3(verticalGap, 0f, 0f), Quaternion.identity);

                // Set the y position of the enemy prefab (assuming a constant y position)
                newEnemy.transform.position = new Vector3(newEnemy.transform.position.x, -3f, newEnemy.transform.position.z);

                // Make the enemy prefab a child of the pipes prefab
                newEnemy.transform.parent = pipes.transform;

                StartCoroutine(EnemyCooldown()); // Start the cooldown for Enemy
            }
        }
    }

    private IEnumerator SlerpCooldown()
    {
        canSpawnSlerp = false;
        yield return new WaitForSeconds(slerpCooldown);
        canSpawnSlerp = true;
    }

    private IEnumerator EnemyCooldown()
    {
        canSpawnEnemy = false;
        yield return new WaitForSeconds(enemyCooldown);
        canSpawnEnemy = true;
    }

    private IEnumerator IncreaseSpawnRates()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnIncreaseInterval);

            // Increase the spawn probabilities gradually
            slerpSpawnProbability = Mathf.Min(slerpSpawnProbability + 0.05f, 1f); // Ensure it doesn't exceed 100%
            enemySpawnProbability = Mathf.Min(enemySpawnProbability + 0.05f, 1f); // Ensure it doesn't exceed 100%

            // Decrease the cooldowns gradually
            slerpCooldown = Mathf.Max(slerpCooldown - 0.5f, 1f); // Ensure it doesn't go below 1 second
            enemyCooldown = Mathf.Max(enemyCooldown - 0.5f, 1f); // Ensure it doesn't go below 1 second
        }
    }
    public IEnumerator EnableSpawningAfterDelay()
    {
        canMainSpawnSlerp = false;
        canMainSpawnEnemy = false;
        yield return new WaitForSeconds(60f); // Wait for 1 minute
        
        canMainSpawnSlerp = true;
        canMainSpawnEnemy = true;
        canSpawnSlerp = true; // Cooldown flag for slerpPrefabs
        canSpawnEnemy = true; // Cooldown flag for Enemy
    }
}
