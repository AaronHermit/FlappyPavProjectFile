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
           
            
        }
    }
    

}
