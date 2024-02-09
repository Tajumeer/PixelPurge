using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class UiData : MonoBehaviour, IDataPersistence
{
    private static UiData m_instance;

    public static UiData Instance
    {
        get
        {
            if (m_instance == null)
            {
                GameObject obj = new GameObject("UiData");
                m_instance = obj.AddComponent<UiData>();
            }

            return m_instance;
        }
    }

    [HideInInspector] public int ResolutionValue;
    [HideInInspector] public bool FullscreenToggle;
    [HideInInspector] public int FullscreenModeValue;
    [HideInInspector] public FullScreenMode Fullscreen;
    [HideInInspector] public bool LoadOptionsOnce;

    private void Awake()
    {
        if (m_instance == null)
        {
            m_instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }


    }

    private void Update()
    {
        Debug.Log("FS Toggle = " + FullscreenToggle);
        Debug.LogWarning(LoadOptionsOnce);
    }

    public void LoadData(GameData _data)
    {
        ResolutionValue = _data.ResolutionValue;
        FullscreenModeValue = _data.FullScreenMode;
        GetFullScreenMode();
    }

    private int FullscreenModeData(FullScreenMode _mode)
    {
        return FullscreenModeValue = (int)_mode;
    }

    private FullScreenMode GetFullScreenMode()
    {
        return (FullScreenMode)FullscreenModeValue;
    }

    public void SaveData(ref GameData _data)
    {
        _data.ResolutionValue = UiData.Instance.ResolutionValue;
        FullscreenModeValue = this.FullscreenModeData(Fullscreen);
        _data.FullScreenMode = FullscreenModeValue;

        Debug.Log("Saved Resolution value: " + _data.ResolutionValue);
    }
}


