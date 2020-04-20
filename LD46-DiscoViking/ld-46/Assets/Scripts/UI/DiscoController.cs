﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

public class DiscoController : MonoBehaviour
{
    public int maxDisco = 10;
    public int maxBoogie = 10;
    //public int maxValhalla = 100;

    private int currDisco;
    private int currBoogie;
    //private int currValhalla;
    public Color discoColor;
    public float discoColorIntensity = 10.0f;

    public MusicEffect musicEffect;
    public DiscoBall discoBall;
    public BeatsCircle beatsCircle;
    //public ValhallaBar valhallaBar;
    public BoogieBarOld boogieBarLeftOld;
    public BoogieBarOld boogieBarRightOld;

    public BoogieBar boogieBarLeft;
    public BoogieBar boogieBarRight;

    public UnityEvent OnGameOver;

    public Volume postprocess;

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
        //currValhalla = 25;
        //valhallaBar.SetMaxValhalla(maxValhalla);
        //valhallaBar.SetValhalla(currValhalla);


        ColorAdjustments ca;
        if (postprocess.profile.TryGet<ColorAdjustments>(out ca))
        {
            print(ca.saturation);
        }

        for (int i = 0; i < postprocess.profile.components.Count; i++)
        {
            print(postprocess.profile.components[i].name);
            print(postprocess.profile.components[i]);
        }
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
        Vector3 color = new Vector3(Random.value, Random.value, Random.value);

        float minValue = 0.1f;
        Vector3 minValue3 = new Vector3(minValue, minValue, minValue);
        color = color.normalized + minValue3;
        
        discoColor = new Color(color.x, color.y, color.z, 1.0f);

        Color boostedColor = discoColor * discoColorIntensity;

        beatsCircle.SetColor(boostedColor * 0.25f);
        discoBall.SetDissolveColor(boostedColor * 0.10f);
        discoBall.SetDissolveRatio(1.0f-GetDiscoRatio());

        boogieBarLeft.SetColor(boostedColor);
        boogieBarRight.SetColor(boostedColor);
    }

    public void OnBeat()
    {
        ChangeColor();
    }

    public void OnBeatSucceed()
    {
        ColorAdjustments colorAdjustments;
        if (postprocess.profile.TryGet<ColorAdjustments>(out colorAdjustments))
        {
            colorAdjustments.saturation.value = 0.0f;
        }
    }

    public void OnFailBeat()
    {
        ColorAdjustments colorAdjustments;
        if (postprocess.profile.TryGet<ColorAdjustments>(out colorAdjustments))
        {
            colorAdjustments.saturation.value = -100.0f;
        }
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
