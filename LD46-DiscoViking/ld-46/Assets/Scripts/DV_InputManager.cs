using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DV_InputManager : MonoBehaviour
{
    public InputAction action;
    public DV_GameManager gameManager;
    private bool bInputDetectionActive;
    private float timeBetweenStuff;
    public Player m_player;
    public World m_world;

    private void Start()
    {
        action.performed += InputLink;
        action.Enable();

        bInputDetectionActive = false;

    }

    // Update is called once per frame
    void Update()
    {
        bInputDetectionActive = gameManager.bGameStarted;

    }

    private void InputLink(InputAction.CallbackContext callbackContext)
    {
          
        if (bInputDetectionActive)
        {

            bool valid = gameManager.musicHandler.ValidateBeat();

            if (valid)
            {
                Debug.Log(gameManager.musicHandler.GetBeatOffset());
            }
            
        }        

    }

    public void testbeat()
    {
        float timeTest = Time.time;
        Debug.Log(timeTest-timeBetweenStuff);

        timeBetweenStuff = timeTest;
    }

    private void InputPlayer(InputAction.CallbackContext callbackContext)
    {
        Vector2 newDirection = callbackContext.action.ReadValue<Vector2>();
        TileState tileState = m_player.GetTileState(newDirection);
        if (tileState == TileState.BorderRight)
        {
            m_world.UseCache(Random.Range(0, m_world.GetCacheSize()));
            // Load next grid
            m_player.ResetPosition(tileState);
        }
        else
            m_player.DoMove(newDirection, tileState);
    }
}
