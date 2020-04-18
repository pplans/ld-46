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
	private World m_World;

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

    public virtual void UpdateCharacter()
    {
       if(!m_isAlive)
	   {
	   }
    }

    public bool IsAlive()
    {
        return m_isAlive;
    }

    public void Die()
    {
        Debug.Log("Character is dead");
        m_isAlive = false;
	}

	public void DoMove(Vector2 direction)
	{
		ITileInfo tileInfo = m_World.GetTileInfo(m_Position, direction);
		if (tileInfo.IsAvailable())
		{
			m_Position += direction;
		}
	}
    #endregion
}
