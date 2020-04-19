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
    public DV_InputManager inputManager;
    public Transform startGridPos;
    public Transform endGridPos;
    public bool bBeatValidated;
    public DanceSequence danceSequence;
    public WorldObject currentDanceTarget;
    private List<DV_EnemyAnimation> wokeEnemies;


    public string currentGamePhase;

    // Start is called before the first frame update
    void Awake()
    {
        bGameStarted = false;
        bBeatValidated = false;
        currentGamePhase = "move";
        wokeEnemies = new List<DV_EnemyAnimation>();
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
            Debug.Log("BAD BEAT");
            discoController.AddDisco(-1);
            discoController.AddBoogie(-1);
        }
        else
        {
            
        }

        bBeatValidated = false;
    }

    public void BeginDanceSequence(WorldObject danceTarget)
    {
        danceSequence.InitializeDanceSequence();
        currentDanceTarget = danceTarget;
        currentGamePhase = "dance";
    }

    public void WokenEnemyDance()
    {
        foreach(DV_EnemyAnimation anim in wokeEnemies)
        {
            anim.Dance();
        }
        //Debug.Log(wokeEnemies.Count);
    }

    public void SucceedDanceSequence()
    {
        danceSequence.HideSequence();
        currentGamePhase = "move";
        currentDanceTarget.GetComponent<DV_EnemyAnimation>().WakeUp();
        wokeEnemies.Add(currentDanceTarget.GetComponent<DV_EnemyAnimation>());
        discoController.AddDisco(3);
        discoController.AddBoogie(3);
    }
}
