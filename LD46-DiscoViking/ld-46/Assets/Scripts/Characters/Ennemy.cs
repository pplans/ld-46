using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ennemy : Character
{
	#region Attributes
	UnityEngine.VFX.VisualEffect m_dancingImpactVFX;
	float m_timerPlay = 0f;
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
	public void SetDancingImpactVFX(UnityEngine.VFX.VisualEffect _vfx) { m_dancingImpactVFX = _vfx; }
	public override bool IsIA() { return true; }
	public void StartDancingImpactVFX()
	{
		m_timerPlay = 5f;
		if(m_dancingImpactVFX)
			m_dancingImpactVFX.Play();
	}
	public void Update()
	{
		float dt = Time.deltaTime;
		float currDt = m_timerPlay - dt;
		if (currDt < 0f && m_timerPlay>0f && m_dancingImpactVFX)
			m_dancingImpactVFX.Stop();

		m_timerPlay -= dt;
	}
	#endregion
}
