using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InstantiateRendererMat : MonoBehaviour
{
    void Awake()
    {
        GetComponent<Image>().material = Instantiate<Material>(GetComponent<Image>().material);
    }
}
