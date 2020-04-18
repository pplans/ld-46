using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiscoBarTest : MonoBehaviour
{
    public int maxValhalla = 100;
    public int currentValhalla;

    public DiscoBar discoBar;

    void Start()
    {
        currentValhalla = 25;
        discoBar.SetMaxValhalla(maxValhalla);
        discoBar.SetValhalla(currentValhalla);
    }

    void Update()
    {
        if(UnityEngine.Input.GetKeyDown(KeyCode.UpArrow))
        {
            AddValhalla(5);
        }
        else if (UnityEngine.Input.GetKeyDown(KeyCode.DownArrow))
        {
            AddValhalla(-5);
        }
    }

    void AddValhalla(int amount)
    {
        currentValhalla += amount;
        Mathf.Clamp(currentValhalla, 0, maxValhalla);
        discoBar.SetValhalla(currentValhalla);
    }
}
