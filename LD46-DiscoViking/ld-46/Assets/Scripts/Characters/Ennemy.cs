using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ennemy : Character
{
	#region UnityEvents
	public override void Awake()
	{
	}

	public void Start()
	{
	}
	#endregion

	#region Methods
	public override bool IsIA() { return true; }
	#endregion
}
