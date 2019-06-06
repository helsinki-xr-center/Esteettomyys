using UnityEngine;

public class DoorOpen : MonoBehaviour
{

	Animator anim;

	[SerializeField] LayerMask hitMask;

	[SerializeField] float radius;
	//Collider[] cols;

	public bool open;

	private void Start()
	{
		anim = gameObject.GetComponent<Animator>();
	}

	public void OpenAnimation()
	{
		anim.SetTrigger("Open");
	}

	public void CloseAnimation()
	{
		anim.SetTrigger("Close");
	}

	//public void CastSphere()
	//{
	//	cols = Physics.OverlapSphere(transform.position, radius, hitMask);


	//	foreach (var col in cols)
	//	{
	//		if (col && !open)
	//		{
	//			open = true;
	//			Debug.Log("OPEN");
	//			anim.SetTrigger("Open");

	//		}
	//		else
	//		{
	//			open = false;
	//			anim.SetTrigger("Close");

	//		}
	//	}

	//if ( cols.Length == 0 && open)
	//{
	//	anim.SetBool("OpenDoor", false);
	//	open = false;
	//}

}
