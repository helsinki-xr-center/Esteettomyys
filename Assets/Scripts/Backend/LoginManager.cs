using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public static class LoginManager
{
	private static ILoginProvider loginProvider = new DummyLogin();
	public static string user = "TestUser";
	public static bool loggedIn;
	public static bool offlineMode;

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

	public static void StartOffline()
	{
		loggedIn = false;
		user = "offline user";
		offlineMode = true;
	}

	public static void Logout()
	{
		loggedIn = false;
		offlineMode = false;
		_ = loginProvider.Logout();
	}
}
