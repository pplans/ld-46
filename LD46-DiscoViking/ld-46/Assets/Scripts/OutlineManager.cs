using UnityEngine;
using UnityEngine.Rendering;
using System.Collections;
using System.Collections.Generic;

public class OutlineManager
{
	static OutlineManager m_Instance; // singleton
	static public OutlineManager instance
	{
		get
		{
			if (m_Instance == null)
				m_Instance = new OutlineManager();
			return m_Instance;
		}
	}

	// list of glow objects
	internal HashSet<GameObject> m_GlowObjs = new HashSet<GameObject>();

	public void Add(GameObject o)
	{
		Remove(o);
		m_GlowObjs.Add(o);
		//Debug.Log("added effect " + o.gameObject.name);
	}

	public void Remove(GameObject o)
	{
		m_GlowObjs.Remove(o);
		//Debug.Log("removed effect " + o.gameObject.name);
	}
}