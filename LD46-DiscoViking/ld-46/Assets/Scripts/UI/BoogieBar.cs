using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoogieBar : MonoBehaviour
{
    public BoogieLight[] lights;

    void Start()
    {

    }

    void Update()
    {
        
    }

    public void SetIntensityMultiplier(float multiplier)
    {
        for (int i = 0; i < lights.Length; ++i)
        {
            lights[i].intensityMultiplier = multiplier;
        }
    }

    public void SetBoogie(int boogie)
    {
        int n = Mathf.Min(boogie, lights.Length);

        for(int i = 0; i < n; ++i)
        {
            lights[i].SetActive(true);
        }

        for(int i = n; i < lights.Length; ++i)
        {
            lights[i].SetActive(false);
        }
    }

    public void SetColor(Color color)
    {
        for(int i = 0; i < lights.Length; ++i)
        {
            lights[i].SetColor(color);
        }
    }
}
