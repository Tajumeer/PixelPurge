using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEndAudio : MonoBehaviour
{
    [SerializeField] private AudioClip m_goAgainClip;
    [SerializeField] private AudioClip m_goVillageClip;

    public void VillageAudio()
    {
        AudioManager.Instance.PlaySound(m_goVillageClip);
    }

    public void AgainAudio()
    {
        AudioManager.Instance.PlaySound(m_goAgainClip);
    }
}
