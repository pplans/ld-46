using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Input
{
	enum Type
	{
		None = 0x0,
		Down = 0x1,
		Up = 0x2,
		Hold = 0x3
	};

	enum Action
	{
		Action1 = 0,
		Action2 = 1,
		Action3 = 2,
		Up = 3,
		Down = 4,
		Left = 5,
		Right = 6
	}

	struct ActionEntry
	{
		private Action action;
		private KeyCode code;
		private string name;
		private Type type;
		public Action Action
		{
			get { return action; }
			set { }
		}
		public KeyCode Code
		{
			get { return code; }
			set { }
		}
		public string Name
		{
			get { return name; }
			set { }
		}
		public Type Type
		{
			get { return type; }
			set { type = value; }
		}
		public ActionEntry(Action a, KeyCode c, string s, Type t)
		{
			action = a; code = c; name = s; type = t;
		}
	};

	static class Mapper
	{
		public static ActionEntry[] Actions =
		{
			new ActionEntry(Action.Action1, KeyCode.X, "Action", Type.None),
			new ActionEntry(Action.Action2, KeyCode.C, "Action2", Type.None),
			new ActionEntry(Action.Action3, KeyCode.V, "Action3", Type.None),
			new ActionEntry(Action.Up, KeyCode.UpArrow, "Up", Type.None),
			new ActionEntry(Action.Down, KeyCode.DownArrow, "Down", Type.None),
			new ActionEntry(Action.Left, KeyCode.LeftArrow, "Left", Type.None),
			new ActionEntry(Action.Right, KeyCode.RightArrow, "Right", Type.None)
		};

		public static Type IsPressed(Action a)
		{
			return Actions[(int)a].Type;
		}
	}
}

public class GameImpl : Game
{
	public InputAction action;

	public LoadLevel loadLevel;

	public List<Character> m_characters;
    bool levelComplete = false;

    public Player m_player = null;
	public PlayerController m_playerController = null;
	public World m_world = null;
	[SerializeField]
	public WorldObject m_testLamp = null;
	[SerializeField]
	private Vector2 TileStartPos = new Vector2(0, 0);
	[SerializeField]
	private Vector2 TileEndPos = new Vector2(10, 4);
	[SerializeField]
	private Vector2 TileSize = new Vector2(1, 1);
	[SerializeField]
	private Vector2 PlayerInitPos = new Vector2(3, 1);

	const float m_playerMovingSpeed = 5f; // per second

	public void Start()
	{
		action.performed += InputPlayer;
		action.Enable();
	
		m_world.Init(TileStartPos, TileEndPos, TileSize);

		m_world.PlaceObject(m_testLamp);
		m_player.Init(PlayerInitPos);
		m_world.SetWorldAnchor(PlayerInitPos);
		m_world.SetObject(m_player, PlayerInitPos);
	}

	private void InputPlayer(InputAction.CallbackContext callbackContext)
	{
		Vector2 newDirection = callbackContext.action.ReadValue<Vector2>();
		TileState tileState = m_player.GetTileState(newDirection);
		if(tileState==TileState.BorderRight)
		{
			m_world.UseCache(Random.Range(0, m_world.GetCacheSize()));
			// Load next grid
			m_player.ResetPosition(tileState);
		}
		else
			m_player.DoMove(newDirection, tileState);
	}

	public override void UpdateGame()
	{
		// update dudes
		m_player.UpdateCharacter();

		if (m_player.IsAlive() == false)
		{
			loadLevel.LoadLevelX(0);
		}

        if (levelComplete)
        {
            Debug.Log("Level Complete");
        }

		foreach (Character p in m_characters)
		{
			p.UpdateCharacter();
		}
	}
}
