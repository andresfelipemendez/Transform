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


}
