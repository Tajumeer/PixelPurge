using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class OptionsGraphics : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown m_resolution;
    [SerializeField] private Toggle m_fullscreen;


    private FullScreenMode m_fullscreenMode;

    private void Awake()
    {
        if (!UiData.Instance.LoadOptionsOnce)
        {
            m_resolution.value = UiData.Instance.ResolutionValue;
            m_fullscreen.isOn = UiData.Instance.FullscreenToggle;

            ChangeResolution();

            Debug.LogError("Settings Loaded - Resolution: " + m_resolution.options[m_resolution.value].text + ", Fullscreen: " + UiData.Instance.FullscreenToggle);

            UiData.Instance.LoadOptionsOnce = true;
        }
    }

    public void ChangeResolution()
    {
        UiData.Instance.ResolutionValue = m_resolution.value;
        switch (UiData.Instance.ResolutionValue)
        {
            case 0:
                Screen.SetResolution(800, 600, m_fullscreenMode);
                break;
            case 1:
                Screen.SetResolution(1280, 800, m_fullscreenMode);
                break;
            case 2:
                Screen.SetResolution(1600, 900, m_fullscreenMode);
                break;
            case 3:
                Screen.SetResolution(1920, 1080, m_fullscreenMode);
                break;
            case 4:
                Screen.SetResolution(2560, 1440, m_fullscreenMode);
                break;
            case 5:
                Screen.SetResolution(3440, 1440, m_fullscreenMode);
                break;
            default:
                break;
        }
    }

    public void OnEnable()
    {
        m_resolution.value = UiData.Instance.ResolutionValue;
        m_fullscreen.isOn = UiData.Instance.FullscreenToggle;
    }

    public void ChangeWindowmode()
    {
        if (UiData.Instance.FullscreenToggle == true)
        {
            UiData.Instance.FullscreenToggle = false;
            m_fullscreenMode = FullScreenMode.Windowed;
        }
        else
        {
            UiData.Instance.FullscreenToggle = true;
            m_fullscreenMode = FullScreenMode.ExclusiveFullScreen;
        }
        ChangeResolution();
    }
}
