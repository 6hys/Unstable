using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndScreen : MonoBehaviour
{
    public Text m_FinalTime;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetFinalTime(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60f);
        int seconds = Mathf.FloorToInt(time - (minutes * 60));
        int mseconds = (int)((time - seconds - (minutes * 60)) * 100);

        m_FinalTime.text = string.Format("{0:00}:{1:00}.{2:00}", minutes, seconds, mseconds);
    }
}
