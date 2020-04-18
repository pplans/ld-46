using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BeatsCircle : MonoBehaviour
{
    private Material material;
    private static string CircleRadiusStr = "_BeatsCircleRadius";

    void Start()
    {
        material = GetComponent<Image>().material;
    }

    void Update()
    {
        
    }

    public void SetBeatsCircleRadius(float radius01)
    {
        material.SetFloat(CircleRadiusStr, radius01);
    }
}
