using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DV_EnemyAnimation : MonoBehaviour
{
    public Animator anim;
    public bool bWokenUp;

    // Start is called before the first frame update
    void Start()
    {
        anim.Play("Drunk");
        bWokenUp = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void WakeUp()
    {
        anim.SetTrigger("WakeUp");
        bWokenUp = true;
    }
}
