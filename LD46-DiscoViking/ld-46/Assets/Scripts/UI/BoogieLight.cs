using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoogieLight : MonoBehaviour
{
    public float intensityMultiplier = 1.0f;
    public bool active = true;

    private Material material;
    private Color color;

    void Start()
    {
        material = GetComponent<MeshRenderer>().material;
        color = material.GetColor("_EmissiveColor");
    }

    void Update()
    {
        
    }

    public void SetActive(bool a)
    {
        active = a;
        if(!active)
        {
            material.SetColor("_EmissiveColor", Color.black);
        }
        else
        {
            material.SetColor("_EmissiveColor", color * intensityMultiplier);
        }
    }

    public void SetColor(Color c)
    {
        color = c;
        if(active)
            material.SetColor("_EmissiveColor", color * intensityMultiplier);
    }

}
