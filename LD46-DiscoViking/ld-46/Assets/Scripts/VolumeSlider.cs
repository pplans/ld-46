using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{
	public AudioMixer	mixer;
	public Slider		slider;

	private void Awake()
	{
		//SetVolume();
		slider.value = PlayerPrefs.GetFloat("MusicVolume", 0.5f);
	}

	public void SetVolume(float newVolume)
	{
		PlayerPrefs.SetFloat("MusicVolume", newVolume);
		mixer.SetFloat("MusicVolume", Mathf.Log10(newVolume) * 20);
	}
}
