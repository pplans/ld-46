using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiscoBall : MonoBehaviour
{
    public float rotationSpeed = 15.0f;
    public MeshRenderer mesh;

    private Material material;
    private static string IntensityOccStr = "_IntensityOcc";

    private int maxDisco;
    private int currDisco;

    void Awake()
    {
        maxDisco = 100;
        material = mesh.GetComponent<MeshRenderer>().materials[0];
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
