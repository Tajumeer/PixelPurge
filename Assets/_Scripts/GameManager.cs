using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour, IDataPersistence
{
    private static GameManager m_instance;

    public static GameManager Instance
    {
        get
        {
            if (m_instance == null)
            {
                GameObject obj = new GameObject("GameManager");
                m_instance = obj.AddComponent<GameManager>();
            }

            return m_instance;
        }
    }

    public string UserName;
    [HideInInspector] public int UserScore;
    [HideInInspector] public int HighestScore;
    public int Gold;
    private FirestoreRest m_firestore;


    private void Awake()
    {
        if (m_instance == null)
        {
            m_instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }


    private void ResetGame()
    {

    }

    private void Start()
    {
        m_firestore = GetComponent<FirestoreRest>();
    }
    public void AddScore(int _score)
    {
        UserScore += _score;
        Debug.Log("Current UserScore = " + UserScore);
    }

    public void AddGold(int _gold)
    {
        Gold += _gold;
        Debug.Log("Current Gold = " + Gold);
    }

    public void Win()
    {
        Time.timeScale = 0f;
        MenuManager.Instance.LoadSceneAsync(Scenes.Win, UnityEngine.SceneManagement.LoadSceneMode.Additive);
    }

    public void Lose()
    {
        Time.timeScale = 0f;
        MenuManager.Instance.LoadSceneAsync(Scenes.Death, UnityEngine.SceneManagement.LoadSceneMode.Additive);
    }

    private void UpdateLeaderboard()
    {
        if (UserScore < HighestScore)
        {
            UserScore = HighestScore;
            m_firestore.SaveUserData("Leaderboard", UserName, UserScore);
        }
    }

    public void EndGameRound()
    {
        UpdateLeaderboard();
        UserScore = 0;
    }

    public void LoadData(GameData _data)
    {
        this.HighestScore = _data.HighScore;
        this.UserName = _data.UserName;
        this.Gold = _data.Gold;
    }

    public void SaveData(ref GameData _data)
    {
        _data.HighScore = this.HighestScore;
        _data.UserName = this.UserName;
        _data.Gold = this.Gold;
    }
}
