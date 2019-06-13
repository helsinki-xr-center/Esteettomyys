using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AreaUiItem : MonoBehaviour
{
	public Image image;
	public Image selectedImage;
	public Button button;
	public TextMeshProUGUI nameText;
	public TextMeshProUGUI missionAmountText;
	public bool selected;

	private AreaScriptableObject area;

	public void SetValues(AreaScriptableObject area, string displayName, int missionsDone, int totalMissions, Sprite sprite, bool accessible)
	{
		this.area = area;
		nameText.SetText(displayName);
		missionAmountText.SetText($"{missionsDone}/{totalMissions}");
		image.overrideSprite = sprite;
		button.interactable = accessible;
		selectedImage.gameObject.SetActive(false);
	}

	public void Select()
	{
		foreach(var uiItem in FindObjectsOfType<AreaUiItem>())
		{
			uiItem.Deselect();
		}

		selected = true;
		selectedImage.gameObject.SetActive(true);

		GlobalValues.startingArea = area;
	}

	public void Deselect()
	{
		selected = false;
		selectedImage.gameObject.SetActive(false);
	}

	public void OnButtonPressed()
	{
		Select();
	}
}
