using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoogieBarOld : MonoBehaviour
{
    private Material material;
    private static string MaxBoggieStr = "_MaxBoogie";
    private static string BoggieStr = "_Boogie";

    void Start()
    {
        material = GetComponent<Image>().material;
    }

    void Update()
    {
        
    }

    public void SetMaxBoogie(int val)
    {
        material.SetInt(MaxBoggieStr, val);
    }

    public void SetBoogie(int val)
    {
        material.SetInt(BoggieStr, val);
    }
}
