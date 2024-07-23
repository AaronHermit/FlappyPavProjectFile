using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[DefaultExecutionOrder(-1)]
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public bool gameStart;
    [SerializeField] private Player player;
    public Animator animator;
    [SerializeField] private GameObject mainPlayer;
    [SerializeField] private GameObject playerDead;
    [SerializeField] private GameObject playerAlive;
    [SerializeField] private Spawner spawner;
    [Header("Scoreset")]
    [SerializeField] private Text scoreText;
    [SerializeField] private Text FinalscoreText;
    [SerializeField] private Text coinText;
    [SerializeField] private Text FinalcoinText;
    [Header("Screens")]
    [SerializeField] private GameObject startPanel;
    [SerializeField] private GameObject gaemPanel;
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject gameoverPanel;
    [SerializeField] private GameObject gameOver;
    //[SerializeField] private Parallax groundParallax;
    [SerializeField] private GameObject instuctionInfo;
    [SerializeField] private float instuctionInfowaitTime;
    public int score { get; private set; } = 0;
    public int coin { get; private set; } = 0;

    private void Awake()
    {
        if (Instance != null) {
            DestroyImmediate(gameObject);
        } else {
            Instance = this;
        }
    }
    IEnumerator Instructiontimer()
    {
        mainPlayer.SetActive(false);
        instuctionInfo.SetActive(true);
        yield return new WaitForSeconds(instuctionInfowaitTime);
        instuctionInfo.SetActive(false);
        mainPlayer.SetActive(true);
    }
    private void OnDestroy()
    {
        if (Instance == this) {
            Instance = null;
        }
    }

    private void Start()
    {
        Pause();
        gameStart = false;
    }

    public void Pause()
    {
       
        
        Time.timeScale = 0f;
        player.enabled = false;
    }
    public void UnPause()
    {


        Time.timeScale = 1f;
        player.enabled = true;
    }

    public void PauseMnueOn()
    {
        pausePanel.SetActive(true);
        Pause();
    }
    public void PauseMnueOff()
    {
        pausePanel.SetActive(false);
        UnPause();
    }
    public void Play()
    {
        
        gaemPanel.SetActive(true);
        scoreText.enabled = true;
        playerDead.SetActive(false);
        playerAlive.SetActive(true);
        score = 0;
        coin = 0;
        coinText.text = coin.ToString();
        FinalscoreText.text = score.ToString();
        scoreText.text = score.ToString();
        FinalcoinText.text = coin.ToString();
        gameStart=true;
        startPanel.SetActive(false);
        gameoverPanel.SetActive(false);
        gameOver.SetActive(false);
        StartCoroutine(Instructiontimer());
        
        Time.timeScale = 1f;
        player.enabled = true;

        Pipes[] pipes = FindObjectsOfType<Pipes>();

        for (int i = 0; i < pipes.Length; i++) {
            Destroy(pipes[i].gameObject);
        }
    }
   
    public void Home()
    {
        gaemPanel.SetActive(false);
        gameStart = false;
        gameoverPanel.SetActive(false) ;
        startPanel.SetActive(true);
        pausePanel.SetActive(false );
        Pause();
    }
    public void GameOver()
    {
        gaemPanel.SetActive(false);
        playerDead.SetActive(true);
        playerAlive.SetActive(false);
        scoreText.enabled = false;
        gameStart = false;

        gameoverPanel.SetActive(true);
        gameOver.SetActive(true);
       
        Pause();
    }
   

    public void IncreaseScore()
    {
        score++;
        scoreText.text = score.ToString();
        FinalscoreText.text = score.ToString();
    }
    public void IncreaseCoins()
    {
        coin++;
        coinText.text = coin.ToString();
        FinalcoinText.text = "+" + coin.ToString();
    }


}
