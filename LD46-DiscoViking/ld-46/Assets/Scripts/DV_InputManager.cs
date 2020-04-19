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
    public int danceStepIndex;
    public CharacterAnim anim;

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
            bool enemy = false;
            bool successfulInput = false;

            if (gameManager.currentGamePhase == "move")
            {
                
                if (valid)
                {
                    ITileInfo tileInfo = m_player.DoMove(new Vector2(0, 1));
                    switch (tileInfo.GetState())
                    {
                        case TileState.Occupied:
                            obstruction = true;
                            break;
                        case TileState.Ennemy:
                            enemy = true;
                            break;
                        case TileState.BorderRight:
                            gameManager.IncrementProgress();
                            break;
                    }
                    if (!obstruction)
                    {
                        gameManager.ValidateBeat();
                        successfulInput = true;
                        if (enemy)
                        {
                            if (!tileInfo.GetWorldObject().GetComponent<DV_EnemyAnimation>().bWokenUp)
                            {
                                gameManager.BeginDanceSequence(tileInfo);
                            }
                            
                        }
                    }
                    
                }
            } else
            {
                if (gameManager.danceSequence.CheckStepValidityAgainstInput("Up", danceStepIndex))
                {
                    gameManager.ValidateBeat();
                    gameManager.danceSequence.ValidateStep(danceStepIndex);
                    if (danceStepIndex < 3)
                    {
                        danceStepIndex++;
                        successfulInput = true;
                    }
                    else
                    {
                        danceStepIndex = 0;
                        gameManager.SucceedDanceSequence();
                        successfulInput = true;
                    }
                }
                else
                {
                    gameManager.danceSequence.ResetDanceSequence();
                    danceStepIndex = 0;
                }
            }

            if (successfulInput)
            {
                anim.AnimationStep();
            }
            
            
        }        

    }

    private void DownPressed(InputAction.CallbackContext callbackContext)
    {

        if (bInputDetectionActive)
        {

            bool valid = gameManager.musicHandler.ValidateBeat();
            bool obstruction = false;
            bool enemy = false;
            bool successfulInput = false;

            if (gameManager.currentGamePhase == "move")
            {
                if (valid)
                {
                    ITileInfo tileInfo = m_player.DoMove(new Vector2(0, -1));
                    switch (tileInfo.GetState())
                    {
                        case TileState.Occupied:
                            obstruction = true;
                            break;
                        case TileState.Ennemy:
                            enemy = true;
                            break;
                        case TileState.BorderRight:
                            gameManager.IncrementProgress();
                            break;
                    }
                    if (!obstruction)
                    {
                        gameManager.ValidateBeat();
                        successfulInput = true;
                        if (enemy)
                        {
                            if (!tileInfo.GetWorldObject().GetComponent<DV_EnemyAnimation>().bWokenUp)
                            {
                                gameManager.BeginDanceSequence(tileInfo);
                            }
                        }
                    }
                }
            } else
            {
                if (gameManager.danceSequence.CheckStepValidityAgainstInput("Down", danceStepIndex))
                {
                    gameManager.ValidateBeat();
                    gameManager.danceSequence.ValidateStep(danceStepIndex);
                    if (danceStepIndex < 3)
                    {
                        danceStepIndex++;
                        successfulInput = true;
                    }
                    else
                    {
                        danceStepIndex = 0;
                        gameManager.SucceedDanceSequence();
                        successfulInput = true;
                    }
                }
                else
                {
                    gameManager.danceSequence.ResetDanceSequence();
                    danceStepIndex = 0;
                }
            }

            if (successfulInput)
            {
                anim.AnimationStep();
            }

        }

    }

    private void LeftPressed(InputAction.CallbackContext callbackContext)
    {

        if (bInputDetectionActive)
        {

            bool valid = gameManager.musicHandler.ValidateBeat();
            bool obstruction = false;
            bool enemy = false;
            bool successfulInput = false;

            if (gameManager.currentGamePhase == "move")
            {
                if (valid)
                {
                    ITileInfo tileInfo = m_player.DoMove(new Vector2(-1, 0));
                    switch (tileInfo.GetState())
                    {
                        case TileState.Occupied:
                            obstruction = true;
                            break;
                        case TileState.Ennemy:
                            enemy = true;
                            break;
                        case TileState.BorderRight:
                            gameManager.IncrementProgress();
                            break;
                    }
                    if (!obstruction)
                    {
                        gameManager.ValidateBeat();
                        successfulInput = true;
                        if (enemy)
                        {
                            if (!tileInfo.GetWorldObject().GetComponent<DV_EnemyAnimation>().bWokenUp)
                            {
                                gameManager.BeginDanceSequence(tileInfo);
                            }
                        }
                    }
                }
            } else
            {
                if (gameManager.danceSequence.CheckStepValidityAgainstInput("Left", danceStepIndex))
                {
                    gameManager.ValidateBeat();
                    gameManager.danceSequence.ValidateStep(danceStepIndex);
                    if (danceStepIndex < 3)
                    {
                        danceStepIndex++;
                        successfulInput = true;
                    }
                    else
                    {
                        danceStepIndex = 0;
                        gameManager.SucceedDanceSequence();
                        successfulInput = true;
                    }
                }
                else
                {
                    gameManager.danceSequence.ResetDanceSequence();
                    danceStepIndex = 0;
                }
            }

            if (successfulInput)
            {
                anim.AnimationStep();
            }


        }

    }

    private void RightPressed(InputAction.CallbackContext callbackContext)
    {

        if (bInputDetectionActive)
        {

            bool valid = gameManager.musicHandler.ValidateBeat();
            bool obstruction = false;
            bool enemy = false;
            bool successfulInput = false;

            if (gameManager.currentGamePhase == "move")
            {
                if (valid)
                {
                    ITileInfo tileInfo = m_player.DoMove(new Vector2(1, 0));
                    switch (tileInfo.GetState())
                    {
                        case TileState.Occupied:
                            obstruction = true;
                            break;
                        case TileState.Ennemy:
                            enemy = true;
                            break;
                        case TileState.BorderRight:
                            gameManager.IncrementProgress();
                            break;
                    }
                    if (!obstruction)
                    {
                        gameManager.ValidateBeat();
                        successfulInput = true;
                        if (enemy)
                        {
                            if (!tileInfo.GetWorldObject().GetComponent<DV_EnemyAnimation>().bWokenUp)
                            {
                                gameManager.BeginDanceSequence(tileInfo);
                            }
                        }
                    }
                }
            } else
            {
                if (gameManager.danceSequence.CheckStepValidityAgainstInput("Right", danceStepIndex))
                {
                    gameManager.ValidateBeat();
                    gameManager.danceSequence.ValidateStep(danceStepIndex);
                    if (danceStepIndex < 3)
                    {
                        danceStepIndex++;
                        successfulInput = true;
                    } else
                    {
                        danceStepIndex = 0;
                        gameManager.SucceedDanceSequence();
                        successfulInput = true;
                    }
                    
                }
                else
                {
                    gameManager.danceSequence.ResetDanceSequence();
                    danceStepIndex = 0;
                }
            }

            if (successfulInput)
            {
                anim.AnimationStep();
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
