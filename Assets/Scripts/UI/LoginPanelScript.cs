using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

/**
 * Author: Nomi Lakkala
 * 
 * <summary>
 * Handles UI elements in the login panel.
 * </summary>
 */
public class LoginPanelScript : AwaitableUIPanel
{
	public TMP_InputField usernameInput;
	public TMP_InputField passwordInput;
	public Button loginButton;
	public bool success = false;

	public ILoginProvider loginProvider = new DummyLogin();


	private void Awake()
	{
		success = false;
	}

	/**
	 * <summary>
	 * Called from Unity UI Login button. Will start the login process.
	 * </summary>
	 */
	public async void LoginButtonClick()
	{
		string username = usernameInput.text;
		string password = passwordInput.text;
		loginButton.interactable = false;

		if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
		{
			new UIInfoMessage("Username and password fields must not be empty.", UIInfoMessage.MessageType.Info).Deliver();
			loginButton.interactable = true;
			return;
		}

		LoginResult result = await loginProvider.Login(username, password);

		switch (result)
		{
			case LoginResult.IncorrectCredentials:
				new UIInfoMessage("Incorrect login credentials.", UIInfoMessage.MessageType.Error).Deliver();
				loginButton.interactable = true;
				break;
			case LoginResult.Success:
				new UIInfoMessage("Login success!", UIInfoMessage.MessageType.Success).Deliver();

				GlobalValues.loggedIn = true;
				GlobalValues.user = username;

				success = true;
				break;
			case LoginResult.Error:
				new UIInfoMessage("An unknown error occurred.", UIInfoMessage.MessageType.Error).Deliver();
				loginButton.interactable = true;
				break;
			default:
				break;
		}
	}

	public override IEnumerator WaitForFinish()
	{
		yield return new WaitUntil(() => success);
	}
}
