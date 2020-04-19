using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DV_InputManager : MonoBehaviour
{
    public InputAction up;
    public InputAction down;
    public InputAction left;
    public InputAction right;

    public DV_GameManager gameManager;
    private bool bInputDetectionActive;
    private float timeBetweenStuff;
    public Player m_player;
    public World m_world;

    private void Start()
    {
        up.performed += UpPressed;
        up.Enable();

        down.performed += DownPressed;
        down.Enable();

        left.performed += LeftPressed;
        left.Enable();

        right.performed += RightPressed;
        right.Enable();

        bInputDetectionActive = false;

    }

    // Update is called once per frame
    void Update()
    {
        bInputDetectionActive = gameManager.bGameStarted;

    }

    private void UpPressed(InputAction.CallbackContext callbackContext)
    {
          
        if (bInputDetectionActive)
        {

            bool valid = gameManager.musicHandler.ValidateBeat();
            bool obstruction = false;

            if (valid)
            {
                Debug.Log("Up");
                TileState state = m_player.DoMove(new Vector2(0, 1));
                switch (state)
                {
                    case TileState.Occupied:
                        obstruction = true;
                        break;
                }
                if (!obstruction)
                {
                    gameManager.ValidateBeat();
                }
            }
            
        }        

    }

    private void DownPressed(InputAction.CallbackContext callbackContext)
    {

        if (bInputDetectionActive)
        {

            bool valid = gameManager.musicHandler.ValidateBeat();
            bool obstruction = false;

            if (valid)
            {
                Debug.Log("Down");
                TileState state = m_player.DoMove(new Vector2(0, -1));
                switch (state)
                {
                    case TileState.Occupied:
                        obstruction = true;
                        break;
                }
                if (!obstruction)
                {
                    gameManager.ValidateBeat();
                }
            }

        }

    }

    private void LeftPressed(InputAction.CallbackContext callbackContext)
    {

        if (bInputDetectionActive)
        {

            bool valid = gameManager.musicHandler.ValidateBeat();
            bool obstruction = false;

            if (valid)
            {
                Debug.Log("Left");
                TileState state = m_player.DoMove(new Vector2(-1, 0));
                switch (state)
                {
                    case TileState.Occupied:
                        obstruction = true;
                        break;
                }
                if (!obstruction)
                {
                    gameManager.ValidateBeat();
                }
            }

        }

    }

    private void RightPressed(InputAction.CallbackContext callbackContext)
    {

        if (bInputDetectionActive)
        {

            bool valid = gameManager.musicHandler.ValidateBeat();
            bool obstruction = false;

            if (valid)
            {
                Debug.Log("Right");
                TileState state = m_player.DoMove(new Vector2(1,0));
                switch (state)
                {
                    case TileState.Occupied:
                        obstruction = true;
                        break;                    
                }
                if (!obstruction)
                {
                    gameManager.ValidateBeat();
                }
                
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
            m_player.DoMove(newDirection);
    }
}
