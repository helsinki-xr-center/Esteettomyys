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

	public static GameMode gameMode;
	public static string user;
	public static bool loggedIn;
}
