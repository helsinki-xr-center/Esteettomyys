﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// @Author = Veli-Matti Vuoti
/// </summary>
[RequireComponent(typeof(Movement),typeof(PlayerInput))]
public class PcPlayer : MonoBehaviour
{

	public Movement movement;
	public PcCamera playerEyes;

	private void Awake()
	{
		movement = gameObject.GetComponent<Movement>();
		playerEyes = gameObject.GetComponentInChildren<PcCamera>();
	}

	private void Update()
	{
		
	}

}
