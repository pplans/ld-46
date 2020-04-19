using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ennemy : Character
{
	#region Attributes
	[SerializeField]
	UnityEngine.VFX.VisualEffect m_dancingImpactVFX = null;
	float m_timerPlay = 0f;
	public float m_dancingImpactVFXDuration = 5f;
	#endregion

	#region UnityEvents
	public override void Awake()
	{
		m_timerPlay = 0f;
	}

	public void Start()
	{
		m_timerPlay = 0f;
	}
	#endregion

	#region Methods
	public override bool IsIA() { return true; }
	public void StartDancingImpactVFX()
	{
		m_timerPlay = m_dancingImpactVFXDuration;
		if(m_dancingImpactVFX)
			m_dancingImpactVFX.SendEvent("Start");
	}
	public void Update()
	{
		float dt = Time.deltaTime;
		float currDt = m_timerPlay - dt;
		if (currDt < 0f && m_timerPlay>0f && m_dancingImpactVFX)
			m_dancingImpactVFX.SendEvent("Stop");

		m_timerPlay -= dt;
	}
	#endregion
}
