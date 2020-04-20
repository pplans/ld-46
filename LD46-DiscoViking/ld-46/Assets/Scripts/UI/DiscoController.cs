using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class DiscoController : MonoBehaviour
{
    public int maxDisco = 10;
    public int maxBoogie = 10;
    //public int maxValhalla = 100;

    private int currDisco;
    private int currBoogie;
    //private int currValhalla;

    public MusicEffect musicEffect;
    public DiscoBall discoBall;
    public BeatsCircle beatsCircle;
    //public ValhallaBar valhallaBar;
    public BoogieBarOld boogieBarLeftOld;
    public BoogieBarOld boogieBarRightOld;

    public BoogieBar boogieBarLeft;
    public BoogieBar boogieBarRight;
    public float boogieLightsIntensityMultiplier = 1.0f;

    public UnityEvent OnGameOver;

    void Start()
    {
        currDisco = maxDisco;
        discoBall.SetMaxDisco(maxDisco);
        discoBall.SetDisco(currDisco);

        currBoogie = maxBoogie;
        //boogieBarLeftOld.SetBoogie(currBoogie);
        //boogieBarLeftOld.SetMaxBoogie(maxBoogie);
        //boogieBarRightOld.SetBoogie(currBoogie);
        //boogieBarRightOld.SetMaxBoogie(maxBoogie);

        boogieBarLeft.SetIntensityMultiplier(boogieLightsIntensityMultiplier);
        boogieBarRight.SetIntensityMultiplier(boogieLightsIntensityMultiplier);

        //currValhalla = 25;
        //valhallaBar.SetMaxValhalla(maxValhalla);
        //valhallaBar.SetValhalla(currValhalla);
    }

    public void AddDisco(int amount)
    {
        if (amount < 0)
        {
            discoBall.FailAnimation();
            if (currDisco == 0)
                OnGameOver.Invoke();
        }
        currDisco = Mathf.Clamp(currDisco + amount, 0, maxDisco);

        discoBall.SetDisco(currDisco);
        musicEffect.ratio = GetDiscoRatio();
    }

    public void AddBoogie(int amount)
    {
        currBoogie = Mathf.Clamp(currBoogie + amount, 0, maxBoogie);
        boogieBarLeft.SetBoogie(currBoogie);
        boogieBarRight.SetBoogie(currBoogie);
        //boogieBarLeftOld.SetBoogie(currBoogie);
        //boogieBarRightOld.SetBoogie(currBoogie);
    }

    public void ChangeColor()
    {
        Color color = beatsCircle.ChangeColor();
        discoBall.SetDissolveColor(color);
        discoBall.SetDissolveRatio(1.0f-GetDiscoRatio());

        boogieBarLeft.SetIntensityMultiplier(boogieLightsIntensityMultiplier);
        boogieBarRight.SetIntensityMultiplier(boogieLightsIntensityMultiplier);
        boogieBarLeft.SetColor(color);
        boogieBarRight.SetColor(color);
    }

    private float GetDiscoRatio()
    {
        return (float)currDisco / (float)maxDisco;
    }

    //void AddValhalla(int amount)
    //{
    //    currValhalla = Mathf.Clamp(currValhalla + amount, 0, maxValhalla);
    //    valhallaBar.SetValhalla(currValhalla);
    //}
}
