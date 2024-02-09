using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsMenuAudio : MonoBehaviour
{
    [Header("Audio Clips")]
    [SerializeField] private AudioClip m_tabClip;
    [SerializeField] private AudioClip m_exitClip;
    [SerializeField] private AudioClip m_resolutionClip;
    [SerializeField] private AudioClip m_sliderClip;

    [Header("Slider")]
    [SerializeField] private Slider m_masterSlider;
    [SerializeField] private Slider m_sfxSlider;
    [SerializeField] private Slider m_musicSlider;



    private AudioManager m_am;
    private void Awake()
    {
        m_am = AudioManager.Instance;
        m_masterSlider.value = m_am.MasterVol;
        m_musicSlider.value = m_am.MusicVol;
        m_sfxSlider.value = m_am.SFXVol;
    }

    public void TabAudio()
    {
        m_am.PlaySound(m_tabClip);
    }

    public void ExitAudio()
    {
        m_am.PlaySound(m_exitClip);
    }

    public void ResolutionAudio()
    {
        m_am.PlaySound(m_resolutionClip);
    }

    public void SFXSliderChange()
    {
        m_am.PlaySound(m_sliderClip);
        m_am.ChangeEffectVolume(m_sfxSlider.value);
    }
    public void MusicSliderChange()
    {
        m_am.PlaySound(m_sliderClip);
        m_am.ChangeMusicVolume(m_musicSlider.value);
    }
    public void MasterSliderChange()
    {
        m_am.PlaySound(m_sliderClip);
        m_am.ChangeMasterVolume(m_masterSlider.value);
    }


}
