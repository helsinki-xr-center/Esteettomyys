using System.Collections;
using System.Collections.Generic;
using UnityEngine;



/**
 * Author: Nomi Lakkala
 * 
 * Contains global values for this game session.
 */
public static class GlobalValues
{
	public enum GameMode
	{
		VR,
		PC
	}

	public static GameMode gameMode = GameMode.PC;
	public static string user = "TestUser";
	public static bool loggedIn;
}


public static class ConstStringKeys
{

	public const string PUN_PLAYER_READY = "playerReady";
}
