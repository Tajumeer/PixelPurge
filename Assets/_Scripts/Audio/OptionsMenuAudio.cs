using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsMenuAudio : MonoBehaviour
{

    [SerializeField] private AudioClip m_tabClip;
    [SerializeField] private AudioClip m_exitClip;

    public void TabAudio()
    {
        AudioManager.Instance.PlaySound(m_tabClip);
    }

    public void ExitAudio()
    {
        AudioManager.Instance.PlaySound(m_exitClip);
    }
}
