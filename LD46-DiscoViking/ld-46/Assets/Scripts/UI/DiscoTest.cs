using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DiscoTest : MonoBehaviour
{
    public int maxDisco = 15;
    public int maxBoogie = 15;
    public int maxValhalla = 100;

    public int currDisco;
    public int currBoogie;
    public int currValhalla;

    public DiscoBall discoBall;
    public BeatsCircle beatsCircle;
    public ValhallaBar valhallaBar;
    public BoogieBar boogieBarLeft;
    public BoogieBar boogieBarRight;

    [Range(0.0f, 1.0f)]
    public float beatsCircleRadius = 1.0f;


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

        currValhalla = 25;
        valhallaBar.SetMaxValhalla(maxValhalla);
        valhallaBar.SetValhalla(currValhalla);
    }

    void Update()
    {
        if(UnityEngine.Input.GetKeyDown(KeyCode.UpArrow))
        {
            AddBoogie(+1);
            AddDisco(+1);
        }
        else if (UnityEngine.Input.GetKeyDown(KeyCode.DownArrow))
        {
            AddBoogie(-1);
            AddDisco(-1);
        }
        else if (UnityEngine.Input.GetKeyDown(KeyCode.LeftArrow))
        {
            AddValhalla(-5);
        }
        else if (UnityEngine.Input.GetKeyDown(KeyCode.RightArrow))
        {
            AddValhalla(+5);
        }

        //beatsCircle.SetBeatsCircleRadius(beatsCircleRadius);
    }

    void AddDisco(int amount)
    {
        currDisco = Mathf.Clamp(currDisco + amount, 0, maxDisco);
        discoBall.SetDisco(currDisco);
    }

    void AddBoogie(int amount)
    {
        currBoogie = Mathf.Clamp(currBoogie + amount, 0, maxBoogie);
        boogieBarLeft.SetBoogie(currBoogie);
        boogieBarRight.SetBoogie(currBoogie);
    }

    void AddValhalla(int amount)
    {
        currValhalla = Mathf.Clamp(currValhalla + amount, 0, maxValhalla);
        valhallaBar.SetValhalla(currValhalla);
    }
}
