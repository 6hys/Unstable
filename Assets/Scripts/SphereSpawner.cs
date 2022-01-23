using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereSpawner : MonoBehaviour
{
    public Transform m_SpawnLocation;
    public GameObject m_SpherePrefab;

    private GameObject m_Sphere = null;
    private bool m_Spawning = false;
    
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (m_Spawning)
        {
            if (m_Sphere == null)
            {
                // Spawn new sphere.
                m_Sphere = Instantiate(m_SpherePrefab, m_SpawnLocation.position, Quaternion.identity);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            m_Spawning = true;
        }
    }

    public void Reset()
    {
        if(m_Sphere != null)
        {
            m_Spawning = false;
            Destroy(m_Sphere);
        }
    }
}