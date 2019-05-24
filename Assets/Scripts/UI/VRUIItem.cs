using UnityEngine;

/// <summary>
/// @Author = Veli-Matti Vuoti
/// 
/// Handles the button Collider
/// </summary>
[RequireComponent(typeof(RectTransform))]
public class VRUIItem : MonoBehaviour
{
	private BoxCollider boxCollider;
	private RectTransform rectTransform;

	private void OnEnable()
	{
		ValidateCollider();
	}

	private void OnValidate()
	{
		ValidateCollider();
	}

	/// <summary>
	/// Resizes collider to match element's rectangle component size
	/// </summary>
	private void ValidateCollider()
	{
		rectTransform = GetComponent<RectTransform>();

		boxCollider = GetComponent<BoxCollider>();

		if (boxCollider == null)
		{
			boxCollider = gameObject.AddComponent<BoxCollider>();
		}

		boxCollider.size = rectTransform.sizeDelta;
	}
}