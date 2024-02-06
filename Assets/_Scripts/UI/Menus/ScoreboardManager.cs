using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

// Maya

public class ScoreboardManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI playerRank;
    [SerializeField] TextMeshProUGUI playerName;
    [SerializeField] TextMeshProUGUI playerScore;
    [Space]
    [SerializeField] TextMeshProUGUI[] allPlayerNames;
    [SerializeField] TextMeshProUGUI[] allPlayerScores;

    public void UnloadScene()
    {
        MenuManager.Instance.UnloadSceneAsync(Scenes.Scoreboard);
    }
}
