using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reset : MonoBehaviour
{
    public Transform m_resetLocation;
    public Transform m_sphereLocation;

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
            other.transform.position = m_resetLocation.position;
            other.transform.eulerAngles = m_resetLocation.eulerAngles;
        }
        else if(other.CompareTag("Sphere"))
        {
            other.attachedRigidbody.velocity = Vector3.zero;
            other.attachedRigidbody.isKinematic = true;
            other.transform.position = m_sphereLocation.position;
            other.attachedRigidbody.isKinematic = false;
        }
    }
}
