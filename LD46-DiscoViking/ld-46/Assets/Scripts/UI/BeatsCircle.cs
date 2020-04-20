using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BeatsCircle : MonoBehaviour
{
    public MusicHandler musicHandler;
    public AnimationCurve curve;

    private Material material;
    private static string CircleRadiusStr = "_BeatsCircleRadius";
    private static string ColorStr = "_Color";

    void Start()
    {
        material = GetComponent<Image>().material;
    }

    void Update()
    {
        if (musicHandler && musicHandler.started)
            SetBeatsCircleRadius(curve.Evaluate(musicHandler.GetBeatOffset() + 0.5f));
    }

    public void SetBeatsCircleRadius(float radius01)
    {
        material.SetFloat(CircleRadiusStr, radius01);
    }

    public void SetColor(Color color)
    {
        material.SetColor(ColorStr, color);
    }

}
