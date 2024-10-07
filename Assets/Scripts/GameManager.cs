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
    [SerializeField] private GameObject profilePanel;
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
    private int currentScore = 0;

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

    [Header("Selection Character and Enviroment")]
    [SerializeField] ScrollRectTracker characterScroll;
    [SerializeField] ScrollRectTracker enviromentScroll;
    [SerializeField] GameObject[] charactersAlive;
    [SerializeField] GameObject[] charactersDeath;
    [SerializeField] GameObject[] instructionCharacter;
    [SerializeField] GameObject[] maps;
    [SerializeField] Sprite[] charcterSprite;
    [SerializeField] SpriteRenderer characterRenderSprite;
    [SerializeField] private Image leftmove;
    [SerializeField] private Image rightmove;
    [SerializeField] private Sprite activeleftSprite;
    [SerializeField] private Sprite activerightSprite;
    [SerializeField] private Sprite inactiveleftSprite;
    [SerializeField] private Sprite inactiverightSprite;

    [Header("Buffs")]
    [SerializeField] public bool isLifeBuff;
    [SerializeField] public int isLifeCount;
    [SerializeField] public bool is2xBuff;
    [SerializeField] public bool is5xBuff;
    [SerializeField] public GameObject ExtraLifeEffect;
    [SerializeField] public Toggle isLifeBuffToggle;
    [SerializeField] public Toggle is2xBuffToggle;
    [SerializeField] public Toggle is5xBuffToggle;
    [SerializeField] public Toggle[] buffsToggle;

    [Header("Request Handler")]
    [SerializeField] private RequestHandler requestHandler;

    public int score { get; private set; } = 0;
    public int coin { get; private set; } = 0;

    private void Awake()
    {
        if (Instance != null)
        {
            DestroyImmediate(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    public void RocketPowerUp()
    {
        StartCoroutine(powerUpTimer(rocketPowerUpElement, "rocket", rocketPowerupFillImage, 3f));

    }
    public void ShieldPowerUp()
    {
        StartCoroutine(powerUpTimer(shieldPowerUpElement, "shield", shieldPowerupFillImage, 6f));
    }



    IEnumerator powerUpTimer(GameObject powerup, string power, Image powerupFillImage, float duration)
    {
        powerup.SetActive(true);

        if (power == "rocket")
        {

            RocketPowerUpSpeedEffect.SetActive(true);
            ImmuneRocketPlayer = true;
            mainPlayer.GetComponent<BoxCollider2D>().enabled = true;
        }
        if (power == "shield")
        {
            ImmuneShieldPlayer = true;

        }

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
        if (audioToggle.isOn)
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
        if (!shot)
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
        gameStart = false;
        yield return new WaitForSeconds(instuctionInfowaitTime);
        instuctionInfo.SetActive(false);
        mainPlayer.SetActive(true);
        gameStart = true;
        countdownText.enabled = false;
    }
    private void OnDestroy()
    {
        if (Instance == this)
        {
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
        LifeBuff(isLifeBuffToggle);
        // FivexBuffOnWork(is5xBuffToggle);
        // TwoxBuffOnWork(is2xBuffToggle);

        OnChangeCharacterScroll();
        ResetBuff();
        StartCoroutine(spawner.intialDelay(60));
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
        Select_Character_Map_Panel.SetActive(false);
        startPanel.SetActive(false);
        gameoverPanel.SetActive(false);
        gameOver.SetActive(false);
        StartCoroutine(Instructiontimer());


        player.enabled = true;

        Pipes[] pipes = FindObjectsOfType<Pipes>();

        for (int i = 0; i < pipes.Length; i++)
        {
            Destroy(pipes[i].gameObject);
        }
    }

    public void OnChangeCharacterScroll()
    {

        if (characterScroll.contentValue == 1)
        {
            charactersAlive[0].SetActive(true);
            charactersDeath[0].SetActive(true);
            instructionCharacter[0].SetActive(true);
            charactersAlive[1].SetActive(false);
            charactersDeath[1].SetActive(false);
            instructionCharacter[1].SetActive(false);
            charactersAlive[2].SetActive(false);
            charactersDeath[2].SetActive(false);
            instructionCharacter[2].SetActive(false);
            charactersAlive[3].SetActive(false);
            charactersDeath[3].SetActive(false);
            instructionCharacter[3].SetActive(false);
            characterRenderSprite.sprite = charcterSprite[0];
        }
        else if (characterScroll.contentValue == 2)
        {
            charactersAlive[0].SetActive(false);
            charactersDeath[0].SetActive(false);
            instructionCharacter[0].SetActive(false);
            charactersAlive[1].SetActive(true);
            charactersDeath[1].SetActive(true);
            instructionCharacter[1].SetActive(true);
            charactersAlive[2].SetActive(false);
            charactersDeath[2].SetActive(false);
            instructionCharacter[2].SetActive(false);
            charactersAlive[3].SetActive(false);
            charactersDeath[3].SetActive(false);
            instructionCharacter[3].SetActive(false);
            characterRenderSprite.sprite = charcterSprite[1];

        }
        else if (characterScroll.contentValue == 3)
        {
            charactersAlive[0].SetActive(false);
            charactersDeath[0].SetActive(false);
            instructionCharacter[0].SetActive(false);
            charactersAlive[1].SetActive(false);
            charactersDeath[1].SetActive(false);
            instructionCharacter[1].SetActive(false);
            charactersAlive[2].SetActive(true);
            charactersDeath[2].SetActive(true);
            instructionCharacter[2].SetActive(true);
            charactersAlive[3].SetActive(false);
            charactersDeath[3].SetActive(false);
            instructionCharacter[3].SetActive(false);
            characterRenderSprite.sprite = charcterSprite[2];
        }
        else if (characterScroll.contentValue == 4)
        {
            charactersAlive[0].SetActive(false);
            charactersDeath[0].SetActive(false);
            instructionCharacter[0].SetActive(false);
            charactersAlive[1].SetActive(false);
            charactersDeath[1].SetActive(false);
            instructionCharacter[1].SetActive(false);
            charactersAlive[2].SetActive(false);
            charactersDeath[2].SetActive(false);
            instructionCharacter[2].SetActive(false);
            charactersAlive[3].SetActive(true);
            charactersDeath[3].SetActive(true);
            instructionCharacter[3].SetActive(true);
            characterRenderSprite.sprite = charcterSprite[3];
        }

        switch (enviromentScroll.contentValue)
        {
            case 1:
                maps[0].SetActive(true);
                maps[1].SetActive(false);
                maps[2].SetActive(false);
                maps[3].SetActive(false);
                maps[4].SetActive(false);
                maps[5].SetActive(false);
                maps[6].SetActive(false);
                spawner.chooseTheRightMap(enviromentScroll.contentValue);

                break;
            case 2:
                maps[0].SetActive(false);
                maps[1].SetActive(true);
                maps[2].SetActive(false);
                maps[3].SetActive(false);
                maps[4].SetActive(false);
                maps[5].SetActive(false);
                maps[6].SetActive(false);
                spawner.chooseTheRightMap(enviromentScroll.contentValue);

                break;
            case 3:
                maps[0].SetActive(false);
                maps[1].SetActive(false);
                maps[2].SetActive(true);
                maps[3].SetActive(false);
                maps[4].SetActive(false);
                maps[5].SetActive(false);
                maps[6].SetActive(false);
                spawner.chooseTheRightMap(enviromentScroll.contentValue);

                break;
            case 4:
                maps[0].SetActive(false);
                maps[1].SetActive(false);
                maps[2].SetActive(false);
                maps[3].SetActive(true);
                maps[4].SetActive(false);
                maps[5].SetActive(false);
                maps[6].SetActive(false);
                spawner.chooseTheRightMap(enviromentScroll.contentValue);

                break;
            case 5:
                maps[0].SetActive(false);
                maps[1].SetActive(false);
                maps[2].SetActive(false);
                maps[3].SetActive(false);
                maps[4].SetActive(true);
                maps[5].SetActive(false);
                maps[6].SetActive(false);
                spawner.chooseTheRightMap(enviromentScroll.contentValue);

                break;
            case 6:
                maps[0].SetActive(false);
                maps[1].SetActive(false);
                maps[2].SetActive(false);
                maps[3].SetActive(false);
                maps[4].SetActive(false);
                maps[5].SetActive(true);
                maps[6].SetActive(false);
                spawner.chooseTheRightMap(enviromentScroll.contentValue);

                break;
            case 7:
                maps[0].SetActive(false);
                maps[1].SetActive(false);
                maps[2].SetActive(false);
                maps[3].SetActive(false);
                maps[4].SetActive(false);
                maps[5].SetActive(false);
                maps[6].SetActive(true);
                spawner.chooseTheRightMap(enviromentScroll.contentValue);

                break;
            default:
                maps[0].SetActive(true);
                maps[1].SetActive(false);
                maps[2].SetActive(false);
                maps[3].SetActive(false);
                maps[4].SetActive(false);
                maps[5].SetActive(false);
                maps[6].SetActive(false);
                spawner.chooseTheRightMap(enviromentScroll.contentValue);

                break;
        }
    }

    public void onButtonChangeForEnviroment()
    {
        if (enviromentScroll.contentValue == 1)
        {
            leftmove.sprite = inactiveleftSprite;
            rightmove.sprite = activerightSprite;
        }
        else if (enviromentScroll.contentValue == 7)
        {
            leftmove.sprite = activeleftSprite;
            rightmove.sprite = inactiverightSprite;
        }
        else
        {
            leftmove.sprite = activeleftSprite;
            rightmove.sprite = activerightSprite;
        }
    }

    public void Home()
    {
        gaemPanel.SetActive(false);
        gameStart = false;
        gameoverPanel.SetActive(false);
        startPanel.SetActive(true);
        pausePanel.SetActive(false);
        Pause();
    }
    public void GameOver()
    {
        if (!ImmuneShieldPlayer && !ImmuneRocketPlayer)
        {

            if (!isLifeBuff)
            {
                StopAllCoroutines();
                // StartCoroutine(SmoothScoreIncrement(score,2f,FinalscoreText));
                // StartCoroutine(SmoothScoreIncrement(coin, 2f,FinalcoinText));
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
                requestHandler.SendScore(score, coin, enviromentScroll.contentValue, characterScroll.contentValue);
                var u = TelegramConnect.GetUserData();
                if (u != null)
                {
                    userData = JsonUtility.FromJson<UserData>(u);
                    UserNameFinal.text = userData.user.first_name;
                }


            }
            else
            {

                StartCoroutine(Instructiontimer());
                StartCoroutine(Delay(1f, ExtraLifeEffect));

            }

        }

    }

    IEnumerator Delay(float time, GameObject gameObject)
    {
        gameObject.SetActive(true);
        // StartCoroutine(player.HoldStill());
        yield return new WaitForSeconds(time);
        gameObject.SetActive(false);
        isLifeBuff = false;
    }


    public void SwitchProfileOnAndOffPanel(bool value)
    {

        if (value)
        {
            profilePanel.SetActive(true);
        }
        else
        {
            profilePanel.SetActive(false);
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

    public void PanelLeaderboardOff(GameObject panel)
    {
        panel.SetActive(false);
    }


    public void TwoxBuffOnWork(Toggle toggle)
    {
        if (toggle.isOn)
        {
            spawner.probabilityForIndex6 = 0.2f;
            buffsToggle[1].isOn = true;
        }
        else
        {
            spawner.probabilityForIndex6 = 0.1f;
            buffsToggle[1].isOn = false;
        }
    }
    public void FivexBuffOnWork(Toggle toggle)
    {
        if (toggle.isOn)
        {
            spawner.probabilityForIndex6 = 0.5f;
            buffsToggle[0].isOn = true;
        }
        else
        {
            buffsToggle[0].isOn = false;
            spawner.probabilityForIndex6 = 0.1f;
        }
    }

    public void LifeBuff(Toggle toggle)
    {
        if (toggle.isOn)
        {
            // isLifeCount = 1;
            isLifeBuff = true;

            buffsToggle[2].isOn = true;
        }
        else
        {
            isLifeBuff = false;
            // isLifeCount = 0;
            buffsToggle[2].isOn = false;
        }
    }
    public void ResetBuff()
    {
        // spawner.probabilityForIndex6    =   0.1f;
        // buffsToggle[0].isOn = false;
        // buffsToggle[1].isOn = false;
    }
   
    
    public void ActivePage(GameObject panel)
    {
        panel.SetActive(true);
        startPanel.SetActive(false); 
        Select_Character_Map_Panel.SetActive(false); 
        gaemPanel.SetActive(false); 
        pausePanel.SetActive(false); 
        gameoverPanel.SetActive(false); 
        gameOver.SetActive(false); 
        instuctionInfo.SetActive(false); 
        profilePanel.SetActive(false); 
    }
}
