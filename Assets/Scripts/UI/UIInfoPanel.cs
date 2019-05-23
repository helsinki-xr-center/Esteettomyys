using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;



/**
 * Author: Nomi Lakkala
 * 
 * <summary>
 * Listens for UIInfoMessages and shows a UI box with the message.
 * </summary>
 */
public class UIInfoPanel : MonoBehaviour
{
	public Graphic colorTarget;
	public GameObject messageBoxObject;
	public TextMeshProUGUI messageBoxText;

	public Color infoColor = Color.white;
	public Color successColor = Color.green;
	public Color errorColor = Color.red;

	private void Start() => messageBoxObject.SetActive(false);

	private void OnEnable() => UIInfoMessage.AddListener(ShowMessage);

	private void OnDisable() => UIInfoMessage.RemoveListener(ShowMessage);


	/**
	 * <summary>
	 * Called by invoking UIInfoMessage.Deliver(). Shows the message on the UI with certain background color.
	 * </summary>
	 */
	public void ShowMessage(UIInfoMessage message)
	{
		if (messageBoxObject.activeSelf)
		{
			//TODO: implement message Queue?
			return;
		}

		messageBoxObject.SetActive(true);

		switch (message.messageType)
		{
			case UIInfoMessage.MessageType.Info:
				colorTarget.color = infoColor;
				break;
			case UIInfoMessage.MessageType.Success:
				colorTarget.color = successColor;
				break;
			case UIInfoMessage.MessageType.Error:
				colorTarget.color = errorColor;
				break;
			default:
				break;
		}

		messageBoxText.SetText(message.message);
	}
}
