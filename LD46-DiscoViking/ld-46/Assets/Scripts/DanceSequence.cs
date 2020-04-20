using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DanceSequence : MonoBehaviour
{
    public enum DanceStep
    {
        Up,
        Down,
        Left,
        Right
    }

    public DanceStep[] DanceSequenceSteps;
    public Sprite[] spriteUp;
    public Sprite[] spriteDown;
    public Sprite[] spriteLeft;
    public Sprite[] spriteRight;
    public SpriteRenderer[] stepDisplays;
    public DiscoController discoController;


    // Start is called before the first frame update
    void Start()
    {
        //InitializeDanceSequence();
        HideSequence();
    }

    public void HideSequence()
    {
        Debug.Log("HideSequence");
        if (stepDisplays == null)
            return;
        foreach (SpriteRenderer rend in stepDisplays)
        {
            rend.enabled = false;
        }
    }

    public void ShowSequence()
    {
        Debug.Log("ShowSequence");
        InitializeDanceSequence();
        ResetDanceSequence();
        //foreach (SpriteRenderer rend in stepDisplays)
        //{
        //    rend.enabled = true;
        //}
    }

    public bool CheckStepValidityAgainstInput(string inputString, int stepIndex)
    {
        bool bInputIsValid = false;
        switch (inputString) 
        {
            case "Up":
                bInputIsValid = DanceSequenceSteps[stepIndex] == DanceStep.Up;
                break;
            case "Down":
                bInputIsValid = DanceSequenceSteps[stepIndex] == DanceStep.Down;
                break;
            case "Left":
                bInputIsValid = DanceSequenceSteps[stepIndex] == DanceStep.Left;
                break;
            case "Right":
                bInputIsValid = DanceSequenceSteps[stepIndex] == DanceStep.Right;
                break;
        }

        Debug.Log(bInputIsValid);

        return bInputIsValid;
    }

    public void ValidateStep(int danceStepIndex)
    {
        danceStepIndex = Mathf.Min(3, danceStepIndex);
        SpriteRenderer renderer = stepDisplays[danceStepIndex];

        //print("Validate step " + renderer.material.GetFloat("_Intensity"));
        //renderer.material.SetFloat("_Intensity", 1.0f);
        ChangeSpriteFromStepValue(stepDisplays[danceStepIndex], DanceSequenceSteps[danceStepIndex], 1);
    }

    public void ResetDanceSequence()
    {
        int index = 0;
        foreach (SpriteRenderer rend in stepDisplays)
        {
            ChangeSpriteFromStepValue(rend, DanceSequenceSteps[index], 0);
            //rend.material.SetFloat("_Intensity", 0.1f);
            index++;
        }
    }

    private DanceStep GenerateRandomStep()
    {
        int rand = Random.Range(0, 4);
        DanceStep step = DanceStep.Up;

        switch (rand)
        {
            case 0:
                step = DanceStep.Up;
                break;
            case 1:
                step = DanceStep.Down;
                break;
            case 2:
                step = DanceStep.Left;
                break;
            case 3:
                step = DanceStep.Right;
                break;
        }

        return step;
    }

    private void ChangeSpriteFromStepValue(SpriteRenderer renderer, DanceStep step, int type)
    {
        switch (step)
        {
            case DanceStep.Up:
                renderer.sprite = spriteUp[type];
                break;
            case DanceStep.Down:
                renderer.sprite = spriteDown[type];
                break;
            case DanceStep.Left:
                renderer.sprite = spriteLeft[type];
                break;
            case DanceStep.Right:
                renderer.sprite = spriteRight[type];
                break;
        }
    }


    public void InitializeDanceSequence()
    {
        DanceSequenceSteps = new DanceStep[4];
        int index = 0;

        foreach (DanceStep step in DanceSequenceSteps)
        {
            DanceSequenceSteps[index] = GenerateRandomStep();
            ChangeSpriteFromStepValue(stepDisplays[index], DanceSequenceSteps[index],0);
            stepDisplays[index].enabled = true;
            //stepDisplays[index].material.SetFloat("_Intensity", 0.1f);
            //stepDisplays[index].material.SetColor("_Color", discoController.beatsCircle.GetColor());
            index++;
        }


    }
}
