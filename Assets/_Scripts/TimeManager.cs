using UnityEngine;
using System.Collections.Generic;

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

    public void StartTimer(string timerName)
    {
        if (!m_timers.ContainsKey(timerName))
        {
            m_timers.Add(timerName, 0f);
        }
    }

    public void StopTimer(string timerName)
    {
        if (m_timers.ContainsKey(timerName))
        {
            m_timers.Remove(timerName);
        }
    }

    public float GetElapsedTime(string timerName)
    {
        if (m_timers.ContainsKey(timerName))
        {
            return m_timers[timerName];
        }

        return 0f;
    }

    public void UpdateTimers()
    {
        foreach (var entry in m_timers)
        {
            string timerName = entry.Key;
            float elapsedTime = entry.Value;

            elapsedTime += Time.deltaTime;
            m_timers[timerName] = elapsedTime;
        }
    }
}
