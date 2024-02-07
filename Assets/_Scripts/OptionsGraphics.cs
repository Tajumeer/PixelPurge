using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class OptionsGraphics : MonoBehaviour, IDataPersistence
{
    [SerializeField] private TMP_Dropdown m_resolution;
    [SerializeField] private Toggle m_fullscreen;

    private int m_value;
    private FullScreenMode m_fullscreenMode;
    private int m_fullscreenValue;
    private bool m_toggled;




    public void ChangeResolution()
    {
        m_value = m_resolution.value;
        switch (m_value)
        {
            case 0:
                Screen.SetResolution(800, 600, GetScreenMode());
                break;
            case 1:
                Screen.SetResolution(1280, 800, GetScreenMode());
                break;
            case 2:
                Screen.SetResolution(1600, 900, GetScreenMode());
                break;
            case 3:
                Screen.SetResolution(1920, 1080, GetScreenMode());
                break;
            case 4:
                Screen.SetResolution(2560, 1440, GetScreenMode());
                break;
            case 5:
                Screen.SetResolution(3440, 1440, GetScreenMode());
                break;
            default:
                break;
        }

    }

    private void LoadSettings()
    {
        m_resolution.value = m_value;
     
        m_resolution.RefreshShownValue();

        m_fullscreen.isOn = m_toggled;

        ChangeResolution();


    }
    private FullScreenMode GetScreenMode()
    {
        if (m_fullscreen.isOn)
        {
            m_toggled = m_fullscreen.isOn;
            return FullScreenMode.ExclusiveFullScreen;
        }
        else
        {
            m_toggled = m_fullscreen.isOn;
            return FullScreenMode.Windowed;
        }
    }
    public void ChangeWindowmode()
    {
        GetScreenMode();
        ChangeResolution();
    }

    private int FullscreenModeData(FullScreenMode _mode)
    {
        return m_fullscreenValue = (int)_mode;
    }

    private FullScreenMode GetFullScreenMode()
    {
        return (FullScreenMode)m_fullscreenValue;
    }

    public void LoadData(GameData _data)
    {
        this.m_value = _data.ResolutionValue;
        this.m_resolution.value = m_value;

        this.m_fullscreenValue = _data.FullScreenMode;
        this.GetFullScreenMode();

        this.m_toggled = _data.Toggled;

        this.LoadSettings();
    }

    public void SaveData(ref GameData _data)
    {
        _data.ResolutionValue = this.m_value;
        m_fullscreenValue = this.FullscreenModeData(m_fullscreenMode);
        _data.FullScreenMode = this.m_fullscreenValue;

        _data.Toggled = this.m_toggled;
    }
}
