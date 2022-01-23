using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereCheckpoint : MonoBehaviour
{
    public Animator m_DoorAnimator;

    private bool m_Entered = false;
    private GameObject m_Sphere = null;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(m_Entered)
        {
            // Check velocity. It might bounce out if going too fast.
            if(m_Sphere.GetComponent<Rigidbody>().velocity.magnitude <= 0.15f)
            {
                // Going slow enough
                m_DoorAnimator.SetBool("CheckpointHit", true);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Sphere"))
        {
            m_Entered = true;
            m_Sphere = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Sphere"))
        {
            m_Entered = false;
            m_Sphere = null;
        }
    }

    public void Reset()
    {
        m_Entered = false;
    }
}
