using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
	public Toggle SkipTutorials = null;

    // Start is called before the first frame update
    void Start()
    {
        if(SkipTutorials)
		{
			SkipTutorials.isOn = DoSkipTutorials();
		}
    }
	
	public void OnChangeSkipTuTorials(Toggle change)
	{
		if (SkipTutorials)
		{
			ApplySkipTutorials(change.isOn);
		}
	}
	public static void ApplySkipTutorials(bool b) { PlayerPrefs.SetInt("SkipTutorials",b?1:0); }
	public static bool DoSkipTutorials() { return PlayerPrefs.GetInt("SkipTutorials", 0) == 1; }
}
