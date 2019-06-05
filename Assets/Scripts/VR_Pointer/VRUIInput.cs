
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

		//if (hitInfo.target.GetComponent<Button>() != null)
		//{
		Button button = hitInfo.target.GetComponent<Button>();
		if (button != null)
		{
			button.Select();
		}
		//}
		//if (hitInfo.target.GetComponent<Slider>() != null)
		//{
		Slider slider = hitInfo.target.GetComponent<Slider>();
		if (slider != null)
		{
			slider.Select();
		}
		//}

		/*if(hitInfo.target.GetComponent<Dropdown>() != null)
		{*/
		Dropdown dropdown = hitInfo.target.GetComponent<Dropdown>();
		if (dropdown != null)
		{
			dropdown.Select();
		}

		ScrollRect scrollRect = hitInfo.target.GetComponent<ScrollRect>();
		if (scrollRect != null)
		{
			EventSystem.current.SetSelectedGameObject(hitInfo.target.gameObject);
		}

		//ScrollRect scrollrect = hitInfo.target.GetComponent<ScrollRect>();
		//if (scrollrect != null)
		//{
		//	PointerEventData pointerEvent = new PointerEventData(EventSystem.current);
		//	pointerEvent.position = hitInfo.hitPoint;
		//	scrollrect.OnScroll(pointerEvent);
		//}

		//}
		//Debug.Log("POINTER HITS");
	}

	/// <summary>
	/// Handles Pointer Left Event On UI Button
	/// </summary>
	/// <param name="sender"> Class that sends the data </param>
	/// <param name="hitInfo"> Information of the Raycast hit </param>
	public void HandlePointerLeft(object sender, RayCastData hitInfo)
	{

		Button button = hitInfo.target.GetComponent<Button>();
		if (button != null)
		{
			EventSystem.current.SetSelectedGameObject(null);
		}


		Slider slider = hitInfo.target.GetComponent<Slider>();
		if (slider != null)
		{
			EventSystem.current.SetSelectedGameObject(null);
		}



		Dropdown dropdown = hitInfo.target.GetComponent<Dropdown>();
		if (dropdown != null)
		{
			EventSystem.current.SetSelectedGameObject(null);
		}

		ScrollRect scrollRect = hitInfo.target.GetComponent<ScrollRect>();
		if(scrollRect != null)
		{
			EventSystem.current.SetSelectedGameObject(null);
		}

		//Debug.Log("POINTER LEFT");
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


