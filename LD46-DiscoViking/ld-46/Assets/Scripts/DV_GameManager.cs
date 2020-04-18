using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DV_GameManager : MonoBehaviour
{
    public bool bGameStarted;
    public MusicHandler musicHandler;
    public World world;
    public Player player;
    public Transform startGridPos;
    public Transform endGridPos;

    // Start is called before the first frame update
    void Start()
    {
        bGameStarted = false;
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

    }
}
