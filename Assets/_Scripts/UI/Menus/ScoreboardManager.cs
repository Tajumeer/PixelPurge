using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static FirestoreRest;

// Maya

public class ScoreboardManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI m_playerRank;
    [SerializeField] private TextMeshProUGUI m_playerName;
    [SerializeField] private TextMeshProUGUI m_playerScore;
    [Space]
    [SerializeField] private TextMeshProUGUI[] m_allPlayerNames;
    [SerializeField] private TextMeshProUGUI[] m_allPlayerScores;

    private List<Document> m_leaderboard;

    public void UnloadScene()
    {
        MenuManager.Instance.UnloadSceneAsync(Scenes.Scoreboard, CursorTypes.None);
        Time.timeScale = 1f;
    }

    private void OnEnable()
    {
        m_leaderboard = GameManager.Instance.gameObject.GetComponent<FirestoreRest>().Leaderboard;

        // show name and score (of the first few players) in the list
        for (int i = 0; i < m_allPlayerNames.Length; i++)
        {
            m_allPlayerNames[i].text = m_leaderboard[i].Fields.User.stringValue;
            m_allPlayerScores[i].text = m_leaderboard[i].Fields.Score.integerValue.ToString();
        }

        int userScore = GameManager.Instance.UserScore;
        string userName = GameManager.Instance.UserName;
        bool inFirst = false;

        // go through the first 100 players in the scorelist
        for (int i = 0; i < m_leaderboard.Count; i++)
        {
            // until someone has a lower score then the player
            if (m_leaderboard[i].Fields.Score.integerValue < userScore)
            {
                // check if the one before is really the player(name)
                if ((i > 0 && m_leaderboard[i - 1].Fields.User.stringValue == userName) ||
                         (i == 0 && m_leaderboard[i].Fields.User.stringValue == userName))
                {
                    // show the rank of the player
                    m_playerRank.text = i.ToString();
                    inFirst = true;
                }
            }
        }

        if (!inFirst) m_playerRank.text = ">100";
        m_playerName.text = userName;
        m_playerScore.text = userScore.ToString();
    }
}
