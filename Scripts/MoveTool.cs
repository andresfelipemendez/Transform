﻿using UnityEngine;
using System.Collections;

public class MoveTool : ModulationEditionTool 
{
	public GameObject Gizmo { get; set; }
	public GameObject Target { get; set;}
	public EditModulationCommand TurnOnGizmo { get { return _tog; } }
	public EditModulationCommand SetTransformation { get { return _st; } }
	EditModulationCommand _st;
	EditModulationCommand _tog;
	Collider _axisCollider;
	Collider _axisPlane;
	Collider hitCollider;
	Vector3 start;
	Vector3 startPosition;

	public MoveTool() {
		_tog = new TurnOnGizmoCommand ();
		_st = new SetTransformationCommand ();
	}

	public void DisableInactiveAxis (GameObject target, RaycastHit hit, Vector2 mousePos) {
		if (hit.transform.parent != null && hit.transform.parent.name == "Snaps")
			Snap (target, hit);
//			Debug.Log ("movetool " + hit.transform.parent.name);
  
		var axis = hit.transform.gameObject;
		if (axis.name != "x" && axis.name != "y" && axis.name != "z") return;

		foreach(Transform sibling in Gizmo.transform) {
			if (sibling.name != hit.collider.name)
				sibling.gameObject.SetActive (false);
		}

		_axisCollider = axis.GetComponent<CapsuleCollider> ();
		_axisPlane = axis.GetComponent<BoxCollider> ();
		if(_axisCollider != null) _axisCollider.enabled = false;
		if(_axisPlane != null) _axisPlane.enabled = true;
		hitCollider = target.GetComponent<BoxCollider> ();
		hitCollider.enabled = false;

		start = mousePos;
		startPosition = target.transform.position;
	}

	public void UpdateTransformation(GameObject target, RaycastHit hit, Vector2 mousePosition) {
		Debug.Log ("hit.point: "+hit.point.ToString ());
		if (hit.transform.parent != null && hit.transform.parent.name == "Snaps")
			UpdateSnaps(target, hit);
		
		var axisName = hit.collider.name;
		if (axisName != "x" && axisName != "y" && axisName != "z") return;

		var of =  mousePosition;
		var o = of.magnitude - start.magnitude;
		o *= 0.01f;
		var pos = new Vector3 ();

		switch (axisName) {
			case "x":
				pos = startPosition + Vector3.right * o;
				break;
			case "y":
				pos = startPosition + Vector3.up * o;
				break;
			case "z":
				pos = startPosition + Vector3.forward * o;
				break;
		}

		target.transform.position = pos;
		Gizmo.transform.position = pos;
	}

	void Snap(GameObject target, RaycastHit hit) {
		Debug.Log (target + " " + hit);

		foreach(Transform sibling in Gizmo.transform) {
			if(sibling.name != "Snaps")
				sibling.gameObject.SetActive (false);
		}

		return;
	}

	void UpdateSnaps(GameObject target, RaycastHit hit)
	{
	 	Debug.DrawLine (startPosition, hit.point);
	}


	public void EnableAllAxis () {
		foreach (Transform sibling in Gizmo.transform) {
			sibling.gameObject.SetActive (true);
		}
		if(_axisCollider != null) _axisCollider.enabled = true;
		if(_axisPlane != null) _axisPlane.enabled = false;
		if(hitCollider != null)
			hitCollider.enabled = true;
	}


	class TurnOnGizmoCommand : EditModulationCommand
	{
		public void Execute (TranformationManager manager, ModulationEditionTool tool, RaycastHit hit) {
			if (hit.transform == null) return;
			tool.Gizmo.SetActive (true);
			tool.Gizmo.transform.position = hit.transform.position;
			manager.state = TranformationManager.State.SET_TRANSFORMATION;

			var comp = hit.transform.gameObject.GetComponent<WallPanelInfo> ();
		}

		public void Undo(ModulationEditionTool tool) {
			tool.Gizmo.SetActive (false);
		}
	}

	class SetTransformationCommand : EditModulationCommand
	{
		public void Execute (TranformationManager manager, ModulationEditionTool tool, RaycastHit hit) 
		{
		}

		public void Undo(ModulationEditionTool tool)
		{
		}
	}
}
