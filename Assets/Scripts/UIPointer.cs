using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIPointer : MonoBehaviour
{
	[Range(1f, 10f)] public float defaultLength = 5.0f;
	public GameObject dot;
	Transform rightHand;
	LineRenderer lineRenderer = null;

	private void Awake()
	{
		
		lineRenderer = gameObject.GetComponent<LineRenderer>();
		dot = gameObject.transform.GetChild(0).transform.gameObject;
		rightHand = GameObject.FindGameObjectWithTag("Player").transform.GetChild(0).transform.GetChild(1).transform;
	}

	private void Update()
	{

	}

	
}
