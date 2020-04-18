using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class MusicHandler : MonoBehaviour
{
	public AudioSource music;
	public InputAction action;
	public CanvasGroup group;
	public AnimationCurve curve;

	public float bpm;
	public float bpmOffset;

	public float inputValidity;

	private bool started = false;

	private void Start()
	{
		action.performed += InputLink;
		action.Enable();
	}

	private void InputLink(InputAction.CallbackContext callbackContext)
	{
		if (!started)
			StartMusic();
		Debug.Log(ValidateBeat());
	}

	private void Update()
	{
		if (!started)
			return;
		group.alpha = ValidateBeat() ? 1f : 0f;
	}

	private void StartMusic()
	{
		bpmOffset += Time.time;
		music.Play();
		started = true;
	}

	private float GetBeatOffset()
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
