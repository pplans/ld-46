using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiscoBall : MonoBehaviour
{
    public float rotationSpeed = 15.0f;

    private Material material;
    private static string IntensityOccStr = "Vector1_6580437B";

    private int maxDisco;
    private int currDisco;

    void Start()
    {
        maxDisco = 100;
        material = GetComponent<MeshRenderer>().material;
    }

    void Update()
    {
        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime, Space.Self);
    }

    public void SetRotationSpeed(float speed)
    {
        rotationSpeed = speed;
    }

    public void SetMaxDisco(int val)
    {
        maxDisco = val;
    }

    public void SetDisco(int val)
    {
        currDisco = val;
        float r = (float)currDisco / maxDisco;
        SetDiscoBallIntensity(r);
    }

    private void SetDiscoBallIntensity(float intensity)
    {
        material.SetFloat(IntensityOccStr, intensity);
    }
}
