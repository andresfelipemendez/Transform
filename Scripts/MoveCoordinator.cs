using UnityEngine;
using System.Collections;

public class MoveCoordinator : MonoBehaviour 
{
	public GameObject x;
	public GameObject y;
	public GameObject z;

	CapsuleCollider localcollider;
	BoxCollider axisCollider;
	Transform grandParent;
	MoveCoordinator coordinator;
	GameObject axis;

	void Update()
	{
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		RaycastHit hit;

		if (Physics.Raycast(ray, out hit, 100)) 
		{
			Debug.DrawLine(ray.origin, hit.point);

			if (Input.GetMouseButtonDown (0)) {
				Debug.Log (hit.transform.name);
			}

			if (Input.GetMouseButtonDown (0)) 
			{
				var g = hit.transform.gameObject;

				axis = hit.transform.gameObject;
				ActiveAxis (axis.name);

				localcollider = axis.GetComponent<CapsuleCollider> ();	
				axisCollider = axis.GetComponent<BoxCollider> ();	
				axisCollider = axis.GetComponent<BoxCollider> ();	
				grandParent = transform.parent;

				localcollider.enabled = false;
				axisCollider.enabled = true;
			}

			if (Input.GetMouseButton (0)) 
			{
				Debug.Log (axis);
				var gp = grandParent.localPosition;
				var hp = hit.point;
				var pos = gp;

				switch (axis.name) {
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
				Finish ();
				localcollider.enabled = true;
				axisCollider.enabled = false;
			}
		}
	}

	public void ActiveAxis(string axis) {
		switch (axis) {
		case "x":
			y.SetActive (false);
			z.SetActive (false);
			break;
		case "y":
			x.SetActive (false);
			z.SetActive (false);
			break;
		case "z":
			x.SetActive (false);
			y.SetActive (false);
			break;
		}
	}

	public void Finish()
	{
		Debug.Log ("finish");
		foreach (Transform child in transform) 
		{
			child.gameObject.SetActive (true);
		}
	}

}
