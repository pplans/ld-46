using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DV_InputManager : MonoBehaviour
{
    public InputAction action;
    public DV_GameManager gameManager;
    private bool bInputDetectionActive;

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
}
