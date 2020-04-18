using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class TestMusicHandler : MonoBehaviour
{
	public InputAction	action;
	public MusicHandler musicHandler;
	public CanvasGroup	metronome;
	public CanvasGroup	player;
	public Image		playerImage;
	public Text			text;

	public AnimationCurve	curve;

	private void Start()
	{
		action.performed += InputLink;
		action.Enable();
	}

	private void InputLink(InputAction.CallbackContext callbackContext)
	{
		if (!musicHandler.started)
			musicHandler.StartMusic();

	//	player.alpha = 1f;
	//	playerImage.color = musicHandler.ValidateBeat() ? Color.green : Color.red;
	//	text.text = musicHandler.GetBeatOffset().ToString();
	}

	private void Update()
	{
	//	if (!musicHandler.started)
	//		return;

	//	float beatValue = musicHandler.GetBeatOffset() + 0.5f;
	//	metronome.alpha = curve.Evaluate(beatValue);
	//	beatValue = 2f - 2f * beatValue;
	//	if (player.alpha > beatValue)
	//		player.alpha = beatValue;
	}
}
