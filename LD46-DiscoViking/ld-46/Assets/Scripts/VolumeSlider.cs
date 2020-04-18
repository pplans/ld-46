using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class VolumeSlider : MonoBehaviour
{
	public AudioMixer	mixer;
	[Range(0.0001f, 1f)]
	public float		volume = 0.5f;

	public void SetVolume(float newVolume)
	{
		mixer.SetFloat("MusicVolume", Mathf.Log10(newVolume) * 20);
	}
}
