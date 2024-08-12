using System.Collections;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public Pipes[] prefabs; // Array of pipe prefabs
    public Pipes slerpPrefabs; // Slerp-specific prefab
    public GameObject tank; // Enemy prefab
    public float enemySpawnRate = 5f; // Rate at which enemies spawn
    public float spawnRate = 1f; // Rate at which pipes spawn
    public float minHeight = -1f; // Minimum height for pipe spawn
    public float maxHeight = 2f; // Maximum height for pipe spawn
    public float verticalGap = 2f; // Gap between pipes
    public GameManager manager; 
    public float slerpSpawnProbability = 0.05f; 
    public float enemySpawnProbability = 0.1f;

    bool canSpawnEnemies;
    bool canSpawnTank;
    bool canSpawnSlerp;
    

    public float spawnIncreaseInterval = 60f; // Interval to increase spawn rates (1 minute)

    private void OnEnable()
    {
        InvokeRepeating(nameof(Spawn), spawnRate, spawnRate);
        
    }

    private void OnDisable()
    {
        CancelInvoke(nameof(Spawn));
       
    }
    private void Start()
    {
        canSpawnEnemies = false;
    }
    private void Spawn()
    {
        if (manager.gameStart)
        {
            // Check if we should spawn the slerp prefab based on the defined probability
            bool spawnSlerp = Random.value < slerpSpawnProbability;

            Pipes prefabToSpawn;
            // Select a random pipe prefab from the array
            int randomIndex = Random.Range(0, prefabs.Length);
            if (spawnSlerp && canSpawnEnemies && canSpawnSlerp)
            {
               
                prefabToSpawn = slerpPrefabs;
               
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
            bool spawnEnemy = Random.value < enemySpawnProbability;

            // Check if enemy meets the random probability condition
            if (spawnEnemy && canSpawnEnemies && canSpawnTank)
            {
               
                // Instantiate the enemy prefab next to the right side of pipes
                GameObject newEnemy = Instantiate(tank, pipes.transform.position + new Vector3(verticalGap, 0f, 0f), Quaternion.identity);

                // Set the y position of the enemy prefab (assuming a constant y position)
                newEnemy.transform.position = new Vector3(newEnemy.transform.position.x, -3f, newEnemy.transform.position.z);

                // Make the enemy prefab a child of the pipes prefab
                newEnemy.transform.parent = pipes.transform;
               
            }
        }
    }

    public IEnumerator intialDelay(int delay)
    {
        canSpawnEnemies = false;
        yield return new WaitForSeconds(delay);
        Debug.Log("can spwan now");
        canSpawnEnemies=true;
        StartCoroutine(OnStartSlerp(delay));
    }
    public IEnumerator OnStartSlerp(int delay)
    {
        canSpawnSlerp = true;
        canSpawnTank = false;
        yield return new WaitForSeconds(delay);
        Debug.Log("can spwan slerp now");
        StartCoroutine(OnStartTank(delay));
    }
    public IEnumerator OnStartTank(int delay) {
        canSpawnSlerp = false;
        canSpawnTank = true;
        yield return new WaitForSeconds(delay);
        Debug.Log("can spwan tank now");
        StartCoroutine(OnStartBoth(delay));
    }

    public IEnumerator OnStartBoth(int delay)
    {
        canSpawnTank = false;
        canSpawnSlerp = true;
        yield return new WaitForSeconds(delay);
        canSpawnTank = true;
        canSpawnSlerp = true;
    }

}
