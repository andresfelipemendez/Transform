using UnityEngine;
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
	Vector2 start;
	Vector3 startPosition;
	Vector3 snapEnd;
	Vector3 offset;

	bool isSnapping;

	public MoveTool() {
		_tog = new TurnOnGizmoCommand ();
		_st = new SetTransformationCommand ();
	}

	public void DisableInactiveAxis (GameObject target, RaycastHit hit, Vector2 mousePos,Camera viewportCamera) {
		Target = target;
		if (hit.transform.parent != null && hit.transform.parent.name == "Snaps") {
			isSnapping = true;
			Snap (target, hit);
		}

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

		offset = target.transform.position - viewportCamera.ScreenToWorldPoint (mousePos);
	}

	public void UpdateTransformation(GameObject target, RaycastHit hit, Vector2 mousePos,Camera viewportCamera) {

//		var p = Camera
		if(isSnapping) {
			UpdateSnaps(hit);
			return;
		}

		var curPosition = viewportCamera.ScreenToWorldPoint (mousePos) + offset;
		var axisName = hit.collider.name;
			if (axisName != "x" && axisName != "y" && axisName != "z") return;

		var pos = new Vector3 ();

		switch (axisName) {
			case "x":
			pos = new Vector3(curPosition.x, startPosition.y, startPosition.z);
				break;
			case "y":
			pos = new Vector3(startPosition.x, curPosition.y, startPosition.z);
				break;
			case "z":
				pos = new Vector3(startPosition.x, startPosition.y, curPosition.z);
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

	void UpdateSnaps(RaycastHit hit)
	{
	 	Debug.DrawLine (startPosition, hit.point);
	 	Debug.Log (hit.collider.GetType ().ToString ());
		if (hit.collider.GetType ().ToString () == "UnityEngine.BoxCollider")
		{
			Gizmo.transform.FindChild ("Snaps").FindChild ("target").position = hit.collider.transform.position;
			snapEnd = hit.collider.transform.position;
		}
	}


	public void EnableAllAxis () {
		foreach (Transform sibling in Gizmo.transform) {
			sibling.gameObject.SetActive (true);
		}
		if(_axisCollider != null) _axisCollider.enabled = true;
		if(_axisPlane != null) _axisPlane.enabled = false;
		if(hitCollider != null)
			hitCollider.enabled = true;
		if(isSnapping){
			Target.transform.position = snapEnd;
			Gizmo.transform.position = snapEnd;
			isSnapping = false;
		}

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
