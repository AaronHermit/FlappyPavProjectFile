using System.Collections;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public Pipes[] prefabs; // Change from single prefab to array
    public GameObject Enemy;
    public Transform startEnemy;
    public float enemySpawnRate = 5f;
    public float spawnRate = 1f;
    public float minHeight = -1f;
    public float maxHeight = 2f;
    public float verticalGap = 3f;
    public GameManager manager;

    private void OnEnable()
    {
        InvokeRepeating(nameof(Spawn), spawnRate, spawnRate);
       
    }

    private void OnDisable()
    {
        CancelInvoke(nameof(Spawn));
       
    }

    private void Spawn()
    {
        if (manager.gameStart)
        {
            // Select a random pipe prefab from the array
            int randomIndex = Random.Range(0, prefabs.Length);

            Pipes pipes = Instantiate(prefabs[randomIndex], transform.position, Quaternion.identity);
            pipes.transform.position += Vector3.up * Random.Range(minHeight, maxHeight);
            pipes.gap = verticalGap;

            // Check if pipes[1] is spawning (assuming prefabs[1] exists and it's the one you want)
            if (randomIndex == 1)
            {
                // Instantiate the enemy prefab next to the right side of pipes[1]
                GameObject newEnemy = Instantiate(Enemy, pipes.transform.position + new Vector3(verticalGap, 0f, 0f), Quaternion.identity);

                // Set the y position of the enemy prefab (assuming a constant y position)
                newEnemy.transform.position = new Vector3(newEnemy.transform.position.x, -3f, newEnemy.transform.position.z);

                // Make the enemy prefab a child of the pipes prefab
                newEnemy.transform.parent = pipes.transform;
            }

        }
    }
    

}
