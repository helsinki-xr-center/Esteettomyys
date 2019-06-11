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

	private void OnEnable()
	{
		success = false;
		loginButton.interactable = true;
	}

	/**
	 * <summary>
	 * Called from Unity UI Login button. Will start the login process.
	 * </summary>
	 */
	public async void OnLoginButtonClick()
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

		LoginResult result = await LoginManager.Login(username, password);

		switch (result)
		{
			case LoginResult.IncorrectCredentials:
				new UIInfoMessage("Incorrect login credentials. Hint: try using \"password\"", UIInfoMessage.MessageType.Error).Deliver();
				loginButton.interactable = true;
				break;
			case LoginResult.Success:
				new UIInfoMessage("Login success!", UIInfoMessage.MessageType.Success).Deliver();
				success = true;
				loginButton.interactable = true;
				break;
			case LoginResult.Error:
				new UIInfoMessage("An unknown error occurred.", UIInfoMessage.MessageType.Error).Deliver();
				loginButton.interactable = true;
				break;
			default:
				break;
		}
	}

	public void OnOfflineButtonClick(){
		LoginManager.StartOffline();
		success = true;
	}

	public override IEnumerator WaitForFinish()
	{
		yield return new WaitUntil(() => success);
	}
}
