using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UsernameAudio : MonoBehaviour
{
    [SerializeField] private AudioClip m_confirmClip;

    public void ConfirmAudio()
    {
        AudioManager.Instance.PlaySound(m_confirmClip);
    }
}
