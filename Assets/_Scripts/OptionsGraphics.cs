using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class OptionsGraphics : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown m_resolution;


    private FullScreenMode m_fullscreenMode;

    private void Awake()
    {
        if (!UiData.Instance.LoadOptionsOnce)
        {
            m_resolution.value = UiData.Instance.ResolutionValue;

            UiData.Instance.LoadOptionsOnce = true;
        }
    }

    public void ChangeResolution()
    {
        UiData.Instance.ResolutionValue = m_resolution.value;
        switch (UiData.Instance.ResolutionValue)
        {
            case 0:
                Screen.SetResolution(800, 600, FullScreenMode.Windowed);
                break;
            case 1:
                Screen.SetResolution(1280, 800, FullScreenMode.Windowed);
                break;
            case 2:
                Screen.SetResolution(1600, 900, FullScreenMode.Windowed);
                break;
            case 3:
                Screen.SetResolution(1920, 1080, FullScreenMode.Windowed);
                break;
            case 4:
                Screen.SetResolution(2560, 1440, FullScreenMode.Windowed);
                break;
            case 5:
                Screen.SetResolution(3440, 1440, FullScreenMode.Windowed);
                break;
            default:
                break;
        }
    }

    public void OnEnable()
    {
        m_resolution.value = UiData.Instance.ResolutionValue;
    }
}
