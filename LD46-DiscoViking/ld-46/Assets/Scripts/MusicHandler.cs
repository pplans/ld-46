﻿using UnityEngine;
using UnityEngine.Events;

public class MusicHandler : MonoBehaviour
{
	public AudioSource	music;

	public float bpm;
	public float bpmOffset;

	public float inputValidity;

	public bool started = false;

	public UnityEvent OnResetBeat;
	public UnityEvent OnBeat;
	public UnityEvent OnBeatInvalid;

	private float previousBeat = 1f;

	private void Update()
	{
		if (!started)
			return;

		if (Time.time < bpmOffset)
			return;

		// check OnBeat
		float newBeat = GetBeatOffset();
		float newSign = Mathf.Sign(newBeat);
		if (newSign != Mathf.Sign(previousBeat))
		{
			if (newSign == 1f)
				OnBeat.Invoke();
			else
				OnResetBeat.Invoke();
		}
		if (previousBeat < inputValidity && newBeat > inputValidity)
			OnBeatInvalid.Invoke();

		previousBeat = newBeat;
	}

	public void StartMusic()
	{
		bpmOffset += Time.time;
		music.Play();
		started = true;
	}

	public float GetBeatOffset()
	{
		float timeValue = Time.time - bpmOffset;

		timeValue *= bpm / 60;

		return timeValue - Mathf.Round(timeValue);
	}

	public bool ValidateBeat()
	{
		return Mathf.Abs(GetBeatOffset()) <= inputValidity;
	}
}