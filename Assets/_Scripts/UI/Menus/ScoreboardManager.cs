using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

// Maya

public class ScoreboardManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI m_playerRank;
    [SerializeField] TextMeshProUGUI m_playerName;
    [SerializeField] TextMeshProUGUI m_playerScore;
    [Space]
    [SerializeField] TextMeshProUGUI[] m_allPlayerNames;
    [SerializeField] TextMeshProUGUI[] m_allPlayerScores;

    public void UnloadScene()
    {
        MenuManager.Instance.UnloadSceneAsync(Scenes.Scoreboard);
    }
}
