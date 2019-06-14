using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Valve.VR;
using System.Linq;

/// <summary>
/// This class controls the states for tablet, uses list of empty gameobjects for positioning. 
/// Also need reference to player and vrcamera. Choose state to start from inspector tabletState.
/// </summary>
public class TabletStatePattern : MonoBehaviour
{

	public bool debugMode;
	public float interval;

	[Tooltip("Current State ID as Enum for menus, change with event")] public TabletStateID tabletState;
	[Tooltip("Set if you want to drag and drop positions , will ignore scripted positions")] public bool manuallyDragAndDropPositions;

	[Tooltip("Tablet Move Speed")] public float speed;
	[Tooltip("How Long it takes to activate tablet")] public float activationTime;
	[Tooltip("Min distance to source on z axis")] public float minDistance;
	[Tooltip("Max distance to source on z axis")]public float maxDistance;

	[Tooltip("Player Position, drag if manually set is toggled on")] public Transform playerT;
	[Tooltip("VRCamera Position, drag if manually set is toggled on")] public Transform vrCamera;
	public Transform rightController;
	public Transform leftController;

	//Only for grabgrib state change coroutines
	bool pressed;
	bool changedMode;

	Transform startTransform;
	float lerpStartTime;
	bool lerping;
	float lerpDistance;
	[Tooltip("Distance to reset lerp start position")]public float stopLerpDistance;
	float tick;
	
	[Tooltip("Previous state id if changed")]public TabletStateID previousState;
	[Tooltip("Currently executing state")]public ITabletState currentState;
	public HoldState holdState;
	public FollowState followState;
	public FrontOfControllerState frontOfControllerState;
	public FrontOfHMDState frontOfHMDState;
	public FollowSideState followSideState;

	// Binds Enums with state classes
	public Dictionary<TabletStateID, ITabletState> states = new Dictionary<TabletStateID, ITabletState>();

	[Header("0 : Front, 1 : Back, 2 : Left, 3 : LeftController, 4 : RightController")]

	/// <summary>
	/// 0 = Front
	/// 1 = Back
	/// 2 = Left
	/// 3 = LeftController
	/// 4 = RightController
	/// </summary>
	public Transform[] positions;

	[Header("Inputs for different tablet state changes")]
	[Tooltip("Used in stopping tablet movement")]public SteamVR_Action_Boolean touchPadPress;
	[Tooltip("Used in Activating/Deactivating tablet")] public SteamVR_Action_Boolean grabGrib;
	[Tooltip("Used in changing tablet distance")] public SteamVR_Action_Vector2 touch;

	private void Awake()
	{
		holdState = new HoldState(this);
		followState = new FollowState(this);
		frontOfControllerState = new FrontOfControllerState(this);
		frontOfHMDState = new FrontOfHMDState(this);
		followSideState = new FollowSideState(this);
	}

	/// <summary>
	/// Add new positions to array and states to dictionary
	/// </summary>
	private void Start()
	{
		if (!manuallyDragAndDropPositions)
		{
			playerT = GameObject.FindGameObjectWithTag("Player").transform;
			vrCamera = playerT.GetChild(0).GetChild(3).transform;
			rightController = playerT.GetChild(0).GetChild(2).transform;
			leftController = playerT.GetChild(0).GetChild(1).transform;

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
	/// Activates and Deactivates Tablet with Grab Grib button
	/// </summary>
	public void OnGrabGribActivate()
	{
		if (grabGrib.GetStateDown(SteamVR_Input_Sources.Any) && !pressed)
		{
			pressed = true;

			if (debugMode)
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
			ChangeState(state);
			StartCoroutine(TabletActivationChangeState());
		}
	}

	/// <summary>
	/// Changes tablet distance with touchpad
	/// </summary>
	/// <param name="target">target to move</param>
	/// <param name="direction">direction to move at</param>
	public void ChangeTabletDistance(Transform target, Vector3 direction, Transform source)
	{
		if (Time.time > tick && debugMode) {
		tick = Time.time + interval;
		Debug.Log(touch.axis);
		Debug.Log(Vector3.Distance(target.position, source.position));
		}	

		if (touch.axis.y != 0 && touch.activeDevice == CheckHandMode())
		{
			
				

			if (touch.axis.y > 0.4f && Vector3.Distance(target.position, source.position) < maxDistance)
			{
				target.position += direction * Time.deltaTime;
			}
			else if (touch.axis.y < 0.4f && Vector3.Distance(target.position, source.position) > minDistance)
			{
				target.position -= direction * Time.deltaTime;
			}
		}
		
	}

	/// <summary>
	/// Tablet Activation with GrabGrib
	/// </summary>
	/// <returns></returns>
	public IEnumerator TabletActivationGrabGribPress()
	{
		///Magic effects
		yield return new WaitForSeconds(activationTime);

		if(debugMode)
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
		yield return new WaitForSeconds(activationTime);

		if (debugMode)
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
		yield return new WaitForSeconds(activationTime);

		if (debugMode)
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

	public SteamVR_Input_Sources CheckHandMode()
	{
		if (GlobalValues.settings.leftHandMode)
		{
			return SteamVR_Input_Sources.LeftHand;
		}
		else
		{
			return SteamVR_Input_Sources.RightHand;
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

	public void DebugStateStatus()
	{
		if (Time.time > tick)
		{
			tick = Time.time + interval;
			Debug.Log("Currently in " + currentState.ToString());
			//Debug.Log(tablet.transform.position);
			//Debug.Log(tablet.positions[0].position);
			//Debug.Log(tablet.speed);
		}
	}
}
