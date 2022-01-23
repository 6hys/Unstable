using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointReached : MonoBehaviour
{
    public Animator m_DoorAnimator;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            m_DoorAnimator.SetBool("CheckpointHit", true);
        }
    }
}
