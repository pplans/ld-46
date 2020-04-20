using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

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

	public Text tutoText;

    private int successfulDanceOnThisPlate;
    private bool bTutoCleared;
    public bool bPaneCleared;

    private bool bFirstInputCleared;
    private int firstInputCount;

    public UnityEvent OnSucceedBeat;
    public UnityEvent OnFailBeat;

    public string currentGamePhase;
    private int scoreValue;

    public MusicEffect musicEffect;

    public Text scoreDisplay;

    public enum ProgressEffect
    {
        Nothing,
        BadBeatCost,
        ValidationWindow
    }

    void AddScore(int score)
    {
        scoreValue += score;
        scoreDisplay.text = "Score: " + scoreValue.ToString();
    }

    void SetScore(int score)
    {
        scoreValue = score;
        scoreDisplay.text = "Score: " + scoreValue.ToString();
    }

    // Start is called before the first frame update
    void Awake()
    {
        //TEMP set to true if ignore tuto from menu
        //
        bGameStarted = false;
        bBeatInput = false;
        bBeatValidated = false;
        currentGamePhase = "move";
        wokeEnemies = new List<DV_EnemyAnimation>();
        progressIndex = 0;
        difficultyLevel = 0;
        successfulDanceOnThisPlate = 0;
        bPaneCleared = false;
        bTutoCleared = false;
        SetScore(0);
        
    }

    public void StartGame()
	{
		bGameStarted = true;

        world.Init(new Vector2 (startGridPos.position.x,startGridPos.position.z), new Vector2(endGridPos.position.x, endGridPos.position.z), new Vector2(1, 1));
        player.Init(new Vector2(0, 0), world);
        tutoText.text = "";
        List<string> descs = world.GetCurrentWorldCacheItem().Desc;
		if(descs!=null)
			foreach (string s in descs)
				tutoText.text += s + "\n";
		if (!Settings.DoSkipTutorials())
		{
			bFirstInputCleared = false;
			firstInputCount = 0;
			musicEffect.tutoMode = true;
			musicHandler.bpmOffset = 0;
		}
		else
		{
			bFirstInputCleared = true;
			bTutoCleared = true;
		}
	}

    public void ValidateBeat(bool succeed)
    {
        bBeatInput = true;
        bBeatValidated = succeed;
        if (bBeatValidated)
        {
            if (!bFirstInputCleared)
            {
                firstInputCount++;
                if (firstInputCount == 4)
                {
                    world.ActivateEndColumn();
                    bFirstInputCleared = true;
                }
            }
            OnSucceedBeat.Invoke();
            AddScore(10);
        }
        
    }

    public void EndOfBeatManager()
    {
        if (!bBeatInput)
        {
            MissBeatDamage();
            bBeatInput = true;
        }
    }

    public void MissBeatDamage()
    {
        if (!musicHandler.started)
            return;

        OnFailBeat.Invoke();

        if (bTutoCleared)
        {
            discoController.AddDisco(-failBeatBoogieCost);
            discoController.AddBoogie(-failBeatBoogieCost);
        }
        
    }

    public void ResetBeat()
    {
        bBeatValidated = false;
        bBeatInput = false;

        if (inputManager.danceStepIndex == 4)
        {
            inputManager.danceStepIndex = 0;
            danceSequence.HideSequence();
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
        AddScore(100);
        world.StopAnimationWaveLeftToRight();
        successfulDanceOnThisPlate = 0;
        bPaneCleared = successfulDanceOnThisPlate >= world.GetEnnemyCount();
        IncrementProgress();
		tutoText.text = "";
		List<string> descs = world.GetCurrentWorldCacheItem().Desc;
		foreach (string s in descs)
			tutoText.text += s + "\n";
	}

    public void SucceedDanceSequence()
    {
        currentGamePhase = "move";
        currentDanceTargetTile.SetVisited();
        currentDanceTargetTile.GetWorldObject().GetComponent<DV_EnemyAnimation>().WakeUp();
		currentDanceTargetTile.GetWorldObject().GetComponent<Ennemy>().StartDancingImpactVFX();
        wokeEnemies.Add(currentDanceTargetTile.GetWorldObject().GetComponent<DV_EnemyAnimation>());
        discoController.AddDisco(3);
        discoController.AddBoogie(3);
        successfulDanceOnThisPlate++;
        bPaneCleared = successfulDanceOnThisPlate >= world.GetEnnemyCount();
        AddScore(50);
        if (bPaneCleared)
        {
            world.ActivateEndColumn();
            world.PlayAnimationWaveLeftToRight();
            foreach (DV_EnemyAnimation anim in wokeEnemies)
            {
                anim.Done();
            }
        }

    }

    public void IncrementProgress()
    {
        progressIndex++;
        if (!bTutoCleared)
        {
            if (progressIndex == 2)
            {
                bTutoCleared = true;
                progressIndex = 0;
                musicEffect.tutoMode = false;
                Debug.Log("Tuto Cleared");
            }
        } else
        {
            if (progressIndex > progressThreshold - 1)
            {
                progressIndex = 0;
                if (difficultyLevel < progressEffectArray.Length - 1)
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
                    Debug.Log("difficulty increased");
                }
                else
                {
                    Debug.Log("max difficulty reached");
                }
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
        musicHandler.inputValidity -= 0.035f;
        Debug.Log("reducing valid window");
    }
}
