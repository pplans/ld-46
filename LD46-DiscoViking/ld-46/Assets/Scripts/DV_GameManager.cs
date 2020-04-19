using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DV_GameManager : MonoBehaviour
{
    public bool bGameStarted;
    public MusicHandler musicHandler;
    public World world;
    public Player player;
    public DV_InputManager inputManager;
    public Transform startGridPos;
    public Transform endGridPos;
    public bool bBeatValidated;
    public DanceSequence danceSequence;


    public string currentGamePhase;

    // Start is called before the first frame update
    void Start()
    {
        bGameStarted = false;
        bBeatValidated = false;
        currentGamePhase = "move";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartGame()
    {
        bGameStarted = true;
        Debug.Log("Game Is Started motherfucker");

        world.Init(new Vector2 (startGridPos.position.x,startGridPos.position.z), new Vector2(endGridPos.position.x, endGridPos.position.z), new Vector2(1, 1));
        player.Init(new Vector2(0, 0), world);

    }

    public void ValidateBeat()
    {
        bBeatValidated = true;
    }

    public void StartDanceSequence()
    {

    }

    public void EndOfBeatManager()
    {
        if (!bBeatValidated)
        {
            //Debug.Log("BAD BEAT");
        }

        bBeatValidated = false;
    }

    public void BeginDanceSequence()
    {
        danceSequence.InitializeDanceSequence();
        currentGamePhase = "dance";
    }

    public void SucceedDanceSequence()
    {
        danceSequence.HideSequence();
        currentGamePhase = "move";
    }
}
