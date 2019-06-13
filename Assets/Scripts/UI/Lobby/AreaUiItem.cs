using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


/**
 * Author: Nomi Lakkala
 * 
 * <summary>
 * Attached to the AreaUIItems spawned by <see cref="TrainingAreaSelectorPanel"/>.
 * </summary>
*/
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

	/**
	 * <summary>
	 * Selects this area as the starting area. Deselects all other AreaUIItems.
	 * </summary>
	 */
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

	/**
	 * <summary>
	 * Deselects this area.
	 * </summary>
	 */
	public void Deselect()
	{
		selected = false;
		selectedImage.gameObject.SetActive(false);
	}

	/**
	 * <summary>
	 * Called from unity Button.
	 * </summary>
	 */
	public void OnButtonPressed()
	{
		Select();
	}
}
