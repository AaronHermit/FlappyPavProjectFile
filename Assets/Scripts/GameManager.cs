using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using static RequestHandler;

[DefaultExecutionOrder(-1)]
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public bool gameStart;
    [Header("Player")]
    [SerializeField] private Player player;
    [SerializeField] private Animator animator;
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
    [SerializeField] private GameObject Select_Character_Map_Panel;
    [SerializeField] private GameObject gaemPanel;
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject gameoverPanel;
    [SerializeField] private GameObject gameOver;
    [SerializeField] private GameObject instuctionInfo;
    [SerializeField] private float instuctionInfowaitTime;
    [Header("UserData textFields")]
    [SerializeField] private Text UserNameFinal;
    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip tapClip;
    [SerializeField] private AudioClip collectClip;
    [SerializeField] private AudioClip countClip_1;
    [SerializeField] private AudioClip countClip_2;
    [SerializeField] private AudioClip deadClip;
    [SerializeField] private AudioClip shootClip;
    [SerializeField] private GameObject audioManger;
    [Header("CountDown")]
    [SerializeField] private Text countdownText;
    [SerializeField] private float countdownTime = 3.0f;

    [Header("Power Ups")]
    [SerializeField] private GameObject rocketPowerUpElement;
    [SerializeField] private GameObject RocketPowerUpSpeedEffect;
    [SerializeField] private GameObject shieldPowerUpElement;
    [SerializeField] public bool ImmuneShieldPlayer;
    [SerializeField] public bool ImmuneRocketPlayer;
    [SerializeField] public Image rocketPowerupFillImage;
    [SerializeField] public Image shieldPowerupFillImage;

    [Header("Requests")]
    [SerializeField] private UserData userData;

    

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

    public void RocketPowerUp()
    {
        StartCoroutine(powerUpTimer(rocketPowerUpElement, "rocket",rocketPowerupFillImage));

    }
    public void ShieldPowerUp()
    {
        StartCoroutine(powerUpTimer(shieldPowerUpElement,"shield",shieldPowerupFillImage));
    }
    IEnumerator powerUpTimer(GameObject powerup,string power,Image powerupFillImage)
    {
        powerup.SetActive(true);
        
        if (power == "rocket")
        {
            
            RocketPowerUpSpeedEffect.SetActive(true);
            ImmuneRocketPlayer = true;
            mainPlayer.GetComponent<BoxCollider2D>().enabled = true;
        }
        if (power =="shield")
        {
            ImmuneShieldPlayer = true;

        }
        float duration = 5f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            powerupFillImage.fillAmount = Mathf.Lerp(1, 0, elapsed / duration);
            yield return null;
        }
        powerupFillImage.fillAmount = 1;
        powerup.SetActive(false);
        if (power == "shield")
        {
            ImmuneShieldPlayer = false;
        }
        if (power == "rocket")
        {
            ImmuneRocketPlayer = false;
            RocketPowerUpSpeedEffect.SetActive(false);
            mainPlayer.GetComponent<BoxCollider2D>().enabled = false;


        }
    }
    public void MuteAction(Toggle audioToggle)
    {
        if(audioToggle.isOn)
        {
            audioManger.SetActive(true);
        }
        else
        {
            audioManger.SetActive(false);
        }
    }
    private IEnumerator Countdown()
    {

        countdownTime = 3.0f;
        countdownText.enabled = true;
        Time.timeScale = 1f;
        while (countdownTime > 0)
        {
            audioSource.volume = 0.3f;
            audioSource.PlayOneShot(countClip_1);
            countdownText.text = countdownTime.ToString("0");
            yield return new WaitForSeconds(1.0f);
            countdownTime--;
           
        }
        audioSource.PlayOneShot(countClip_2);
        countdownText.text = "Go!";
        audioSource.volume = 0.5f;

    }

    public void ShootingSfx(bool shot)
    {
        if(!shot)
        {

        }

    }
    public void OpenSelectCharacterMapPanel(bool value)
    {
        if (value)
        {
            Select_Character_Map_Panel.SetActive(true);
        }
        else
        {
            Select_Character_Map_Panel.SetActive(false);
        }
    }
    public void PlayTap()
    {
        audioSource.PlayOneShot(tapClip);
    }
    
    IEnumerator Instructiontimer()
    {
        mainPlayer.SetActive(false);
        instuctionInfo.SetActive(true);
        yield return new WaitForSeconds(instuctionInfowaitTime);
        instuctionInfo.SetActive(false);
        mainPlayer.SetActive(true);
        gameStart = true;
        countdownText.enabled = false;
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
        StartCoroutine(spawner.EnableSpawningAfterDelay());
        rocketPowerupFillImage.fillAmount = 1;
        shieldPowerupFillImage.fillAmount = 1;
        StartCoroutine(Countdown());
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
        Select_Character_Map_Panel.SetActive(false) ;
        startPanel.SetActive(false);
        gameoverPanel.SetActive(false);
        gameOver.SetActive(false);
        StartCoroutine(Instructiontimer());
        
        
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
        if(!ImmuneShieldPlayer && !ImmuneRocketPlayer)
        {
           

            StopAllCoroutines();
            player.isWaiting = false;
            audioSource.PlayOneShot(deadClip);
            gaemPanel.SetActive(false);
            playerDead.SetActive(true);
            playerAlive.SetActive(false);
            scoreText.enabled = false;
            gameStart = false;
            rocketPowerUpElement.SetActive(false);
            shieldPowerUpElement.SetActive(false);
            gameoverPanel.SetActive(true);
            gameOver.SetActive(true);
            RocketPowerUpSpeedEffect.SetActive(false);
            Pause();
            var u = TelegramConnect.GetUserData();
            if(u != null)
            { 
                userData = JsonUtility.FromJson<UserData>(u);
                UserNameFinal.text = userData.user.first_name;
            }
        }
       
    }
   

    public void IncreaseScore()
    {
        //audioSource.PlayOneShot(scoreClip);
        score++;
        scoreText.text = score.ToString();
        FinalscoreText.text = score.ToString();
    }
    public void IncreaseCoins()
    {
        audioSource.PlayOneShot(collectClip);
        coin++;
        coinText.text = coin.ToString();
        FinalcoinText.text = "+" + coin.ToString();
    }


}
