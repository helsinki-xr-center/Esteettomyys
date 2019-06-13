using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class TabletStatePattern : MonoBehaviour
{

	public TabletStateID tabletState;
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

	public ITabletState currentState;
	public HoldState holdState;
	public FollowState followState;
	public FrontOfControllerState frontOfControllerState;
	public FrontOfHMDState frontOfHMDState;
	public FollowSideState followSideState;

	//public Dictionary<TabletStateID, ITabletState> states = new Dictionary<TabletStateID, ITabletState>();

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
		positions[0] = vrCamera.GetChild(0).transform;
		positions[1] = vrCamera.GetChild(1).transform;
		positions[2] = vrCamera.GetChild(2).transform;
		positions[3] = playerT.GetChild(0).GetChild(1).GetChild(4).transform;
		positions[4] = playerT.GetChild(0).GetChild(2).GetChild(4).transform;
		//states.Add(TabletStateID.Follow, followState);
		//states.Add(TabletStateID.FollowSide, followSideState);
		//states.Add(TabletStateID.FrontController, frontOfControllerState);
		//states.Add(TabletStateID.FrontHMD, frontOfHMDState);
		//states.Add(TabletStateID.Hold, holdState);
		currentState = holdState;
	}

	private void Update()
	{
		currentState.UpdateState();
		StateChangeByID(); // Can be changed to settings

	}

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
			StartCoroutine(TabletActivation());
		}
	}

	public void OnGrabGribChangeState(TabletStateID state)
	{
		if (grabGrib.GetStateDown(SteamVR_Input_Sources.Any) && !changedMode)
		{
			positions[0].position = vrCamera.position + vrCamera.forward;
			changedMode = true;
			Debug.Log("CHANGED STATE TO " + state.ToString());
			tabletState = state;
			StartCoroutine(TabletActivation2());
		}
	}

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
				target.position += -direction * Time.deltaTime;
			}
			else if (touch.axis.y < 0.5f)
			{
				target.position -= -direction * Time.deltaTime;
			}
		}

	}

	public IEnumerator TabletActivation()
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

	public IEnumerator TabletActivation2()
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

	public void StateChangeByID()
	{

		switch (tabletState)
		{
			case TabletStateID.Hold:
				currentState = holdState;
				OnGrabGribActivate();
				break;
			case TabletStateID.Follow:
				currentState = followState;
				OnGrabGribChangeState(TabletStateID.FrontHMD);
				break;
			case TabletStateID.FollowSide:
				currentState = followSideState;
				OnGrabGribActivate();
				break;
			case TabletStateID.FrontHMD:
				currentState = frontOfHMDState;
				OnGrabGribChangeState(TabletStateID.Follow);
				break;
			case TabletStateID.FrontController:
				currentState = frontOfControllerState;
				OnGrabGribActivate();
				break;
			default:
				break;
		}
	}
}
