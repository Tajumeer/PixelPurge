using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
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

    [HideInInspector] public string UserName;
    [HideInInspector] public int UserScore;

    private FirestoreRest m_firestore;

    private void Start()
    {
        m_firestore = GetComponent<FirestoreRest>();
    }
    public void AddScore(int _score)
    {
        UserScore += _score;
        Debug.Log("Current UserScore = " + UserScore);
    }

    private void UpdateLeaderboard()
    {
        m_firestore.SaveUserData("Leaderboard", UserName, UserScore);
    }

    public void EndGameRound()
    {
        UpdateLeaderboard();
        UserScore = 0;
    }
}
