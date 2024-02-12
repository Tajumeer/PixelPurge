using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Maya

public class LoadingScreen : MonoBehaviour
{
    [SerializeField] private Scenes m_sceneToLoad;

    [Header("Loading Screen")]
    [SerializeField] private float loadDelay = 1f;
    [SerializeField] private float minLoadingTime;
    [SerializeField] private Image loadingBar;
    [SerializeField] private Transform barLeft;
    [SerializeField] private Transform barRight;
    [SerializeField] private Transform enemy;

    private void OnEnable()
    {
        StartCoroutine(StartLoadTheScene());
    }

    public IEnumerator StartLoadTheScene()
    {
        yield return new WaitForSeconds(loadDelay);

        AsyncOperation loadOperation = SceneManager.LoadSceneAsync((int)m_sceneToLoad);  // load scene 

        StartCoroutine(LoadingBar(loadOperation));
    }

    private IEnumerator LoadingBar(AsyncOperation loadOperation)
    {
        float playerPosition = 0f;

        loadOperation.allowSceneActivation = false;     // Scene is not allowed to load (fake loadingBar)

        buttons.SetActive(false);
        newGameQuestion.SetActive(false);
        loading.SetActive(true);

        loadingBar.fillAmount = 0f;
        var counter = 0f;     // for fake bar

        while (loadOperation.progress < 0.9f || counter <= minLoadingTime)
        {
            yield return null;
            counter += Time.unscaledDeltaTime;

            var waitProgress = counter / minLoadingTime;
            var loadingProgress = loadOperation.progress / 0.9f;
            loadingBar.fillAmount = Mathf.Min(loadingProgress, waitProgress); // loading bar from 0 to 1

            // head move with bar progress
            playerPosition = Mathf.Lerp(barLeft.position.x, barRight.position.x, loadingBar.fillAmount);
            enemy.position = new Vector2(playerPosition, enemy.position.y);
        }

        loadOperation.allowSceneActivation = true;      // load Scene

        yield return new WaitUntil(() => loadOperation.isDone);
        loadingBar.fillAmount = 1f;
        loading.SetActive(false);
        buttons.SetActive(true);
    }

    private void LoadTheScene()
    {
        if (m_sceneToLoad == Scenes.Dungeon)
        {
            MenuManager.Instance.LoadSceneAsync(Scenes.Beta);
            MenuManager.Instance.LoadSceneAsync(Scenes.DungeonHUD, LoadSceneMode.Additive);
        }
        else
            MenuManager.Instance.LoadSceneAsync(m_sceneToLoad);
    }
}
