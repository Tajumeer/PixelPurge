using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsAudio : MonoBehaviour
{
    [SerializeField] private AudioClip m_exitClip;

    public void ExitAudio()
    {
        AudioManager.Instance.PlaySound(m_exitClip);
    }
}
