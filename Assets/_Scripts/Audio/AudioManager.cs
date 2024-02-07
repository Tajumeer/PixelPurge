
using UnityEngine;
using UnityEngine.Audio;


public class AudioManager : MonoBehaviour, IDataPersistence
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

    [SerializeField] private AudioMixer m_mixer;
    [SerializeField] private AudioSource m_SFXSource;
    [SerializeField] private AudioSource m_musicSource;

    [HideInInspector] public float MasterVol;
    [HideInInspector] public float MusicVol;
    [HideInInspector] public float SFXVol;

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

    private void InitAudio()
    {
        m_mixer.SetFloat("MasterVolume", MasterVol);
        m_mixer.SetFloat("MusicVolume", MusicVol);
        m_mixer.SetFloat("SFXVolume", SFXVol);
    }

    public void PlaySound(AudioClip _clip)
    {
        m_SFXSource.PlayOneShot(_clip);
    }

    public void ChangeMasterVolume(float _value)
    {
        m_mixer.SetFloat("MasterVolume", _value);
        MasterVol = _value;
    }

    public void ChangeMusicVolume(float _value)
    {
        m_mixer.SetFloat("MusicVolume", _value);
        MusicVol = _value;
    }

    public void ChangeEffectVolume(float _value)
    {
        m_mixer.SetFloat("SFXVolume", _value);
        SFXVol = _value;
    }

    public void LoadData(GameData _data)
    {
        MasterVol = _data.MasterVolume;
        MusicVol = _data.MusicVolume;
        SFXVol = _data.EffectVolume;

        InitAudio();
    }

    public void SaveData(ref GameData _data)
    {
        _data.MasterVolume = MasterVol;
        _data.MusicVolume = MusicVol;
        _data.EffectVolume = SFXVol;
    }
}
