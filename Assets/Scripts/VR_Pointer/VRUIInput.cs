
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Valve.VR.InteractionSystem;

/// <summary>
/// @Author = Veli-Matti Vuoti
/// 
/// Handles Button Clicks for Pointer
/// </summary>
public class VRUIInput : MonoBehaviour
{

	private void OnEnable()
	{
		Pointer.PointerHit -= HandlePointerHit;
		Pointer.PointerHit += HandlePointerHit;
		Pointer.PointerLeft -= HandlePointerLeft;
		Pointer.PointerLeft += HandlePointerLeft;
		Pointer.PointerClick -= HandlePointerClick;
		Pointer.PointerClick += HandlePointerClick;
		Pointer.PointerDrag -= HandlePointerDrag;
		Pointer.PointerDrag += HandlePointerDrag;
	}

	/// <summary>
	/// Handles Pointer Hit Event On UI Button
	/// </summary>
	/// <param name="sender">Class that sends the data </param>
	/// <param name="hitInfo">Information of the Raycast hit </param>
	public void HandlePointerHit(object sender, RayCastData hitInfo)
	{

		Button button = hitInfo.target.GetComponent<Button>();

		Slider slider = hitInfo.target.GetComponent<Slider>();

		Dropdown dropdown = hitInfo.target.GetComponent<Dropdown>();

		ScrollRect scrollRect = hitInfo.target.GetComponent<ScrollRect>();

		if (button != null)
		{
			button.Select();
		}
		else if (slider != null)
		{
			slider.Select();
		}
		else if (dropdown != null)
		{
			dropdown.Select();
		}
		else if (scrollRect != null)
		{
			EventSystem.current.SetSelectedGameObject(hitInfo.target.gameObject);
		}
		else
		{
			EventSystem.current.SetSelectedGameObject(null);
		}

	}

	/// <summary>
	/// Handles Pointer Left Event On UI Button
	/// </summary>
	/// <param name="sender"> Class that sends the data </param>
	/// <param name="hitInfo"> Information of the Raycast hit </param>
	public void HandlePointerLeft(object sender, RayCastData hitInfo)
	{

		Button button = hitInfo.target.GetComponent<Button>();

		Slider slider = hitInfo.target.GetComponent<Slider>();

		Dropdown dropdown = hitInfo.target.GetComponent<Dropdown>();

		ScrollRect scrollRect = hitInfo.target.GetComponent<ScrollRect>();

		if (button != null)
		{
			EventSystem.current.SetSelectedGameObject(null);
		}	
		else if (slider != null)
		{
			EventSystem.current.SetSelectedGameObject(null);
		}	
		else if (dropdown != null)
		{
			EventSystem.current.SetSelectedGameObject(null);
		}
		else if (scrollRect != null)
		{
			EventSystem.current.SetSelectedGameObject(null);
		}
		else
		{
			EventSystem.current.SetSelectedGameObject(null);
		}
	}

	/// <summary>
	/// Handles Pointer Click Event on UI Button
	/// </summary>
	/// <param name="sender"> Class that sends the data </param>
	/// <param name="hitInfo"> Information of the Raycast hit </param>
	public void HandlePointerClick(object sender, RayCastData hitInfo)
	{

		//Debug.Log("CLICKED");
		if (hitInfo.target.GetComponent<Button>() != null)
		{
			if (EventSystem.current.currentSelectedGameObject != null)
			{
				ExecuteEvents.Execute(EventSystem.current.currentSelectedGameObject, new PointerEventData(EventSystem.current), ExecuteEvents.submitHandler);
			}
		}
		if (hitInfo.target.GetComponent<Slider>() != null)
		{
			if (EventSystem.current.currentSelectedGameObject != null)
			{
				PointerEventData eventData = new PointerEventData(EventSystem.current);
				eventData.dragging = true;
				eventData.position = hitInfo.hitPoint;

				ExecuteEvents.Execute(EventSystem.current.currentSelectedGameObject, eventData, ExecuteEvents.dragHandler);

			}
		}

		if (hitInfo.target.GetComponent<Dropdown>())
		{
			if (EventSystem.current.currentSelectedGameObject != null)
			{
				PointerEventData eventData = new PointerEventData(EventSystem.current);
				eventData.pressPosition = hitInfo.hitPoint;

				ExecuteEvents.Execute(EventSystem.current.currentSelectedGameObject, eventData, ExecuteEvents.submitHandler);
			}
		}
	}

	public void HandlePointerDrag(object sender, RayCastData hitInfo)
	{

		if (hitInfo.target.GetComponent<Slider>() != null)
		{
			if (EventSystem.current.currentSelectedGameObject != null)
			{
				PointerEventData eventData = new PointerEventData(EventSystem.current);
				eventData.dragging = true;
				eventData.position = hitInfo.hitPoint;

				ExecuteEvents.Execute(EventSystem.current.currentSelectedGameObject, eventData, ExecuteEvents.dragHandler);

			}
		}
		if (hitInfo.target.GetComponent<ScrollRect>() != null)
		{
			Debug.Log(EventSystem.current.currentSelectedGameObject);
			PointerEventData eventData = new PointerEventData(EventSystem.current);
			eventData.dragging = true;
			eventData.position = hitInfo.hitPoint;
			ScrollRect scrollRect = hitInfo.target.GetComponent<ScrollRect>();
			scrollRect.OnBeginDrag(eventData);
			scrollRect.OnDrag(eventData);
			//ExecuteEvents.Execute(hitInfo.target.gameObject, eventData, ExecuteEvents.submitHandler);
			ExecuteEvents.Execute(EventSystem.current.currentSelectedGameObject, eventData, ExecuteEvents.dragHandler);

		}
	}
}


