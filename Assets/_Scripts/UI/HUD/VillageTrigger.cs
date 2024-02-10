using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VillageTrigger : MonoBehaviour
{
    [SerializeField] private Scenes m_sceneToLoad;
    private bool m_canLoadScene = false;

    [Header("Text Movement")]
    [SerializeField] private TextMeshProUGUI m_sceneText;
    [SerializeField] private float m_textMoveTime;
    [SerializeField] private float m_textMoveYPosUp;
    private float m_textYPosition;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        m_canLoadScene = true;

        m_textYPosition = m_sceneText.transform.position.y;
        MoveTextUp();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        m_canLoadScene = false;
    }

    private void MoveTextUp()
    {
        LeanTween.moveLocalY(m_sceneText.gameObject, m_textYPosition + m_textMoveYPosUp, m_textMoveTime).setEaseOutCirc().setOnComplete(MoveTextDown);
    }

    private void MoveTextDown()
    {
        if (m_canLoadScene)
            LeanTween.moveLocalY(m_sceneText.gameObject, m_textYPosition, m_textMoveTime).setEaseInCirc().setOnComplete(MoveTextUp);
        else
            LeanTween.moveLocalY(m_sceneText.gameObject, m_textYPosition, m_textMoveTime).setEaseInCirc();
    }

    private void Update()
    {
        if (m_canLoadScene)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                // cannot open pausemenu if optionsmenu or pause are open or the scene is already loaded
                if (SceneManager.GetSceneByBuildIndex((int)m_sceneToLoad).isLoaded) UnloadScene();
                else if (SceneManager.GetSceneByBuildIndex((int)Scenes.Pause).isLoaded) return;
                else if (SceneManager.GetSceneByBuildIndex((int)Scenes.Options).isLoaded) return;
                else LoadScene();
            }
        }
    }


    /// <summary>
    /// Load the Scene (Dungeon or an additive scene to the village)
    /// </summary>
    private void LoadScene()
    {
        if (m_sceneToLoad == Scenes.Dungeon)     // Load Dungeon
            MenuManager.Instance.LoadDungeon();
        else                                    // Load Credits, Shop or Scoreboard
        {
            Time.timeScale = 0f;
            MenuManager.Instance.LoadSceneAsync(m_sceneToLoad, LoadSceneMode.Additive);
        }
    }

    /// <summary>
    /// Unload Scene
    /// </summary>
    private void UnloadScene()
    {
        //can only be called when village is still open in the background, so we dont need to check if we are in the dungeon

        MenuManager.Instance.UnloadSceneAsync(m_sceneToLoad);
        Time.timeScale = 1f;
    }
}
