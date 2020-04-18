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

	private void Start()
	{
		action.performed += InputLink;
		action.Enable();
		bpmOffset += Time.time;
		music.Play();
	}
	private void InputLink(InputAction.CallbackContext callbackContext)
	{
		Debug.Log(ValidateBeat());
	}

	private void Update()
	{
		group.alpha = curve.Evaluate( 1f - ( Mathf.Abs( GetBeatOffset() ) * 4f ) );
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
