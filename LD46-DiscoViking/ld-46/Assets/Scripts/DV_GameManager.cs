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
    public bool bBeatInput;
    public DanceSequence danceSequence;
    public ITileInfo currentDanceTargetTile;
    private List<DV_EnemyAnimation> wokeEnemies;
    private int progressIndex;
    private int difficultyLevel;
    public int progressThreshold;
    public int failBeatBoogieCost;
    public ProgressEffect[] progressEffectArray;

    private int successfulDanceOnThisPlate;

    public bool bPaneCleared;


    public string currentGamePhase;

    public enum ProgressEffect
    {
        Nothing,
        BadBeatCost,
        ValidationWindow
    }

    // Start is called before the first frame update
    void Awake()
    {
        bGameStarted = false;
        bBeatInput = false;
        bBeatValidated = false;
        currentGamePhase = "move";
        wokeEnemies = new List<DV_EnemyAnimation>();
        progressIndex = 0;
        difficultyLevel = 0;
        successfulDanceOnThisPlate = 0;
        bPaneCleared = false;
    }

    public void StartGame()
    {
        bGameStarted = true;
        Debug.Log("Game Is Started motherfucker");

        world.Init(new Vector2 (startGridPos.position.x,startGridPos.position.z), new Vector2(endGridPos.position.x, endGridPos.position.z), new Vector2(1, 1));
        player.Init(new Vector2(0, 0), world);

    }

    public void ValidateBeat(bool succeed)
    {
        bBeatInput = true;
        bBeatValidated = succeed;
    }

    public void EndOfBeatManager()
    {
        if (!bBeatInput)
            MissBeatDamage();
    }

    public void MissBeatDamage()
    {
        if (!musicHandler.started)
            return;

        discoController.AddDisco(-failBeatBoogieCost);
        discoController.AddBoogie(-failBeatBoogieCost);
    }

    public void ResetBeat()
    {
        bBeatValidated = false;
        bBeatInput = false;

        if (inputManager.danceStepIndex == 4)
        {
            inputManager.danceStepIndex = 0;
            SucceedDanceSequence();
        }
    }

    public void BeginDanceSequence(ITileInfo tileInfo)
    {
        //danceSequence.InitializeDanceSequence();
        danceSequence.ShowSequence();
        currentDanceTargetTile = tileInfo;
        currentGamePhase = "dance";
    }

    public void WokenEnemyDance()
    {
        foreach(DV_EnemyAnimation anim in wokeEnemies)
        {
            anim.Dance();
        }
        
    }

    public void SwitchPane()
    {
        successfulDanceOnThisPlate = 0;
        bPaneCleared = successfulDanceOnThisPlate >= world.GetEnnemyCount();
        IncrementProgress();
    }

    public void SucceedDanceSequence()
    {

        danceSequence.HideSequence();
        currentGamePhase = "move";
        currentDanceTargetTile.SetVisited();
        currentDanceTargetTile.GetWorldObject().GetComponent<DV_EnemyAnimation>().WakeUp();
		currentDanceTargetTile.GetWorldObject().GetComponent<Ennemy>().StartDancingImpactVFX();
        wokeEnemies.Add(currentDanceTargetTile.GetWorldObject().GetComponent<DV_EnemyAnimation>());
        discoController.AddDisco(3);
        discoController.AddBoogie(3);
        successfulDanceOnThisPlate++;
        bPaneCleared = successfulDanceOnThisPlate >= world.GetEnnemyCount();
        if (bPaneCleared)
        {
            Debug.Log("You Can Go Now");
            world.ActivateEndColumn();
            foreach (DV_EnemyAnimation anim in wokeEnemies)
            {
                anim.Done();
            }
        }

    }

    public void IncrementProgress()
    {
        progressIndex++;
        if (progressIndex > progressThreshold-1)
        {
            progressIndex = 0;
            if (difficultyLevel< progressEffectArray.Length - 1)
            {
                switch (progressEffectArray[difficultyLevel])
                {
                    case ProgressEffect.Nothing:
                        break;
                    case ProgressEffect.BadBeatCost:
                        IncreaseBadBeatCost();
                        break;
                    case ProgressEffect.ValidationWindow:
                        ReduceValidationWindow();
                        break;
                }
                difficultyLevel++;
            } else
            {
                Debug.Log("max difficulty reached");
            }            
        }
    }

    private void IncreaseBadBeatCost()
    {
        failBeatBoogieCost++;
        Debug.Log("increase fail cost");
    }

    private void ReduceValidationWindow()
    {
        musicHandler.inputValidity -= 0.05f;
        Debug.Log("reducing valid window");
    }
}
