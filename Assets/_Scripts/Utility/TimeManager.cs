using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class TimeManager : MonoBehaviour
{
    private static TimeManager m_instance;

    private Dictionary<string, float> m_timers = new Dictionary<string, float>();

    public static TimeManager Instance
    {
        get
        {
            if (m_instance == null)
            {
                GameObject obj = new GameObject("TimerManager");
                m_instance = obj.AddComponent<TimeManager>();
            }

            return m_instance;
        }
    }

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

    private void Update()
    {
        UpdateTimers();
    }

    public void StartTimer(string _timerName)
    {
        if (!m_timers.ContainsKey(_timerName))
        {
            m_timers.Add(_timerName, 0f);
        }
    }

    public void StopTimer(string _timerName)
    {
        if (m_timers.ContainsKey(_timerName))
        {
            m_timers.Remove(_timerName);
        }
    }

    public float GetElapsedTime(string _timerName)
    {
        if (m_timers.ContainsKey(_timerName))
        {
            return m_timers[_timerName];
        }

        return 0f;
    }

    public void SetTimer(string _timerName, float _newValue)
    {
        if (m_timers.ContainsKey(_timerName))
        {
            m_timers[_timerName] = _newValue;
        }
    }
    public void UpdateTimers()
    {
        Dictionary<string, float> copyOfTimers = new Dictionary<string, float>(m_timers);

        foreach (var entry in copyOfTimers)
        {
            string timerName = entry.Key;
            float elapsedTime = entry.Value;

            elapsedTime += Time.deltaTime;
            m_timers[timerName] = elapsedTime;
        }
    }
}
