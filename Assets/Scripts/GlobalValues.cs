using System.Collections;
using System.Collections.Generic;
using UnityEngine;



/**
 * Author: Nomi Lakkala
 * <summary>
 * Contains global values for this game session.
 * </summary>
 */
public static class GlobalValues
{

	public static GameMode gameMode = GameMode.Tutorial;
	public static ControllerMode controllerMode = ControllerMode.PC;
	public static string user { get{ return LoginManager.user; } }
	public static bool loggedIn { get { return LoginManager.loggedIn; } }
	public static bool offlineMode { get { return LoginManager.offlineMode; } }

	public static AreaScriptableObject startingArea;


	public static Settings settings
	{
		get => Settings.Get();
	}
}


public static class ConstStringKeys
{

	public const string PUN_PLAYER_READY = "playerReady";
	public const string PUN_MATCH_START = "matchStart";
}
