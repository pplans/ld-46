using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class MusicHandler : MonoBehaviour
{
	public InputAction	action;
	public AudioSource	music;
	public CanvasGroup	metronome;
	public CanvasGroup	player;
	public Image		playerImage;
	public Text			text;

	public AnimationCurve	curve;

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

		player.alpha = 1f;
		playerImage.color = ValidateBeat() ? Color.green : Color.red;
		text.text = GetBeatOffset().ToString();
	}

	private void Update()
	{
		if (!started)
			return;

		float beatValue = GetBeatOffset() + 0.5f;
		metronome.alpha = curve.Evaluate(beatValue);
		beatValue = 2f - 2f * beatValue;
		if (player.alpha > beatValue)
			player.alpha = beatValue;
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
