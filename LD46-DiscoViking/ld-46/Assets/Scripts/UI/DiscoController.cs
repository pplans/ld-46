using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DiscoController : MonoBehaviour
{
    public int maxDisco = 15;
    public int maxBoogie = 15;
    //public int maxValhalla = 100;

    private int currDisco;
    private int currBoogie;
    //private int currValhalla;

    public MusicEffect musicEffect;
    public DiscoBall discoBall;
    public BeatsCircle beatsCircle;
    //public ValhallaBar valhallaBar;
    public BoogieBar boogieBarLeft;
    public BoogieBar boogieBarRight;

    void Start()
    {
        currDisco = maxDisco;
        discoBall.SetMaxDisco(maxDisco);
        discoBall.SetDisco(currDisco);

        currBoogie = maxBoogie;
        boogieBarLeft.SetBoogie(currBoogie);
        boogieBarLeft.SetMaxBoogie(maxBoogie);
        boogieBarRight.SetBoogie(currBoogie);
        boogieBarRight.SetMaxBoogie(maxBoogie);

        //currValhalla = 25;
        //valhallaBar.SetMaxValhalla(maxValhalla);
        //valhallaBar.SetValhalla(currValhalla);
    }

    void Update()
    {

    }

    public void AddDisco(int amount)
    {
        currDisco = Mathf.Clamp(currDisco + amount, 0, maxDisco);
        discoBall.SetDisco(currDisco);
        if (amount < 0)
            discoBall.FailAnimation();

        musicEffect.ratio = (float)currDisco / maxDisco;
    }

    public void AddBoogie(int amount)
    {
        currBoogie = Mathf.Clamp(currBoogie + amount, 0, maxBoogie);
        boogieBarLeft.SetBoogie(currBoogie);
        boogieBarRight.SetBoogie(currBoogie);
    }

    //void AddValhalla(int amount)
    //{
    //    currValhalla = Mathf.Clamp(currValhalla + amount, 0, maxValhalla);
    //    valhallaBar.SetValhalla(currValhalla);
    //}
}
