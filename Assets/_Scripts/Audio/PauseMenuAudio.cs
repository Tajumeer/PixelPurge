using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuAudio : MonoBehaviour
{
    [SerializeField] private AudioClip m_resumeClip;
    [SerializeField] private AudioClip m_optionsClip;
    [SerializeField] private AudioClip m_quitClip;

    public void ResumeAudio()
    {
        AudioManager.Instance.PlaySound(m_resumeClip);
    }

    public void OptionsAudio()
    {
        AudioManager.Instance.PlaySound(m_optionsClip);
    }

    public void QuitAudio()
    {
        AudioManager.Instance.PlaySound(m_quitClip);
    }
}
