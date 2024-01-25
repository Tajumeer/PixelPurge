using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Maya

public class OptionsManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> m_tabs;

    private int m_activeTab;

    private void OnEnable()
    {
        m_activeTab = 0;
        ActivateTab(0);
        DeactivateTab(1);
        DeactivateTab(2);
    }

    public void ActivateTab(int _tabNumber)
    {
        DeactivateTab(m_activeTab);
        m_activeTab = _tabNumber;
        m_tabs[_tabNumber].SetActive(true);
    }

    private void DeactivateTab(int _tabNumber)
    {
        m_tabs[_tabNumber].SetActive(false);
    }

    public void UnloadOptions()
    {
        MenuManager.Instance.UnloadSceneAsync(Scenes.Options);
        FindObjectOfType<DungeonHUD>().UnloadOptions();
    }
}
