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
        Move(new Vector2(0, 1));
    }

    private void DownPressed(InputAction.CallbackContext callbackContext)
    {
        Move(new Vector2(0, -1));
    }

    private void LeftPressed(InputAction.CallbackContext callbackContext)
    {
        Move(new Vector2(-1, 0));
    }

    private void RightPressed(InputAction.CallbackContext callbackContext)
    {
        Move(new Vector2(1, 0));
    }

    private void Move(Vector2 dir)
    {
        if (bInputDetectionActive)
        {
            bool valid = gameManager.musicHandler.ValidateBeat();
            bool obstruction = false;
            bool enemy = false;
            bool successfulInput = false;

            if (gameManager.currentGamePhase == "move")
            {
                if (valid && gameManager.bBeatInput == false)
                {
                    ITileInfo tileInfo = m_player.DoMove(dir);
                    switch (tileInfo.GetState())
                    {
                        case TileState.Occupied:
                            obstruction = true;
                            break;
                        case TileState.Ennemy:
                            enemy = true;
                            break;
                    }
                    if (!obstruction)
                    {
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
            }
            else
            {
                string moveDirection;
                if (dir.x == 0f)
                {
                    if (dir.y > 0f)
                        moveDirection = "Up";
                    else
                        moveDirection = "Down";
                }
                else
                {
                    if (dir.x > 0f)
                        moveDirection = "Right";
                    else
                        moveDirection = "Left";
                }

                successfulInput = gameManager.danceSequence.CheckStepValidityAgainstInput(moveDirection, danceStepIndex);
                if (successfulInput)
                {
                    gameManager.danceSequence.ValidateStep(danceStepIndex);
                    if (danceStepIndex < 3)
                        danceStepIndex++;
                    else
                    {
                        danceStepIndex = 0;
                        gameManager.SucceedDanceSequence();
                    }
                }
                else
                {
                    gameManager.danceSequence.ResetDanceSequence();
                    danceStepIndex = 0;
                }
            }

            if (successfulInput)
                anim.AnimationStep();
            else
                gameManager.MissBeatDamage();

            gameManager.ValidateBeat(successfulInput);
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
