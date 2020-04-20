using UnityEngine;
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

		if (Time.unscaledTime < bpmOffset)
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
		bpmOffset += Time.unscaledTime;
		music.Play();
		started = true;
	}

	public void StopMusic()
	{
		music.Stop();
		started = false;
	}

	public float GetBeatOffset()
	{
		if (!started)
			return -0.5f;

		float timeValue = Time.unscaledTime - bpmOffset;

		timeValue *= bpm / 60;

		return timeValue - Mathf.Round(timeValue);
	}

	public bool ValidateBeat()
	{
		return Mathf.Abs(GetBeatOffset()) <= inputValidity;
	}
}
