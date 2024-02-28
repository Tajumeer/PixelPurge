using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using static Unity.Burst.Intrinsics.X86.Avx;

public class UiData : MonoBehaviour, IDataPersistence
{
    private static UiData m_instance;

    public static UiData Instance
    {
        get
        {
            if (m_instance == null)
            {
                GameObject obj = new GameObject("UiData");
                m_instance = obj.AddComponent<UiData>();
            }

            return m_instance;
        }
    }
    //Options Menu
    [HideInInspector] public int ResolutionValue;
    [HideInInspector] public bool LoadOptionsOnce;

    //Upgrade Menu
    //visual
    [HideInInspector] public int[] StatLevel;

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

    public void LoadData(GameData _data)
    {
        ResolutionValue = _data.ResolutionValue;
        StatLevel = _data.StatLevel;
    }

    public void SaveData(ref GameData _data)
    {
        _data.ResolutionValue = this.ResolutionValue;
        _data.StatLevel = this.StatLevel;
    }
}


