using System.Collections;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public Pipes[] prefabs; // Array of pipe prefabs
    public Pipes[] map_0_prefabs;
    public Pipes[] map_1_prefabs;
    public Pipes[] map_2_prefabs;
    public Pipes[] map_3_prefabs;
    public Pipes[] map_4_prefabs;
    public Pipes[] map_5_prefabs;
    public Pipes[] map_6_prefabs;

    public Pipes slerpPrefabs; // Slerp-specific prefab
    public Pipes map_0_slerpPrefabs;
    public Pipes map_1_slerpPrefabs;
    public Pipes map_2_slerpPrefabs;
    public Pipes map_3_slerpPrefabs;
    public Pipes map_4_slerpPrefabs;
    public Pipes map_5_slerpPrefabs;
    public Pipes map_6_slerpPrefabs;
    public GameObject tank; // Enemy prefab
    public GameObject map_0_tank; // Enemy prefab
    public GameObject map_1_tank; // Enemy prefab
    public GameObject map_2_tank; // Enemy prefab
    public GameObject map_3_tank; // Enemy prefab
    public GameObject map_4_tank; // Enemy prefab
    public GameObject map_5_tank; // Enemy prefab
    public GameObject map_6_tank; // Enemy prefab
    public float enemySpawnRate = 5f; // Rate at which enemies spawn
    public float spawnRate = 1f; // Rate at which pipes spawn
    public float minHeight = -1f; // Minimum height for pipe spawn
    public float maxHeight = 2f; // Maximum height for pipe spawn
    public float verticalGap = 1f; // Gap between pipes
    public GameManager manager;
    public float slerpSpawnProbability = 0.05f;
    public float enemySpawnProbability = 0.1f;

    [Header("Coin Probabilty")]
    public float probabilityForIndex6 = 0.2f; // 20% chance
    public int prefabIndexToAlwaysSpawn = 6;

    bool canSpawnEnemies;
    bool canSpawnTank;
    bool canSpawnSlerp;


    public float spawnIncreaseInterval = 60f; // Interval to increase spawn rates (1 minute)

    private void OnEnable()
    {
        InvokeRepeating(nameof(Spawn), spawnRate, spawnRate);

    }

    public void chooseTheRightMap(int value)
    {
        switch (value)
        {
            case 1:
                prefabs = map_0_prefabs;
                slerpPrefabs = map_0_slerpPrefabs;
                tank = map_0_tank;
                break;
            case 2:
                prefabs = map_1_prefabs;
                slerpPrefabs = map_1_slerpPrefabs;
                tank = map_1_tank;

                break;
            case 3:
                prefabs = map_2_prefabs;
                slerpPrefabs = map_2_slerpPrefabs;
                tank = map_2_tank;

                break;
            case 4:
                prefabs = map_3_prefabs;
                slerpPrefabs = map_3_slerpPrefabs;
                tank = map_3_tank;

                break;
            case 5:
                prefabs = map_4_prefabs;
                slerpPrefabs = map_4_slerpPrefabs;
                tank = map_4_tank;

                break;
            case 6:
                prefabs = map_5_prefabs;
                slerpPrefabs = map_5_slerpPrefabs;
                tank = map_5_tank;

                break;
            case 7:
                prefabs = map_6_prefabs;
                slerpPrefabs = map_6_slerpPrefabs;
                tank = map_6_tank;

                break;
            default:
                prefabs = map_0_prefabs;
                slerpPrefabs = map_0_slerpPrefabs;
                tank = map_0_tank;

                break;
        }
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

            // Determine which prefab to spawn
            if (spawnSlerp && canSpawnEnemies && canSpawnSlerp)
            {
                // Ensure slerpPrefabs is not null
                if (slerpPrefabs != null)
                {
                    prefabToSpawn = slerpPrefabs;
                    Debug.Log("Spawning Slerp prefab.");
                }
                else
                {
                    Debug.LogWarning("slerpPrefabs is null. Cannot spawn.");
                    return; // Exit if no slerp prefab is available
                }
            }
            else
            {
                // Generate a random index for normal pipe prefabs
                int randomIndex = Random.Range(0, prefabs.Length);

                // Validate index and array bounds
                if (prefabs.Length == 0)
                {
                    Debug.LogWarning("No prefabs available to spawn.");
                    return; // Exit if there are no prefabs
                }

                // Determine which normal pipe prefab to spawn
                float randomValue = Random.value;

                if (randomValue < probabilityForIndex6 && prefabIndexToAlwaysSpawn < prefabs.Length)
                {
                    prefabToSpawn = prefabs[prefabIndexToAlwaysSpawn];
                    // Debug.Log($"Spawning always at index {prefabIndexToAlwaysSpawn}");
                }
                else
                {
                    if (randomIndex == 6 && prefabIndexToAlwaysSpawn < prefabs.Length)
                    {
                        randomIndex = (randomIndex - 1 + prefabs.Length) % prefabs.Length; // Ensure valid index within range
                    }
                    prefabToSpawn = prefabs[randomIndex];
                    // Debug.Log($"Spawning random prefab at index {randomIndex}");
                }
            }

            // Instantiate the selected prefab and adjust its position
            Pipes pipes = Instantiate(prefabToSpawn, transform.position, Quaternion.identity);
            pipes.transform.position += Vector3.up * Random.Range(minHeight, maxHeight);
            pipes.gap = verticalGap;

            // Determine if an enemy should spawn
            bool spawnEnemy = Random.value < enemySpawnProbability;

            if (spawnEnemy && canSpawnEnemies && canSpawnTank)
            {
                // Ensure tank prefab is not null
                if (tank != null)
                {
                    GameObject newEnemy = Instantiate(tank, pipes.transform.position + new Vector3(verticalGap, 0f, 0f), Quaternion.identity);
                    newEnemy.transform.position = new Vector3(newEnemy.transform.position.x, -3f, newEnemy.transform.position.z);
                    newEnemy.transform.parent = pipes.transform;

                    Debug.Log("Enemy spawned successfully next to pipes.");
                }
                else
                {
                    Debug.LogWarning("Tank prefab is null. Cannot spawn enemy.");
                }
            }
        }
        else
        {
            Debug.Log("Game not started. No spawning.");
        }
    }

    public IEnumerator intialDelay(int delay)
    {
        canSpawnEnemies = false;
        yield return new WaitForSeconds(delay);
        Debug.Log("can spwan now");
        canSpawnEnemies = true;
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
    public IEnumerator OnStartTank(int delay)
    {
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
