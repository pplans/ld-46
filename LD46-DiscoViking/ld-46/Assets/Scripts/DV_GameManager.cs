using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DV_GameManager : MonoBehaviour
{
    public bool bGameStarted;
    public MusicHandler musicHandler;
    public DiscoController discoController;
    public World world;
    public Player player;
    public Transform startGridPos;
    public Transform endGridPos;
    public bool bBeatValidated;

    // Start is called before the first frame update
    void Start()
    {
        bGameStarted = false;
        bBeatValidated = false;
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

    public void EndOfBeatManager()
    {
        if (!bBeatValidated)
        {
            Debug.Log("BAD BEAT");
            discoController.AddDisco(-1);
            discoController.AddBoogie(-1);
        }
        else
        {
            discoController.AddDisco(1);
            discoController.AddBoogie(1);
        }

        bBeatValidated = false;
    }
}
