using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public enum GameState
    {
        State_StartScreen,
        State_Playing,
        State_EndScreen
    }

    public GameState m_GameState;
    public GameObject m_Player;

    [Header("UI")]
    public GameObject m_StartScreen;
    public GameObject m_PlayingScreen;
    public GameObject m_EndScreen;

    [Header("Cameras")]
    public Camera m_MenuCamera;
    public Camera m_GameCamera;

    [Header("Starter Assets")]
    public StarterAssets.FirstPersonController m_FPC;

    [Header("Reset")]
    public SphereSpawner[] m_Spheres;
    public Animator[] m_Gates;
    public Animator[] m_MovingFloors;
    public SpinScript[] m_RotatingFloors;
    public MovingFloorTrigger m_MovingFloorTrigger;
    public SphereCheckpoint m_SphereCheckpoint;
    
    private const float c_MoveSpeed = 5f;
    private const float c_SprintSpeed = 5f;
    private const float c_RotationSpeed = 0.25f;
    private const float c_JumpHeight = 1.2f;
    private const float c_Gravity = -15f;

    // Start is called before the first frame update
    void Start()
    {
        UpdateGameState(GameState.State_StartScreen);
    }

    // Update is called once per frame
    void Update()
    {

        switch(m_GameState)
        {
            case GameState.State_StartScreen:
                // Check for inputs.
                if(Input.GetKeyDown(KeyCode.V))
                {
                    // Closes the guide if it's open.
                    m_StartScreen.GetComponent<MainMenu>().CloseGuide();
                }
                break;
            case GameState.State_Playing:
                break;
            case GameState.State_EndScreen:
                // Check for inputs.
                if (Input.GetKeyDown(KeyCode.V))
                {
                    // Returns to main menu
                    UpdateGameState(GameState.State_StartScreen);
                }
                break;
            default:
                break;
        }
    }

    public void UpdateGameState(GameState newState)
    {
        switch (newState)
        {
            case GameState.State_StartScreen:
                m_EndScreen.SetActive(false);
                // Camera setup
                m_MenuCamera.enabled = true;
                m_GameCamera.enabled = false;
                // Player movement disabled
                SetPlayerMovement(false);
                // Show screen
                m_StartScreen.SetActive(true);
                break;

            case GameState.State_Playing:
                // Fully reset game. This sets all elements back to their intended positions.
                ResetGame();
                // Camera setup
                m_MenuCamera.enabled = false;
                m_GameCamera.enabled = true;
                // Enable player movement
                SetPlayerMovement(true);
                // Setup the UI
                m_StartScreen.SetActive(false);
                m_PlayingScreen.SetActive(true);
                break;

            case GameState.State_EndScreen:
                // Stop the timer
                float finalTime = m_PlayingScreen.GetComponent<Timer>().StopTimer();
                // Leaving the game
                m_PlayingScreen.SetActive(false);
                // Camera setup
                m_MenuCamera.enabled = true;
                m_GameCamera.enabled = false;
                // Player movement disabled
                SetPlayerMovement(false);
                // Show end screen
                m_EndScreen.SetActive(true);
                m_EndScreen.GetComponent<EndScreen>().SetFinalTime(finalTime);
                break;

            default:
                break;
        }

        m_GameState = newState;
    }
    
    private void SetPlayerMovement(bool canMove)
    {
        if(canMove)
        {
            m_FPC.MoveSpeed = c_MoveSpeed;
            m_FPC.SprintSpeed = c_SprintSpeed;
            m_FPC.JumpHeight = c_JumpHeight;
            m_FPC.RotationSpeed = c_RotationSpeed;
            m_FPC.Gravity = c_Gravity;
        }
        else
        {
            m_FPC.MoveSpeed = 0f;
            m_FPC.SprintSpeed = 0f;
            m_FPC.JumpHeight = 0f;
            m_FPC.RotationSpeed = 0f;
            m_FPC.Gravity = 0f;
        }
    }

    private void ResetGame()
    {
        foreach (SphereSpawner sphere in m_Spheres)
        {
            // Delete all spheres if they exist.
            sphere.Reset();
        }

        foreach (Animator gate in m_Gates)
        {
            // Reset all gate animators to the start.
            gate.SetBool("CheckpointHit", false);
            gate.Play("GateClosed");
        }

        foreach (Animator floor in m_MovingFloors)
        {
            // Reset all floor animators to the start.
            floor.SetBool("Moving", false);
            floor.Play("New State");
            
        }

        foreach (SpinScript spin in m_RotatingFloors)
        {
            // Reset position of all spinning platforms.
            spin.Reset();
        }

        // Reset the timer.
        m_PlayingScreen.GetComponent<Timer>().Reset();

        // Reset triggers
        m_MovingFloorTrigger.Reset();
        m_SphereCheckpoint.Reset();

        // Reset player position.
        m_Player.transform.position = new Vector3(0f, 0.5f, 0f);
        // Make sure build ability is turned off.
        m_Player.GetComponent<AbilityController>().ResetAbility();
    }
}
