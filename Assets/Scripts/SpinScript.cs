using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinScript : MonoBehaviour
{
    public float m_RotationSpeed;
    public bool m_Moving = false;
    public bool m_XAxis = true;

    public GameManager m_GameManager;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Only move if game state is playing
        if (m_Moving && m_GameManager.m_GameState == GameManager.GameState.State_Playing)
        {
            if (m_XAxis)
            {
                transform.Rotate(m_RotationSpeed * Time.deltaTime, 0, 0);
            }
            else
            {
                transform.Rotate(0, 0, m_RotationSpeed * Time.deltaTime);
            }
        }
    }

    public void Reset()
    {
        transform.eulerAngles = Vector3.zero;
    }
}
