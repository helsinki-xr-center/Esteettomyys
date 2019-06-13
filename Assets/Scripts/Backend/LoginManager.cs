using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

/**
 * Author: Nomi Lakkala
 * 
 * <summary>
 * The central login manager. When logging in or logging out, this class should be used instead of the individual login providers.
 * </summary>
 */
public static class LoginManager
{
	private static ILoginProvider loginProvider = new DummyLogin();
	public static string user = "TestUser";
	public static bool loggedIn;
	public static bool offlineMode;

	/**
	 * <summary>
	 * Logs in with the default <see cref="ILoginProvider"/>
	 * </summary>
	 */
	public static async Task<LoginResult> Login(string username, string password)
	{
		
		var result = await loginProvider.Login(username, password);

		if(result == LoginResult.Success)
		{
			user = username;
			loggedIn = true;
			offlineMode = false;
		}

		return result;
	}

	/**
	 * <summary>
	 * Sets the game to offline mode.
	 * </summary>
	 */
	public static void StartOffline()
	{
		loggedIn = false;
		user = "offline user";
		offlineMode = true;
	}

	/**
	 * <summary>
	 * Logs out with the default <see cref="ILoginProvider"/>
	 * </summary>
	 */
	public static void Logout()
	{
		loggedIn = false;
		offlineMode = false;
		_ = loginProvider.Logout();
	}
}
