using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using static RequestHandler;


public class RequestHandler : MonoBehaviour
{

    private UserData userData = null;
    
    private const string baseUrl = "https://pav-backend.onrender.com/";

    [Header("Profile UI")]
    public Text username;
    public Text displayname;
    public Text coinsCount;
    public Text coinsCountScreenSelectionPart;
    public Text personalBest;
    public Image AvatarSprite;
    public Image AvatarDeadScreenSprite;
    public String coinValue;

    private ValidateData validateData = null;
    [Serializable]
    public class ValidateData
    {
        public string token;
        public bool premium;
        public int premiumEndDate;
        public int userType;
        public int points;
        public int coins;
        public bool sbt;
    }
    [Serializable]
    public class ResultWrapper
    {
        public List<Result> results;
    }
    [Serializable]
    public class Result
    {
        public int position;
        public Player user;
        public int score;
    }
    [Serializable]
    public class User
    {
        public long id;
        public string first_name;
        public string last_name;
        public string language_code;
        public bool allows_write_to_pm;
    }
    [Serializable]
    public class UserData
    {
        public User user;
        public string chat_instance;
        public string chat_type;
        public string auth_date;
        public string hash;
    }
    [Serializable]
    public class PlayerScore
    {
        public string username;
        public string firstName;
        public int score;
        public string url;
    }
    [Serializable]
    public class ResultWrapper10
    {
        public List<PlayerScore> results;
    }
    [Header("Leaderboard 10")]
    public GameObject leaderBoardUI;
    public Transform playerScoreParent;

    [Serializable]
    public class PlayerScoreDisplay
    {
        public Text username;
        public Text score;
    }
    [Serializable]
    public class ResultProfile
    {
        public int userId;
        public string first_name;
        public string language_code;
        public string last_name;
        public string username;
        public int personalBest;
        public int coins;
        public int played;
        public int globalRank;
        public string base64;
    }
    
    [Header("Profile Page")]
    public Text profilename;
    void Start()
    {
        try
        {

            TelegramConnect.Validate(baseUrl);
            var u = TelegramConnect.GetUserData();
            userData = JsonUtility.FromJson<UserData>(u);
            GetProfile(true);
            Debug.Log("Sucessful");
     

        }
        catch (Exception e)
        { Debug.Log("Error: " + e.Message); }
    }
    public void GetProfile(bool isAvatar)
    {
        if (userData?.user?.id == null)
            return;

        string url = $"{baseUrl}pofile/{userData.user.id}";
        StartCoroutine(SendGetRequestProfile(url, isAvatar));
    }
    private IEnumerator SendGetRequestProfile(string url, bool isAvatar)
    {
        UnityWebRequest request = UnityWebRequest.Get(url);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError("Error: " + request.error);
        }
        else
        {
            string jsonText = request.downloadHandler.text;
            ResultProfile resultWrapper = JsonUtility.FromJson<ResultProfile>(jsonText);

            Debug.Log("Response: " + jsonText);

            displayname.text = resultWrapper.first_name;
            coinsCount.text = resultWrapper.coins.ToString("#,##0");
            coinValue = resultWrapper.coins.ToString("#,##0");
            coinsCountScreenSelectionPart.text = resultWrapper.coins.ToString("#,##0");
            if (isAvatar)
            {
                url = $"{baseUrl}pofile/image/{userData.user.id}";
                StartCoroutine(SendGetRequestProfileImage(url));
            }
        }
    }
    private IEnumerator SendGetRequestProfileImage(string url)
    {
        UnityWebRequest request = UnityWebRequest.Get(url);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError("Error: " + request.error);
        }
        else
        {
            string jsonText = request.downloadHandler.text;
            ResultProfile resultWrapper = JsonUtility.FromJson<ResultProfile>(jsonText);

            Debug.Log("Response: " + resultWrapper);

            AvatarSprite.sprite = Base64ToSprite(resultWrapper.base64);
            AvatarDeadScreenSprite.sprite = Base64ToSprite(resultWrapper.base64);
        }
    }
    private Sprite Base64ToSprite(string base64)
    {
        byte[] bytes = Convert.FromBase64String(base64);
        Texture2D texture = new(1, 1);
        texture.LoadImage(bytes);
        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        return sprite;
    }
    public void SendScore(int score, int coins,int map,int character)
    {
        if (userData?.hash == null)
            return;

        string url = $"{baseUrl}highscore/{score}?hash={userData.hash}&coins={coins}&map={map}&character={character}";
        StartCoroutine(SendGetRequest(url));
   }
    private IEnumerator SendGetRequest(string url)
    {
        UnityWebRequest request = UnityWebRequest.Get(url);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError("Error: " + request.error);
        }
        else
        {
            Debug.Log("Response: " + request.downloadHandler.text);

            string jsonText = request.downloadHandler.text;
            ResultWrapper resultWrapper = JsonUtility.FromJson<ResultWrapper>("{\"results\":" + jsonText + "}");

            
        }
    }

    public void GetLeaderboard()
    {
        StartCoroutine(LeaderboardGetRequest());
    }
    private IEnumerator LeaderboardGetRequest()
    {
        string url = baseUrl + "reports/leaderboard-new";
        UnityWebRequest request = UnityWebRequest.Get(url);
        yield return request.SendWebRequest();
        leaderBoardUI.SetActive(true);
        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError("Error: " + request.error);
        }
        else
        {
            

            string jsonText = request.downloadHandler.text;
            ResultWrapper10 resultWrapper = JsonUtility.FromJson<ResultWrapper10>("{\"results\":" + jsonText + "}");

            for (int i = 0; i < resultWrapper.results.Count; i++)
            {
             
                Transform t = playerScoreParent.GetChild(i);
                
                t.GetChild(0).GetChild(2).GetComponent<Text>().text = resultWrapper.results[i].firstName;
                t.GetChild(0).GetChild(4).GetChild(0).GetComponent<Text>().text = resultWrapper.results[i].score.ToString("#,##0");
            }
        }
    }
}
