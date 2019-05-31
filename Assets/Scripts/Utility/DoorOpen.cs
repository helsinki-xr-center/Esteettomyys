using UnityEngine;

public class DoorOpen : MonoBehaviour
{

	Animator anim;
	[SerializeField] LayerMask hitMask;

	[SerializeField] float radius;

	bool open;


	private void Start()
	{
		anim = gameObject.GetComponent<Animator>();
	}

	private void FixedUpdate()
	{
		if (!open)
		{
			CastSphere();
		}
	}

	public void CastSphere()
	{
		Collider[] cols = Physics.OverlapSphere(transform.position, radius, hitMask);

		foreach (var col in cols)
		{
			if ( col && !open )
			{
				anim.SetBool("OpenDoor", true);
				open = true;
			}
		}

		//if ( cols.Length == 0 && open)
		//{
		//	anim.SetBool("OpenDoor", false);
		//	open = false;
		//}
	}


	private void OnDrawGizmos()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(transform.position, radius);
	}

}
