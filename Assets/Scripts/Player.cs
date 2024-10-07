using System;
using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    
    public Sprite[] sprites;

    public float strength = 5f;
    public float gravity = -9.81f;
    public float tilt = 5f;

    private SpriteRenderer spriteRenderer;
    private Vector3 direction;
    private int spriteIndex;
    [SerializeField] GameManager gameManager;

    public bool isWaiting = false;
    public bool isWaitingtrail = false;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {

        Application.targetFrameRate = 80;
        //InvokeRepeating(nameof(AnimateSprite), 0.15f, 0.15f);
    }

    private void OnEnable()
    {
        Vector3 position = transform.position;
        position.y = 0f;
        transform.position = position;
        direction = Vector3.zero;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            direction = Vector3.up * strength;
            gameManager.PlayTap();
        }

        if (!isWaiting)
        {
            // Apply gravity and update the position
            direction.y += gravity * Time.deltaTime;
            transform.position += direction * Time.deltaTime;

            // Tilt the bird based on the direction
            Vector3 rotation = transform.eulerAngles;
            rotation.z = direction.y * tilt;
            transform.eulerAngles = rotation;
        }
    }

    // Button click handler to move the player to y = 0 and wait for 5 seconds
    public void OnButtonClick()
    {
        StartCoroutine(MoveToYZeroAndWait());
    }

    public IEnumerator MoveToYZeroAndWait()
    {
        isWaiting = true;
        GameManager.Instance.RocketPowerUp();
        Vector3 position = transform.position;
        position.y = 0f;
        transform.position = position;
        yield return new WaitForSeconds(3f);
        isWaiting = false;
        
    }

    

    public void OnButtonBuff()
    {
        StartCoroutine(isLifeBuffTimer());

    }
    private IEnumerator isLifeBuffTimer()
    {
        isWaiting = false;
        Vector3 position = transform.position;
        position.y = 0f;
        transform.position = position;
        yield return new WaitForSeconds(1f);
        isWaiting = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Obstacle"))
        {
            GameManager.Instance.GameOver();
            try
            {
                TelegramConnect.HapticFeedback("warning");
            }
            catch (Exception e)
            {
                Debug.Log("Error: " + e.Message);
            }
        }
        else if (other.gameObject.CompareTag("Scoring"))
        {
            GameManager.Instance.IncreaseScore();
            //Destroy(other.gameObject);
        }
        else if (other.gameObject.CompareTag("Coins"))
        {
            GameManager.Instance.IncreaseCoins();
            other.gameObject.GetComponent<BoxCollider2D>().enabled = false;
        }
        else if (other.gameObject.CompareTag("Finish"))
        {
            GameManager.Instance.ImmuneShieldPlayer = false;
            GameManager.Instance.GameOver();
        }
    }
    
   
}
