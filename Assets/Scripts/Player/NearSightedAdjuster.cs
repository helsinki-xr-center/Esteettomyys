using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using System.Linq;

/**
 * Author: Nomi Lakkala
 * 
 * <summary>
 * A script for setting different sight impairments. 
 * Also handles focal point for nearsighted view to focus on the closest object.
 * </summary>
 */
[RequireComponent(typeof(PostProcessVolume))]
public class NearSightedAdjuster : MonoBehaviour
{
	[Range(1, 5)]
	public float maxFocalPoint = 2;
	public bool dofAdjustOn = false;

	public PostProcessProfile normalProfile;
	public PostProcessProfile farSightedProfile;
	public PostProcessProfile nearSightedProfile;
	public PostProcessProfile everythingIsBadProfile;

	private PostProcessVolume volume;
	private Camera mainCam;

	private DepthOfField dof;

	private Collider[] colliderArray = new Collider[10];
	private Quaternion[] raycastAngles;

	// Start is called before the first frame update
	void Start()
	{
		volume = GetComponent<PostProcessVolume>();
		raycastAngles = GetRandomXYRotationValuesInRange(20, 10).ToArray();
	}

	// Update is called once per frame
	void Update()
	{
		if (!dofAdjustOn)
		{
			return;
		}
		if (dof == null)
		{
			return;
		}
		if (mainCam == null)
		{
			mainCam = Camera.main;
			return;
		}

		int found = Physics.OverlapSphereNonAlloc(mainCam.transform.position, maxFocalPoint, colliderArray);

		if (found > 0)
		{
			Vector3 camF = mainCam.transform.forward;
			Vector3 camP = mainCam.transform.position;
			float closest = maxFocalPoint;
			for (int i = 0; i < raycastAngles.Length; i++)
			{
				if (Physics.Raycast(new Ray(camP, raycastAngles[i] * camF), out RaycastHit hit, maxFocalPoint))
				{
					if (hit.distance < closest)
					{
						closest = hit.distance + 0.1f;
					}
				}
			}
			dof.focusDistance.Override(closest);
		}
		else
		{
			dof.focusDistance.Override(maxFocalPoint);
		}
	}

	/**
	 * <summary>
	 * Sets the current eyesight mode to be the desired mode. Turns on <see cref="dofAdjustOn"/> for near sightedness.
	 * </summary>
	 */
	public void SetSightMode(EyesightMode mode)
	{
		if (GlobalValues.controllerMode == ControllerMode.PC)
		{
			dofAdjustOn = false;
			switch (mode)
			{
				case EyesightMode.Normal:
					volume.profile = normalProfile;
					break;
				case EyesightMode.NearSighted:
					volume.profile = nearSightedProfile;
					dofAdjustOn = true;
					dof = volume.profile.GetSetting<DepthOfField>();
					break;
				case EyesightMode.FarSighted:
					volume.profile = farSightedProfile;
					break;
				case EyesightMode.Bad:
					volume.profile = everythingIsBadProfile;
					break;
				default:
					break;
			}
		}
		else
		{
			//TODO: VR implementation
		}
	}


	private void OnDrawGizmos()
	{
		if (!dofAdjustOn || volume == null)
		{
			return;
		}
		if (dof == null)
		{
			return;
		}
		if (mainCam == null)
		{
			return;
		}

		int found = Physics.OverlapSphereNonAlloc(mainCam.transform.position, maxFocalPoint, colliderArray);

		if (found > 0)
		{
			Vector3 camF = mainCam.transform.forward;
			Vector3 camP = mainCam.transform.position;

			for (int i = 0; i < raycastAngles.Length; i++)
			{
				if (Physics.Raycast(new Ray(camP, raycastAngles[i] * camF), out RaycastHit hit, maxFocalPoint))
				{
					Gizmos.color = Color.green;
					Gizmos.DrawLine(camP, hit.point);
				}
				else
				{
					Gizmos.color = Color.grey;
					Gizmos.DrawLine(camP, camP + raycastAngles[i] * camF * maxFocalPoint);
				}
			}

		}
	}

	private IEnumerable<Quaternion> GetRandomXYRotationValuesInRange(float r, int num)
	{
		for (int i = 0; i < num; i++)
		{
			yield return Quaternion.Euler(Random.insideUnitCircle * r);
		}
	}
}
