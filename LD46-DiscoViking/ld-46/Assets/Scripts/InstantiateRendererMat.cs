using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InstantiateRendererMat : MonoBehaviour
{
    void Awake()
    {
        Material mat;
        if (GetComponent<RectTransform>() != null)
        {
            mat = GetComponent<Image>().material;
            GetComponent<Image>().material = Instantiate(mat);
        }
        else
        {
            mat = GetComponent<Renderer>().material;
            GetComponent<Renderer>().material = Instantiate(mat);
        }
    }
}
