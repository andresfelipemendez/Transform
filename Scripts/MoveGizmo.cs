using UnityEngine;
using System.Collections;

public class MoveGizmo : MonoBehaviour 
{
	CapsuleCollider localcollider;
	BoxCollider axisCollider;
	Transform grandParent;
	MoveCoordinator coordinator;
	public string axis;

	void Awake()
	{
		localcollider = GetComponent<CapsuleCollider> ();	
		axisCollider = GetComponent<BoxCollider> ();	
		grandParent = transform.parent.parent;
		coordinator = transform.parent.GetComponent<MoveCoordinator> ();

	}

	void Start () 
	{
		axis = gameObject.name;
		localcollider.enabled = true;
		axisCollider.enabled = false;
	}

	void Update()
	{
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		RaycastHit hit;
		
		if (Physics.Raycast(ray, out hit, 100)) 
		{
			Debug.DrawLine(ray.origin, hit.point);

			if (Input.GetMouseButtonDown (0)) {
				coordinator.ActiveAxis (axis);
				localcollider.enabled = false;
				axisCollider.enabled = true;
			}
			
			if (Input.GetMouseButton (0)) 
			{
				Debug.Log (axis);
				var gp = grandParent.localPosition;
				var hp = hit.point;
				var pos = gp;

				switch (axis) {
				case "x":
					pos = new Vector3 (hp.x, gp.y, gp.z);
					break;
				case "y":
					pos = new Vector3 (gp.x, hp.y, gp.z);
					break;
				case "z":
					pos = new Vector3 (gp.x, gp.y, hp.z);
					break;
				}

				grandParent.position = pos;
			}

			if (Input.GetMouseButtonUp (0)) 
			{
				coordinator.Finish ();
				localcollider.enabled = true;
				axisCollider.enabled = false;
			}
		}
	}
}
