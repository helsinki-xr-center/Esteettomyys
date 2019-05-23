
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Valve.VR.InteractionSystem;

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
	}

	public void HandlePointerHit(object sender, NewRayCastData hitInfo)
	{
		Button button = hitInfo.target.GetComponent<Button>();
		if(button != null)
		{
			button.Select();
		}
		Debug.Log("POINTER HITS");
	}

	public void HandlePointerLeft(object sender, NewRayCastData hitInfo)
	{
		Button button = hitInfo.target.GetComponent<Button>();
		if (button != null)
		{
			EventSystem.current.SetSelectedGameObject(null);
		}
		Debug.Log("POINTER LEFT");
	}

	public void HandlePointerClick(object sender, NewRayCastData hitInfo)
	{
		Debug.Log("CLICKED");
		if(EventSystem.current.currentSelectedGameObject != null)
		{
			ExecuteEvents.Execute(EventSystem.current.currentSelectedGameObject, new PointerEventData(EventSystem.current), ExecuteEvents.submitHandler);
		}
	}
}

