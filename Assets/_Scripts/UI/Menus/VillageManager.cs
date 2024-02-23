using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

// Maya

public class VillageManager : MonoBehaviour
{
    [Header("Texts")]
    [SerializeField] private TextMeshProUGUI m_coinsText;
    [SerializeField] private TextMeshProUGUI m_scoreText;

    [Header("Stats")]
    [SerializeField] private GameObject m_statWindow;
    [SerializeField] private TextMeshProUGUI m_statTitle;
    [SerializeField] private TextMeshProUGUI m_statAmount_maxHealth;
    [SerializeField] private TextMeshProUGUI m_statAmount_healthRegeneration;
    [SerializeField] private TextMeshProUGUI m_statAmount_damageReduction;
    [SerializeField] private TextMeshProUGUI m_statAmount_movementSpeed;
    [SerializeField] private TextMeshProUGUI m_statAmount_collectionRadius;
    [SerializeField] private TextMeshProUGUI m_statAmount_goldMultiplier;
    [SerializeField] private TextMeshProUGUI m_statAmount_damage;
    [SerializeField] private TextMeshProUGUI m_statAmount_critChance;
    [SerializeField] private TextMeshProUGUI m_statAmount_cdReduction;
    [SerializeField] private TextMeshProUGUI m_statAmount_projectileSpeed;

    private PlayerController m_playerScript;

    private void OnEnable()
    {
        ShowCoins();
        ShowScore();

        m_statTitle.text = "Archer";
        HideStats(Characters.Archer);
    }

    private void Update()
    {
        // Open/Close Pausemenu
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // open pausemenu or close credits, scoreboard, shop
            if (SceneManager.GetSceneByBuildIndex((int)Scenes.Options).isLoaded) return;
            else if (SceneManager.GetSceneByBuildIndex((int)Scenes.Pause).isLoaded) UnloadScene(Scenes.Pause);
            else if (SceneManager.GetSceneByBuildIndex((int)Scenes.Credits).isLoaded) UnloadScene(Scenes.Credits);
            else if (SceneManager.GetSceneByBuildIndex((int)Scenes.Scoreboard).isLoaded) UnloadScene(Scenes.Scoreboard);
            else if (SceneManager.GetSceneByBuildIndex((int)Scenes.Shop).isLoaded) UnloadScene(Scenes.Shop);
            else LoadPause();
        }
    }

    private void LoadPause()
    {
        Time.timeScale = 0f;
        MenuManager.Instance.LoadSceneAsync(Scenes.Pause, LoadSceneMode.Additive);
    }

    public void UnloadScene(Scenes _scene)
    {
        MenuManager.Instance.UnloadSceneAsync(_scene);
        Time.timeScale = 1f;
    }

    #region HUD

    public void ShowCoins()
    {
        m_coinsText.text = GameManager.Instance.Gold.ToString();
    }

    private void ShowScore()
    {
        m_scoreText.text = GameManager.Instance.UserScore.ToString();
    }

    public void ShowStats(Characters _character)
    {
        switch (_character)
        {
            case Characters.Archer:
                m_statTitle.text = "Archer";
                break;

            case Characters.Swordsman:
                m_statTitle.text = "Swordsman";
                break;

            case Characters.Mage:
                m_statTitle.text = "Mage";
                break;
        }

        // Find PlayerController
        if (m_playerScript == null)
        {
            if (FindObjectOfType<PlayerController>()) m_playerScript = FindObjectOfType<PlayerController>();
            else
            {
                Debug.LogWarning("No PlayerController Script found");
                return;
            }
        }

        PlayerStats statSO = m_playerScript.PlayerData[(int)_character];

        m_statAmount_maxHealth.text = statSO.MaxHealth.ToString();
        m_statAmount_healthRegeneration.text = statSO.HealthRegeneration.ToString();
        m_statAmount_damageReduction.text = statSO.DamageReductionPercentage.ToString();
        m_statAmount_movementSpeed.text = statSO.MovementSpeed.ToString();
        m_statAmount_collectionRadius.text = statSO.CollectionRadius.ToString();
        m_statAmount_goldMultiplier.text = statSO.GoldMultiplier.ToString() + "%";
        m_statAmount_damage.text = statSO.DamageMultiplier.ToString() + "%";
        m_statAmount_critChance.text = statSO.CritChance.ToString() + "%";
        m_statAmount_cdReduction.text = statSO.CdReduction.ToString() + "%";
        m_statAmount_projectileSpeed.text = statSO.ProjectileSpeed.ToString();

        m_statWindow.SetActive(true);
    }

    public void HideStats(Characters _character)
    {
        // only hide it when still this´characters stats are open and not yet another
        switch (_character)
        {
            case Characters.Archer:
                if (m_statTitle.text == "Archer") m_statWindow.SetActive(false);
                break;

            case Characters.Swordsman:
                if (m_statTitle.text == "Swordsman") m_statWindow.SetActive(false);
                break;

            case Characters.Mage:
                if (m_statTitle.text == "Mage") m_statWindow.SetActive(false);
                break;
        }
    }

    #endregion
}
