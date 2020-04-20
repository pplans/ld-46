using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartButton : MonoBehaviour
{

    public MusicHandler musicHandler;
    public DV_GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        gameManager.StartGame();
        Invoke("StartGame", .2f);
    }

    public void StartGame()
    {
        musicHandler.StartMusic();
    }
}
