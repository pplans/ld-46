using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicEffect : MonoBehaviour
{
	public AudioSource music;
	public AudioSource beep;
	public MusicHandler musicHandler;

	public float ratio = 1f;
	public bool tutoMode = false;

	private void Start()
	{
		musicHandler.OnBeat.AddListener(PlayBeep);
	}

	private void Update()
	{
		if (tutoMode)
		{
			music.volume = 1f;
			beep.volume = 1f;
		}
		else
		{
			music.volume = ratio;
			beep.volume = 1f - ratio;
		}
	}

	private void PlayBeep()
	{
		beep.Stop();
		beep.Play();
	}
}
