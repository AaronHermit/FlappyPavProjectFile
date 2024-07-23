using System.Collections;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public Pipes[] prefabs; // Change from single prefab to array
    public GameObject Enemy;
    public Transform startEnemy;
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
           

            //// Spawn the enemy prefab
            //GameObject enemyObject = Instantiate(Enemy, startEnemy.position, Quaternion.identity);

            //// Adjust position of enemy
            //Vector3 enemyPosition = enemyObject.transform.position;
            //enemyPosition += Vector3.back; // Move enemy back in z-axis (assuming this is intended)
            //enemyObject.transform.position = enemyPosition;

            //// Continuously move the enemy in the x-axis
            //StartCoroutine(MoveEnemy(enemyObject));

            // Set gap for pipes (assuming pipes.gap needs to be set here)
            
        }
    }

    private IEnumerator MoveEnemy(GameObject enemy)
    {
        while (true)
        {
            // Adjust the movement speed as needed (1 unit per second in this example)
            float movementSpeed = 1f;
            Vector3 currentPosition = enemy.transform.position;
            Vector3 newPosition = currentPosition + Vector3.left * movementSpeed * Time.deltaTime;
            enemy.transform.position = newPosition;

            yield return null;
        }
    }
}
