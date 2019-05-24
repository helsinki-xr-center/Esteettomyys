using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/**
 * Author: Nomi Lakkala
 * 
 * <summary>
 * Custom Photon transform view component modified for tracking multiple child objects. Suitable for things like VR hands or weapons.
 * </summary>
 */
[RequireComponent(typeof(PhotonView))]
public class ChildTransformView : MonoBehaviour, IPunObservable
{

	[Tooltip("The child transforms to synchronize across network")]
	public Transform[] childTransforms;

	[Header("What to synchronize")]
	public bool synchronizePosition = true;
	public bool synchronizeRotation = true;
	public bool synchronizeScale = false;


	private PhotonView photonView;

	private float[] distances;
	private float[] angles;
	private Vector3[] directions;
	private Vector3[] networkPositions;
	private Quaternion[] networkRotations;
	private Vector3[] storedPositions;

	bool firstReceive = false;

	public void Awake()
	{
		photonView = GetComponent<PhotonView>();
		InitializeArrays();
	}

	/**
	 * Author: Nomi Lakkala
	 * 
	 * <summary>
	 * Initializes private arrays. Not much else.
	 * </summary>
	 */
	private void InitializeArrays()
	{
		distances = new float[childTransforms.Length];
		angles = new float[childTransforms.Length];
		directions = new Vector3[childTransforms.Length];
		networkPositions = new Vector3[childTransforms.Length];
		networkRotations = new Quaternion[childTransforms.Length];
		storedPositions = new Vector3[childTransforms.Length];

		for (int i = 0; i < childTransforms.Length; i++)
		{
			storedPositions[i] = childTransforms[i].localPosition;
		}
	}


	void OnEnable()
	{
		firstReceive = true;
	}

	public void Update()
	{
		if (!this.photonView.IsMine)
		{
			for (int i = 0; i < childTransforms.Length; i++)
			{
				float maxDeltaDist = distances[i] * (1.0f / PhotonNetwork.SerializationRate);
				float maxDeltaAngle = angles[i] * (1.0f / PhotonNetwork.SerializationRate);
				childTransforms[i].localPosition = Vector3.MoveTowards(childTransforms[i].localPosition, networkPositions[i], maxDeltaDist);
				childTransforms[i].localRotation = Quaternion.RotateTowards(childTransforms[i].localRotation, networkRotations[i], maxDeltaAngle);
			}
		}
	}


	/**
	 * Author: Nomi Lakkala
	 * 
	 * <summary>
	 * Handles reading and writing the child objects' data to the provided PhotonStream.
	 * </summary>
	 */
	public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		if (stream.IsWriting)
		{
			for (int i = 0; i < childTransforms.Length; i++)
			{
				if (this.synchronizePosition)
				{
					directions[i] = childTransforms[i].localPosition - storedPositions[i];
					storedPositions[i] = childTransforms[i].localPosition;

					stream.SendNext(childTransforms[i].localPosition);
					stream.SendNext(directions[i]);
				}

				if (this.synchronizeRotation)
				{
					stream.SendNext(childTransforms[i].localRotation);
				}

				if (this.synchronizeScale)
				{
					stream.SendNext(childTransforms[i].localScale);
				}
			}
		}
		else
		{

			for (int i = 0; i < childTransforms.Length; i++)
			{
				if (this.synchronizePosition)
				{
					networkPositions[i] = (Vector3)stream.ReceiveNext();
					directions[i] = (Vector3)stream.ReceiveNext();

					if (firstReceive)
					{
						childTransforms[i].localPosition = networkPositions[i];
						distances[i] = 0f;
					}
					else
					{
						float lag = Mathf.Abs((float)(PhotonNetwork.Time - info.SentServerTime));
						networkPositions[i] += directions[i] * lag;
						distances[i] = Vector3.Distance(childTransforms[i].localPosition, networkPositions[i]);
					}


				}

				if (this.synchronizeRotation)
				{
					networkRotations[i] = (Quaternion)stream.ReceiveNext();

					if (firstReceive)
					{
						angles[i] = 0f;
						childTransforms[i].localRotation = networkRotations[i];
					}
					else
					{
						angles[i] = Quaternion.Angle(childTransforms[i].localRotation, networkRotations[i]);
					}
				}

				if (this.synchronizeScale)
				{
					childTransforms[i].localScale = (Vector3)stream.ReceiveNext();
				}

			}


			if (firstReceive)
			{
				firstReceive = false;
			}
		}
	}
}
