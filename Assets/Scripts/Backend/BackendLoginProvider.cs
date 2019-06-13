using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class BackendLoginProvider : ILoginProvider
{
	public Task<LoginResult> Login(string username, string password)
	{
		return BackendConnector.Login(new Credentials() { username = username, password = password });
	}

	public Task Logout()
	{
		return BackendConnector.Logout();
	}
}
