using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlerfScript : MonoBehaviour
{


    public string playerTag = "Player"; // Tag of the player GameObject
    private float detectionDistance = 7f; // Distance at which something should happen

    private Transform playerTransform; // Reference to the player's Transform

    [SerializeField] private GameObject poopyAnimation;

    public Transform spawnPoint1; // First spawn point
    public Transform spawnPoint2; // Second spawn point
    public Transform poop;        // Object to move between spawn points

    private float movementSpeed = 2f; // Speed of movement

    


    void Start()
    {
     
        poop.position = spawnPoint1.position;

        // Find the GameObject with the playerTag and get its Transform
        GameObject playerObject = GameObject.FindGameObjectWithTag(playerTag);

        if (playerObject != null)
        {
            playerTransform = playerObject.transform;
        }
        else
        {
            Debug.LogWarning("Player with tag '" + playerTag + "' not found!");
        }
    }

    void Update()
    {
        if (playerTransform != null)
        {
            // Calculate the distance between the player and the enemy
            float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);

            // Check if the player is within the detection distance
            if (distanceToPlayer < detectionDistance)
            {
                //Debug.Log("Player is near the enemy!");
                // Move towards the spawn point
                poop.Translate((spawnPoint2.position - poop.position).normalized * movementSpeed * Time.deltaTime);
                // You can add your own actions or print statements here
            }
        }

        float DistanceToGround = Vector3.Distance(transform.position, spawnPoint2.position);

        if(DistanceToGround < 1)
        {
           
            poopyAnimation.SetActive(true);
            gameObject.GetComponent<SpriteRenderer>().enabled = false;
        }


    }
   


}
