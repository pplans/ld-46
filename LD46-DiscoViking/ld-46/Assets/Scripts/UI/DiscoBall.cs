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

    private float failTime = -5f;

    void Awake()
    {
        maxDisco = 100;
        material = mesh.GetComponent<MeshRenderer>().materials[0];
    }

    void Update()
    {
        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime, Space.Self);

        float failRatio = failTime - Time.time;
        if (failRatio > 0f)
            transform.localPosition = new Vector3(Random.Range(-failRatio * 0.5f, failRatio * 0.5f), Random.Range(-failRatio * 0.3f, failRatio * 0.3f), 0f);
        else
            transform.localPosition = Vector3.zero;

    }

    public void FailAnimation()
    {
        failTime = Time.time + 0.5f;
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
