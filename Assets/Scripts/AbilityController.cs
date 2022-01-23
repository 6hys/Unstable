using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityController : MonoBehaviour
{
    public enum E_Ability
    {
        Ability_None,
        Ability_Platform
    };

    public E_Ability m_ActiveAbility;
    public Camera m_Camera;

    [Header("UI")]
    public Image m_Crosshair;
    public GameObject m_Border;

    [Header("Platform Ability")]
    public GameObject m_PlatformPrefab;
    public GameObject m_PlatformPreviewPrefab;
    public Transform m_InstantiateLocation;

    [Header("Materials")]
    public Material m_PlatformMaterial;
    public Material m_PlatformHighlight;

    // Private variables
    StarterAssets.FirstPersonController m_FirstPersonController;

    List<GameObject> m_Platforms;
    GameObject m_PlatformPreview;

    // Start is called before the first frame update
    void Start()
    {
        m_ActiveAbility = E_Ability.Ability_None;
        m_FirstPersonController = this.gameObject.GetComponent(typeof(StarterAssets.FirstPersonController)) as StarterAssets.FirstPersonController;

        m_Platforms = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        // Check for inputs for abilities.
        if (Input.GetKeyDown(KeyCode.E))
        {
            // Check that no ability is active.
            if (m_ActiveAbility == E_Ability.Ability_None)
            {
                // Activate the ability
                m_ActiveAbility = E_Ability.Ability_Platform;
                m_Border.SetActive(true);
            }
            else if (m_ActiveAbility == E_Ability.Ability_Platform)
            {
                ResetAbility();
            }
        }

        // Check for jumping.
        if(m_ActiveAbility != E_Ability.Ability_None && !m_FirstPersonController.Grounded)
        {
            ResetAbility();
        }

        // Remove any platform highlights.
        for (int i = 0; i < m_Platforms.Count; i++)
        {
            m_Platforms[i].GetComponent<Renderer>().material = m_PlatformMaterial;
        }

        // Ability Controls
        switch (m_ActiveAbility)
        {
            case E_Ability.Ability_Platform:
                {
                    RaycastHit hitInfo;
                    //Ray centerScreen = m_Camera.ScreenPointToRay(new Vector2(Screen.width / 2, Screen.height / 2));
                    Ray centerScreen = m_Camera.ScreenPointToRay(m_Crosshair.GetComponent<RectTransform>().position);

                    bool hit = Physics.Raycast(centerScreen, out hitInfo, 100.0f);
                    bool previewActive = false;

                    // Check if looking at a platform.
                    if (hit && hitInfo.collider.gameObject.CompareTag("Platform"))
                    {
                        int index = m_Platforms.IndexOf(hitInfo.collider.gameObject);
                        GameObject selected = m_Platforms[index];

                        // Highlight platform
                        selected.GetComponent<Renderer>().material = m_PlatformHighlight;
                       
                        // Destroy platform.
                        if (Input.GetMouseButtonDown(0))
                        {
                            m_Platforms.RemoveAt(index);
                            Destroy(selected);
                        }
                    }
                    // Check if looking at a wall we can build on.
                    else if (hit && hitInfo.collider.gameObject.CompareTag("PlatformTarget"))
                    {
                        Vector3 platformPosition = hitInfo.point;
                        int platformSize = 3;

                        Vector3 normal = hitInfo.normal;

                        // Wall pivot bottom left corner.
                        Vector3 wallSize = hitInfo.collider.bounds.size;
                        Vector3 wallPosition = hitInfo.collider.gameObject.transform.position;

                        if(normal.x >= 0.5f || normal.x <= -0.5f)
                        {
                            // check against y and z.
                            platformPosition.y += (platformSize / 2f);
                            platformPosition.z -= (normal.x * (platformSize / 2f));

                            // Check location would be within the wall space.
                            if (platformPosition.z * normal.x > wallPosition.z * normal.x &&
                                (platformPosition.z - platformSize) * normal.x < (wallPosition.z + (wallSize.z * normal.x)) * normal.x &&
                                platformPosition.y - platformSize > wallPosition.y &&
                                platformPosition.y < wallPosition.y + wallSize.y)
                            {
                                bool freeSpace = true;

                                // Check if it's intersecting with other platforms
                                for (int i = 0; i < m_Platforms.Count; i++)
                                {
                                    // top left
                                    Vector3 i_PlatformPosition = m_Platforms[i].gameObject.transform.position;

                                    if (platformPosition.y < i_PlatformPosition.y + platformSize &&
                                        platformPosition.y > i_PlatformPosition.y - platformSize &&
                                        platformPosition.z * normal.x > (i_PlatformPosition.z - (platformSize * normal.x)) * normal.x &&
                                        platformPosition.z * normal.x < (i_PlatformPosition.z + (platformSize * normal.x)) * normal.x)
                                    {
                                        freeSpace = false;
                                        break;
                                    }
                                }

                                if (freeSpace)
                                {
                                    if (Input.GetMouseButtonDown(0))
                                    {
                                        BuildPlatform(hitInfo, platformPosition, normal);
                                    }
                                    else
                                    {
                                        // Display preview.
                                        DisplayPreview(hitInfo, platformPosition, normal);
                                        previewActive = true;
                                    }
                                }
                            }
                        }
                        else
                        {
                            // check against y and x.
                            platformPosition.y += (platformSize / 2f);
                            platformPosition.x += (normal.z * (platformSize / 2f));

                            // Check location would be within the wall space.
                            if (platformPosition.x * normal.z < wallPosition.x * normal.z &&
                                (platformPosition.x + platformSize) * normal.z > (wallPosition.x + (wallSize.x)) * normal.z &&
                                platformPosition.y - platformSize > wallPosition.y &&
                                platformPosition.y < wallPosition.y + wallSize.y)
                            {
                                bool freeSpace = true;

                                // Check if it's intersecting with other platforms
                                for (int i = 0; i < m_Platforms.Count; i++)
                                {
                                    // top left
                                    Vector3 i_PlatformPosition = m_Platforms[i].gameObject.transform.position;

                                    if (platformPosition.y < i_PlatformPosition.y + platformSize &&
                                        platformPosition.y > i_PlatformPosition.y - platformSize &&
                                        platformPosition.x * normal.z > (i_PlatformPosition.x - (platformSize * normal.z)) * normal.z &&
                                        platformPosition.x * normal.z < (i_PlatformPosition.x + (platformSize * normal.z)) * normal.z)
                                    {
                                        freeSpace = false;
                                        break;
                                    }
                                }

                                if (freeSpace)
                                {
                                    if (Input.GetMouseButtonDown(0))
                                    {
                                        BuildPlatform(hitInfo, platformPosition, normal);
                                    }
                                    else
                                    {
                                        // Display preview.
                                        DisplayPreview(hitInfo, platformPosition, normal);
                                        previewActive = true;
                                    }
                                }
                            }
                        }
                    }

                    if(!previewActive)
                    {
                        PreviewCheck();
                    }
                    break;
                }
            case E_Ability.Ability_None:
                PreviewCheck();
                break;
            default:
                break;
        }
    }

    public E_Ability GetCurrentAbility()
    {
        return m_ActiveAbility;
    }

    public void ResetAbility()
    {
        m_ActiveAbility = E_Ability.Ability_None;
        m_Border.SetActive(false);
    }

    private void BuildPlatform(RaycastHit hitInfo, Vector3 platformPosition, Vector3 normal)
    {
        GameObject newPlatform = Instantiate(m_PlatformPrefab, m_InstantiateLocation.position, Quaternion.identity);

        Vector3 platformRotation = Vector3.zero;

        if (normal.x >= 0.5f || normal.x <= -0.5f)
        {
            platformRotation = new Vector3(90, 0, (normal.x * -90));
        }
        else
        {
            platformRotation = new Vector3(90, 90 * normal.z, 90);
        }

        newPlatform.transform.eulerAngles = platformRotation;
        newPlatform.transform.position = platformPosition;

        m_Platforms.Add(newPlatform);

        if (m_Platforms.Count > 3)
        {
            GameObject oldPlatform = m_Platforms[0];
            m_Platforms.RemoveAt(0);
            Destroy(oldPlatform);
        }
    }

    private void DisplayPreview(RaycastHit hitInfo, Vector3 platformPosition, Vector3 normal)
    {
        // Instantiate object if needed.
        if(m_PlatformPreview == null)
        {
            m_PlatformPreview = Instantiate(m_PlatformPreviewPrefab, m_InstantiateLocation.position, Quaternion.identity);
        }

        Vector3 platformRotation = Vector3.zero;

        // Update position
        if (normal.x >= 0.5f || normal.x <= -0.5f)
        {
            platformRotation = new Vector3(90, 0, (normal.x * -90));
        }
        else
        {
            platformRotation = new Vector3(90, 90 * normal.z, 90);
        }

        m_PlatformPreview.transform.eulerAngles = platformRotation;
        m_PlatformPreview.transform.position = platformPosition;
    }

    private void PreviewCheck()
    {
        if (m_PlatformPreview != null)
        {
            Destroy(m_PlatformPreview);
        }
    }
}
