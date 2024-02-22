using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Maya

public class LoadingScreen : MonoBehaviour
{
    [SerializeField] private Scenes m_sceneToLoad;

    [Header("Timings")]
    [SerializeField] private float loadDelay = 1f;
    [SerializeField] private float minLoadingTime;

    [Header("Bar and Image")]
    [SerializeField] private Image loadingBar;
    [Space]
    [SerializeField] private Transform enemy;
    [Tooltip("Where the Enemy starts moving")]
    [SerializeField] private Transform barLeft;
    [Tooltip("Where the Enemy ends moving")]
    [SerializeField] private Transform barRight;

    private void OnEnable()
    {
        StartCoroutine(StartLoadTheScene());
    }

    public IEnumerator StartLoadTheScene()
    {
        loadingBar.fillAmount = 0f;
        enemy.position = new Vector2(barLeft.position.x, enemy.position.y);

        yield return new WaitForSeconds(loadDelay);

        AsyncOperation loadOperation = SceneManager.LoadSceneAsync((int)m_sceneToLoad);  // load scene 

        StartCoroutine(LoadingBar(loadOperation));
    }

    private IEnumerator LoadingBar(AsyncOperation loadOperation)
    {
        float enemyPosition = 0f;

        loadOperation.allowSceneActivation = false;     // Scene is not allowed to load (fake loadingBar)

        var fakeBarTimer = 0f;     // for fake bar

        while (loadOperation.progress < 0.9f || fakeBarTimer <= minLoadingTime)
        {
            yield return null;
            fakeBarTimer += Time.unscaledDeltaTime;

            var waitProgress = fakeBarTimer / minLoadingTime;       // fake bar
            var loadingProgress = loadOperation.progress / 0.9f;    // real loading bar
            loadingBar.fillAmount = Mathf.Min(loadingProgress, waitProgress); // show the bar that is slower

            // enemy move with bar progress
            enemyPosition = Mathf.Lerp(barLeft.position.x, barRight.position.x, loadingBar.fillAmount);
            enemy.position = new Vector2(enemyPosition, enemy.position.y);
        }

        loadOperation.allowSceneActivation = true;      // load Scene

        // add HUD scene
        if (m_sceneToLoad == Scenes.Dungeon)
            MenuManager.Instance.LoadSceneAsync(Scenes.DungeonHUD, LoadSceneMode.Additive);

        yield return new WaitUntil(() => loadOperation.isDone);
        loadingBar.fillAmount = 1f;

    }
}
