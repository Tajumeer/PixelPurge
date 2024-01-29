using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Maya

public class OptionsManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> m_tabs;
    [SerializeField] private List<Button> m_tabButtons;

    private int m_activeTab;

    private void OnEnable()
    {
        m_activeTab = 0;
        ActivateTab(0);
        DeactivateTab(1);
        DeactivateTab(2);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            UnloadOptions();
    }

    public void ActivateTab(int _tabNumber)
    {
        DeactivateTab(m_activeTab);             // deactivate last active tab
        m_activeTab = _tabNumber;

        m_tabs[_tabNumber].SetActive(true);     // show new tab
        m_tabButtons[_tabNumber].Select();      // selct tab button
    }

    private void DeactivateTab(int _tabNumber)
    {
        m_tabs[_tabNumber].SetActive(false);
    }

    public void UnloadOptions()
    {
        MenuManager.Instance.UnloadSceneAsync(Scenes.Options);
    }
}
