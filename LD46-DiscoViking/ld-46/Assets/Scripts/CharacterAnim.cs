using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnim : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Animator>().Play("DiscoUp");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AnimationStep()
    {
        GetComponent<Animator>().SetTrigger("Step");
    }
}
