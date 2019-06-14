using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Valve.VR;
using System.Linq;

/// <summary>
/// This class controls the states for tablet, uses list of empty gameobjects for positioning. 
/// Also need reference to player and vrcamera.
/// </summary>
public class TabletStatePattern : MonoBehaviour
{

	public TabletStateID tabletState;
	public bool manuallySetPositions;

	public float speed;
	public float deactivateTime;

	public LayerMask hitMask;
	public Transform playerT;
	public Transform vrCamera;
	bool pressed;
	bool changedMode;

	Transform startTransform;
	float lerpStartTime;
	bool lerping;
	float lerpDistance;
	public float stopLerpDistance;
	float tick;

	public TabletStateID previousState;
	public ITabletState currentState;
	public HoldState holdState;
	public FollowState followState;
	public FrontOfControllerState frontOfControllerState;
	public FrontOfHMDState frontOfHMDState;
	public FollowSideState followSideState;

	public Dictionary<TabletStateID, ITabletState> states = new Dictionary<TabletStateID, ITabletState>();

	[Header("0 : Front, 1 : Back, 2 : Left, 3 : LeftController, 4 : RightController")]
	/// <summary>
	/// 0 = Front,
	/// 1 = Back,
	/// 2 = Left,
	/// 3 = LeftController,
	/// 4 = RightController,
	/// </summary>
	public Transform[] positions;

	public SteamVR_Action_Boolean grabPinch;
	public SteamVR_Action_Boolean grabGrib;
	public SteamVR_Action_Vector2 touch;

	private void Awake()
	{
		holdState = new HoldState(this);
		followState = new FollowState(this);
		frontOfControllerState = new FrontOfControllerState(this);
		frontOfHMDState = new FrontOfHMDState(this);
		followSideState = new FollowSideState(this);
	}

	private void Start()
	{
		playerT = GameObject.FindGameObjectWithTag("Player").transform;
		vrCamera = playerT.GetChild(0).GetChild(3).transform;

		if (!manuallySetPositions)
		{
			positions[0] = vrCamera.GetChild(0).transform;
			positions[1] = vrCamera.GetChild(1).transform;
			positions[2] = vrCamera.GetChild(2).transform;
			positions[3] = playerT.GetChild(0).GetChild(1).GetChild(4).transform;
			positions[4] = playerT.GetChild(0).GetChild(2).GetChild(4).transform;
		}

		states.Add(TabletStateID.Follow, followState);
		states.Add(TabletStateID.FollowSide, followSideState);
		states.Add(TabletStateID.FrontController, frontOfControllerState);
		states.Add(TabletStateID.FrontHMD, frontOfHMDState);
		states.Add(TabletStateID.Hold, holdState);
		
		currentState = states[tabletState];
	}

	private void Update()
	{
		currentState.UpdateState();
		//StateChangeByIDForTesting();
	}

	/// <summary>
	/// Changes state and launches state Start and Exit Methods
	/// </summary>
	/// <param name="state">state to change to as parameter</param>
	public void ChangeState(TabletStateID state)
	{
		if (EventSystem.current.currentSelectedGameObject == null)
		{
			if (currentState != null)
			{
				previousState = states.FirstOrDefault(x => x.Value == currentState).Key;
				currentState.ExitState();
			}

			//var thekey = 
			currentState = states[state];
			
			currentState.StartState();
		}
	}

	/// <summary>
	/// Starts The Linear Interpolation in update and stops if distance is ok for position resets.
	/// </summary>
	/// <param name="target">Takes target to lerp to as parameter</param>
	public void StartLerp(Vector3 target)
	{

		if (!lerping)
		{
			lerpStartTime = Time.time;
			lerping = true;
			startTransform = transform;
			lerpDistance = Vector3.Distance(transform.position, target);
		}

		if (lerping)
		{
			float distanceLerped = (Time.time - lerpStartTime) * speed * Time.deltaTime;
			float fractJorney = distanceLerped / lerpDistance;
			transform.position = Vector3.Lerp(startTransform.position, target, fractJorney);
		}

		if (Vector3.Distance(startTransform.position, target) < stopLerpDistance)
		{
			lerping = false;
		}
	}

	/// <summary>
	/// Rotates to target direction
	/// </summary>
	/// <param name="target">target to lookAt</param>
	public void WatchTarget(Vector3 target)
	{
		transform.LookAt(target);
	}

	/// <summary>
	/// Activates and Deactivates Tablet
	/// </summary>
	public void OnGrabGribActivate()
	{
		if (grabGrib.GetStateDown(SteamVR_Input_Sources.Any) && !pressed)
		{
			pressed = true;
			Debug.Log("GRABGRIBED");
			StartCoroutine(TabletActivationGrabGribPress());
		}
	}

	/// <summary>
	/// If use grabgrib for state change and other stuff
	/// </summary>
	/// <param name="state">state to change to</param>
	public void OnGrabGribChangeState(TabletStateID state)
	{
		if (grabGrib.GetStateDown(SteamVR_Input_Sources.Any) && !changedMode)
		{
			positions[0].position = vrCamera.position + vrCamera.forward;
			changedMode = true;
			Debug.Log("CHANGED STATE TO " + state.ToString());
			ChangeState(state);
			StartCoroutine(TabletActivationChangeState());
		}
	}

	/// <summary>
	/// Changes tablet distance with touchpad
	/// </summary>
	/// <param name="target">target to move</param>
	/// <param name="direction">direction to move at</param>
	public void ChangeTabletDistance(Transform target, Vector3 direction)
	{
		//if (Time.time > tick) {
		//	tick = Time.time + 1;
		//	Debug.Log(touch.axis);
		//}		
		if (touch.axis.y != 0)
		{
			if (touch.axis.y > 0.5f)
			{
				target.position += direction * Time.deltaTime;
			}
			else if (touch.axis.y < 0.5f)
			{
				target.position -= direction * Time.deltaTime;
			}
		}
	}

	public IEnumerator TabletActivationGrabGribPress()
	{
		///Magic effects
		yield return new WaitForSeconds(deactivateTime);
		Debug.Log("TABLET ACTIVATED/UNACTIVATED");
		if (transform.GetChild(0).gameObject.activeSelf)
		{
			for (int i = 0; i < transform.childCount; i++)
			{
				transform.GetChild(i).gameObject.SetActive(false);
			}
		}
		else
		{
			for (int i = 0; i < transform.childCount; i++)
			{
				transform.GetChild(i).gameObject.SetActive(true);
			}
		}
		pressed = false;
	}

	public IEnumerator TabletActivationChangeState()
	{
		///Magic effects
		yield return new WaitForSeconds(deactivateTime);
		Debug.Log("TABLET ACTIVATED/UNACTIVATED");
		if (transform.GetChild(0).gameObject.activeSelf)
		{
			for (int i = 0; i < transform.childCount; i++)
			{
				transform.GetChild(i).gameObject.SetActive(false);
			}
		}
		else
		{
			for (int i = 0; i < transform.childCount; i++)
			{
				transform.GetChild(i).gameObject.SetActive(true);
			}
		}
		changedMode = false;
	}

	public IEnumerator TabletActivationStateChange()
	{
		///Magic effects
		yield return new WaitForSeconds(deactivateTime);
		Debug.Log("TABLET ACTIVATED/UNACTIVATED");
		if (transform.GetChild(0).gameObject.activeSelf)
		{
			for (int i = 0; i < transform.childCount; i++)
			{
				transform.GetChild(i).gameObject.SetActive(false);
			}
		}
		else
		{
			for (int i = 0; i < transform.childCount; i++)
			{
				transform.GetChild(i).gameObject.SetActive(true);
			}
		}
	}


	/// <summary>
	/// MUST ACTIVATE FROM EVENT OR NO NEED AT ALL WE CHOOSE ONLY ONE MODE IN GAME
	/// </summary>
	public void StateChangeByIDForTesting()
	{

		currentState = states[tabletState];

		//switch (tabletState)
		//{
		//	case TabletStateID.Hold:

		//		break;
		//	case TabletStateID.Follow:
		//		currentState = followState;
		//		break;
		//	case TabletStateID.FollowSide:
		//		currentState = followSideState;
		//		break;
		//	case TabletStateID.FrontHMD:
		//		currentState = frontOfHMDState;
		//		break;
		//	case TabletStateID.FrontController:
		//		currentState = frontOfControllerState;
		//		break;
		//	default:
		//		break;
		//}
	}
}
