using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;



/**
 * Author: Nomi Lakkala
 * 
 * <summary>
 * Dummy login class that accepts any username with "password" as username and password
 * </summary>
 */
public class DummyLogin : ILoginProvider
{
	public async Task<LoginResult> Login(string username, string password)
	{
		if (password == "password")
		{
			return LoginResult.Success;
		}
		else
		{
			return LoginResult.IncorrectCredentials;
		}
	}
}
