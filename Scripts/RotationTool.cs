using UnityEngine;
using System.Collections;

public class RotationTool : ModulationEditionTool 
{
	public GameObject Gizmo { get; set; }
	public GameObject Target { get; set; }
	public EditModulationCommand TurnOnGizmo { get { return _tog; } }
	public EditModulationCommand SetTransformation { get { return _st; } }
	GameObject allAxis;
	GameObject singleAxis;
	EditModulationCommand _st;
	EditModulationCommand _tog;
	Vector3 _direction;
	Vector3 _rotationDirection;

	public RotationTool() {
		_tog = new TurnOnGizmoCommand ();
		_st = new SetTransformationCommand ();
	}

	public void DisableInactiveAxis (RaycastHit hit) {
		var axisName = hit.collider.name;
		switch (axisName) {
		case "x":
			_direction = Vector3.forward;
			_rotationDirection = Vector3.left;
			break;
		case "y":
			_direction = Vector3.right;
			_rotationDirection = Vector3.up;
			break;
		case "z":
			_direction = Vector3.up;
			_rotationDirection = Vector3.back;
			break;
		}

//		rotateGizmo.SetActive (false);
//		axisGizmo.SetActive (true);
//		axisGizmo.transform.position = target.transform.position;
	}

	public void UpdateTransformation(GameObject target, RaycastHit hit) {
//		angle =  Vector3.Angle ( hit.point , direction);
//			dot = Vector3.Dot (axisGizmo.transform.right, hit.point);
//			if (dot > 0)
//				angle += 180;
//
//			target.transform.rotation = Quaternion.AngleAxis (angle, rotationDirection);
	}

	public void EnableAllAxis () {
//		targetCollider.enabled = true;
//		rotateGizmo.SetActive (false);
//		axisGizmo.SetActive (false);
	}

	class TurnOnGizmoCommand : EditModulationCommand
	{
		public void Execute (TranformationManager manager, ModulationEditionTool tool, RaycastHit hit) {
			tool.Gizmo.SetActive (true);
			var pos = manager.Target.transform.position;
			tool.Gizmo.transform.position = pos;
			manager.state = TranformationManager.State.SET_TRANSFORMATION;
		}

		public void Undo(ModulationEditionTool tool) {
			tool.Gizmo.SetActive (false);
		}
	}

	class SetTransformationCommand : EditModulationCommand
	{
		public void Execute (TranformationManager manager, ModulationEditionTool tool, RaycastHit hit) {}

		public void Undo(ModulationEditionTool tool) {}
	}
}