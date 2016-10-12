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

	public MoveTool() {
		_tog = new TurnOnGizmoCommand ();
		_st = new SetTransformationCommand ();
	}

	public void DisableInactiveAxis (RaycastHit hit) {
		foreach(Transform sibling in Gizmo.transform)
		{
			if (sibling.name != hit.collider.name)
				sibling.gameObject.SetActive (false);
			
		}

		var axis = hit.transform.gameObject;
		_axisCollider = axis.GetComponent<CapsuleCollider> ();
		_axisPlane = axis.GetComponent<BoxCollider> ();
		_axisCollider.enabled = false;
		_axisPlane.enabled = true;
	}

	public void UpdateTransformation(GameObject target, RaycastHit hit) {
		var axisName = hit.collider.name;
		var gp = target.transform.localPosition;
		var hp = hit.point;
		var pos = gp;

		switch (axisName) {
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

		Gizmo.transform.position = pos;
		target.transform.position = Gizmo.transform.position;
	}

	public void EnableAllAxis () {
		foreach (Transform sibling in Gizmo.transform)
			sibling.gameObject.SetActive (true);

		_axisCollider.enabled = true;
		_axisPlane.enabled = false;
	}

	class TurnOnGizmoCommand : EditModulationCommand
	{
		public void Execute (TranformationManager manager, ModulationEditionTool tool, RaycastHit hit) {
			if (manager.Target == null) return;
			tool.Gizmo.SetActive (true);
			tool.Gizmo.transform.position = manager.Target.transform.position;
			manager.state = TranformationManager.State.SET_TRANSFORMATION;
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
