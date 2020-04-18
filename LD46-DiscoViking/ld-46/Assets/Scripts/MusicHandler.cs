using UnityEngine;
using UnityEngine.UI;

public class MusicHandler : MonoBehaviour
{
	public AudioSource	music;

	public float bpm;
	public float bpmOffset;

	public float inputValidity;

	public bool started = false;

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
