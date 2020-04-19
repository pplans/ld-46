using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DV_Enemy : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Animator>().Play("Drunk");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void WakeUp()
    {
        GetComponent<Animator>().SetTrigger("WakeUp");
    }
}
