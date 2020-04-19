using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DiscoTest : MonoBehaviour
{
    public DiscoController discoController;

    void Start()
    {

    }

    void Update()
    {
        if(UnityEngine.Input.GetKeyDown(KeyCode.UpArrow))
        {
            discoController.AddBoogie(+1);
            discoController.AddDisco(+1);
        }
        else if (UnityEngine.Input.GetKeyDown(KeyCode.DownArrow))
        {
            discoController.AddBoogie(-1);
            discoController.AddDisco(-1);
        }
        else if (UnityEngine.Input.GetKeyDown(KeyCode.LeftArrow))
        {
            //discoController.AddValhalla(-5);
        }
        else if (UnityEngine.Input.GetKeyDown(KeyCode.RightArrow))
        {
            //discoController.AddValhalla(+5);
        }
    }
}
