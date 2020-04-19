using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BeatsCircle : MonoBehaviour
{
    public MusicHandler musicHandler;
    public AnimationCurve curve;
    public float colorIntensity = 1.0f;

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

    public Color ChangeColor()
    {
        Vector3 col = new Vector3(Random.value, Random.value, Random.value);
        float minValue = 0.1f;
        Vector3 minValue3 = new Vector3(minValue, minValue, minValue);
        col = (col.normalized + minValue3) * colorIntensity;
        Color color = new Color(col.x, col.y, col.z, 1.0f);
        material.SetColor(ColorStr, color);
        return color;
    }

    public Color GetColor()
    {
        return material.GetColor(ColorStr);
    }

}
