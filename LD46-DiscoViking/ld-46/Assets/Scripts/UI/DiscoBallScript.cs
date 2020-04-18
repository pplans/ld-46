using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiscoBallScript : MonoBehaviour
{
    public float rotateSpeed = 15.0f;

    void Start()
    {
        
    }

    void Update()
    {
        transform.Rotate(Vector3.up * rotateSpeed * Time.deltaTime, Space.Self);
    }
}
