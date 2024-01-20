using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;

public class AudioManager : MonoBehaviour
{
    private static AudioManager m_instance;

    public static AudioManager Instance
    {
        get
        {
            if (m_instance == null)
            {
                GameObject obj = new GameObject("AudioManager");
                m_instance = obj.AddComponent<AudioManager>();
            }

            return m_instance;
        }
    }

    [SerializeField] private AudioSource m_SFXSource;
    [SerializeField] private AudioSource m_MusicSource;


    private void Awake()
    {
        if (m_instance == null)
        {
            m_instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlaySound(AudioClip _clip)
    {
        m_SFXSource.PlayOneShot(_clip);
    }

    public void ChangeMV(float _value)
    {
        AudioListener.volume = _value;
    }
}
