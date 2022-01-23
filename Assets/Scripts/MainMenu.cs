using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MainMenu : MonoBehaviour
{
    // https://forum.unity.com/threads/keyboard-navigation-doesnt-work-if-you-click-off-the-ui.421977/
    public EventSystem m_EventSystem;
    private GameObject m_LastSelected;

    public GameManager m_GameManager;

    public GameObject m_ButtonHolder;
    public GameObject m_GuideScreen;
    public GameObject m_GuideButton;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(m_EventSystem.currentSelectedGameObject == null)
        {
            m_EventSystem.SetSelectedGameObject(m_LastSelected);
        }
        else
        {
            m_LastSelected = m_EventSystem.currentSelectedGameObject;
        }
    }

    public void StartPressed()
    {
        m_GameManager.UpdateGameState(GameManager.GameState.State_Playing);
    }

    public void GuidePressed()
    {
        // Display guide,tutorial,etc.
        m_ButtonHolder.SetActive(false);
        m_GuideScreen.SetActive(true);
    }

    public void CloseGuide()
    {
        if(m_GuideScreen.activeSelf == true)
        {
            m_GuideScreen.SetActive(false);
            m_ButtonHolder.SetActive(true);
            m_EventSystem.SetSelectedGameObject(m_GuideButton);
        }
    }
}
