﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DiscoTest : MonoBehaviour
{
    public int maxDisco = 100;
    public int maxValhalla = 100;

    public int currDisco;
    public int currValhalla;

    public DiscoBall discoBall;
    public BeatsCircle beatsCircle;
    public ValhallaBar valhallaBar;

    [Range(0.0f, 1.0f)]
    public float beatsCircleRadius = 1.0f;


    void Start()
    {
        currDisco = maxDisco;
        discoBall.SetMaxDisco(maxDisco);
        discoBall.SetDisco(currDisco);

        currValhalla = 25;
        valhallaBar.SetMaxValhalla(maxValhalla);
        valhallaBar.SetValhalla(currValhalla);
    }

    void Update()
    {
        if(UnityEngine.Input.GetKeyDown(KeyCode.UpArrow))
        {
            AddDisco(5);
        }
        else if (UnityEngine.Input.GetKeyDown(KeyCode.DownArrow))
        {
            AddDisco(-5);
        }
        else if (UnityEngine.Input.GetKeyDown(KeyCode.LeftArrow))
        {
            AddValhalla(-5);
        }
        else if (UnityEngine.Input.GetKeyDown(KeyCode.RightArrow))
        {
            AddValhalla(+5);
        }

        beatsCircle.SetBeatsCircleRadius(beatsCircleRadius);
    }

    void AddDisco(int amount)
    {
        currDisco = Mathf.Clamp(currDisco + amount, 0, maxDisco);
        discoBall.SetDisco(currDisco);
    }

    void AddValhalla(int amount)
    {
        currValhalla = Mathf.Clamp(currValhalla + amount, 0, maxValhalla);
        valhallaBar.SetValhalla(currValhalla);
    }
}