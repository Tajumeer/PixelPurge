using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelUpMenuSound : MonoBehaviour
{
    [SerializeField] private AudioClip m_activeSound;
    [SerializeField] private AudioClip m_passiveSound;
    [SerializeField] private AudioClip m_goldSound;

    public void ActiveAudio()
    {
        AudioManager.Instance.PlaySound(m_activeSound);
    }
    public void PassiveAudio()
    {
        AudioManager.Instance.PlaySound(m_passiveSound);
    }

    public void GoldAudio()
    {
        AudioManager.Instance.PlaySound(m_goldSound);
    }
}
