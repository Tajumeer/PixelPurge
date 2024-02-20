using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EndDungeonManager : MonoBehaviour
{

    [Header("Score Points")]
    [SerializeField] private TextMeshProUGUI m_userScoreText;
    [SerializeField] private TextMeshProUGUI m_goldAmountText;

    private void OnEnable()
    {
        if(m_goldAmountText != null) m_goldAmountText.text = GameManager.Instance.Gold.ToString();
        if(m_userScoreText != null) m_userScoreText.text = GameManager.Instance.UserScore.ToString();
    }

    public void ReloadDungeon()
    {
        MenuManager.Instance.UnloadDungeon(false);
        MenuManager.Instance.LoadDungeon();
        Time.timeScale = 1f;
    }

    public void LoadVillage()
    {
        MenuManager.Instance.UnloadDungeon();
        Time.timeScale = 1f;
    }
}
