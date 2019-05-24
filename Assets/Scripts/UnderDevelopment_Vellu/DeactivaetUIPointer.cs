using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeactivaetUIPointer : MonoBehaviour
{
	Camera eyes;
	GameObject laser;
	public LayerMask UImask;

	private void Start()
	{
		eyes = transform.GetChild(0).transform.GetChild(1).transform.GetChild(4).transform.gameObject.GetComponent<Camera>();
		laser = FindObjectOfType<UIPointer>().gameObject;
	}

	private void Update()
	{

		RayCastToPointFromCamera();

	}

	public void RayCastToPointFromCamera()
	{

		if (Physics.Raycast(eyes.transform.position, eyes.transform.forward, out RaycastHit hit, 5f))
		{
			Debug.DrawRay(eyes.transform.position, eyes.transform.forward, Color.red, 1f);

			if (hit.transform.gameObject.layer == UImask)
			{
				//Debug.Log ( "HIT UI" );
				laser.SetActive(true);
			}
			else
			{
				//Debug.Log ( "HITTING GROUND" );
				laser.SetActive(false);
			}
		}
	}
}
