using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Timer : MonoBehaviour
{
    public Text m_TimerText;

    private float m_TimePassed;

    private bool m_TimerOn;

    // Start is called before the first frame update
    void Start()
    {
        m_TimePassed = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (m_TimerOn)
        {
            m_TimePassed += Time.deltaTime;

            int minutes = Mathf.FloorToInt(m_TimePassed / 60f);
            int seconds = Mathf.FloorToInt(m_TimePassed - (minutes * 60));
            int mseconds = (int)((m_TimePassed - seconds - (minutes * 60)) * 100);

            m_TimerText.text = string.Format("{0:00}:{1:00}.{2:00}", minutes, seconds, mseconds);
        }
    }

    public void Reset()
    {
        m_TimePassed = 0f;
        m_TimerOn = true;
    }

    public float StopTimer()
    {
        m_TimerOn = false;
        return m_TimePassed;
    }
}
