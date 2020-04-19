using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : WorldObject
{
	#region Members

	protected bool m_isAlive;

	// World Position
	[SerializeField]
	private Vector2 m_Position;
	[SerializeField]
	private World m_World = null;

	#endregion

	#region UnityEvents

	public virtual void Awake()
	{
		m_isAlive = true;
	}

    #endregion

    #region Methods

	public Character()
	{
		m_isAlive = true;
	}

	~Character()
	{
		m_isAlive = false;
	}

	public void Init(Vector2 position, World world)
	{
		m_Position = position;
		world.SetWorldAnchor(m_Position);
		world.SetObject(this, m_Position);
	}

	public virtual void UpdateCharacter()
	{
       if(!m_isAlive)
	   {
	   }
    }

	public Vector2 Position { get { return m_Position; } set { m_Position = value; } }

    public bool IsAlive()
    {
        return m_isAlive;
    }

    public void Die()
    {
        Debug.Log("Character is dead");
        m_isAlive = false;
	}

	public TileState GetTileState(Vector2 direction)
	{
		return m_World.GetTileInfo(m_Position+direction).GetState();
	}

	public void ResetPosition(TileState state)
	{
		if (state == TileState.BorderRight)
		{
			m_Position.x = 0f;
			m_World.SetWorldAnchor(m_Position);
			m_World.SetObject(this, m_Position);
		}
	}

	public ITileInfo DoMove(Vector2 direction)
	{
		ITileInfo tileInfo = m_World.GetTileInfo(m_Position + direction);
		if (tileInfo.GetState() == TileState.BorderRight)
		{
			m_World.UseCache(Random.Range(0, m_World.GetCacheSize()));
			m_Position.x = 0;
			m_World.SetObject(this, m_Position);
			return tileInfo;
		} else if (tileInfo.GetState() == TileState.Occupied || tileInfo.GetState()==TileState.Ennemy)
        {
			return tileInfo;
        }
		else if(tileInfo.GetState() == TileState.Border)
		{
			return tileInfo;
		}
		TileState tileState = m_World.MoveObject(m_Position, direction);
		m_Position += direction;
		return tileInfo;
	}
    #endregion
}
