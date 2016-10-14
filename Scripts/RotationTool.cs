using UnityEngine;
using System.Collections;

public class RotationTool : ModulationEditionTool 
{
	GameObject _gizmo;
	public GameObject Gizmo { 
		get { return _gizmo; }
		set { 
			_gizmo = value; 
			foreach(Transform sibling in _gizmo.transform)
			{
				if (sibling.childCount == 3)
					allAxis = sibling.gameObject;
				else
					singleAxis = sibling.gameObject;
			}
		}
	}
	public GameObject Target { get; set; }
	public EditModulationCommand TurnOnGizmo { get { return _tog; } }
	public EditModulationCommand SetTransformation { get { return _st; } }
	EditModulationCommand _st;
	EditModulationCommand _tog;
	public GameObject allAxis { get; private set; }
	public GameObject singleAxis { get; private set; }
	Vector3 _direction;
	Vector3 _rotationDirection;
	Vector3 _originalRotation;

	public RotationTool() {
		_tog = new TurnOnGizmoCommand ();
		_st = new SetTransformationCommand ();

	}

	public void DisableInactiveAxis (GameObject target, RaycastHit hit) {
		var axis = hit.transform.gameObject;
		if (axis.name != "x" && axis.name != "y" && axis.name != "z") return;
		var axisName = hit.collider.name;
		switch (axisName) {
		case "x":
			_direction = Vector3.forward;
			_rotationDirection = Vector3.left;
			break;
		case "y":
			_direction = Vector3.forward;
			_rotationDirection = Vector3.up;
			break;
		case "z":
			_direction = Vector3.up;
			_rotationDirection = Vector3.back;
			break;
		}

		var axisRotation = hit.transform.rotation;
		var grandpa = hit.transform.parent.parent;

		allAxis.SetActive (false);
		singleAxis.SetActive (true);
		singleAxis.transform.rotation = axisRotation;
		_originalRotation = target.transform.rotation.eulerAngles;
	}

	public void UpdateTransformation(GameObject target, RaycastHit hit) {
		Vector3 targetDir = hit.point - target.transform.position;
		var dot = Vector3.Dot (singleAxis.transform.right, hit.point);
		var angle = Quaternion.FromToRotation (Vector3.up, hit.point - target.transform.position).eulerAngles.z;
		target.transform.rotation = Quaternion.Euler (_originalRotation + Quaternion.AngleAxis (angle, _rotationDirection).eulerAngles) ;
//		target.transform.rotation = Quaternion.FromToRotation(_rotationDirection, targetDir);
	}

	public void EnableAllAxis () {

		allAxis.SetActive (true);
		singleAxis.SetActive (false);
	}

	class TurnOnGizmoCommand : EditModulationCommand
	{
		public void Execute (TranformationManager manager, ModulationEditionTool tool, RaycastHit hit) {
			if (hit.transform == null)
				return;

		// si cambio de gizmo y raycast es null
			var rotationTool = tool as RotationTool;
			rotationTool.Gizmo.SetActive (true);
			rotationTool.allAxis.SetActive (true);
			rotationTool.singleAxis.SetActive (false);

			rotationTool.Gizmo.transform.position = manager.Target.transform.position;
//			var pos = hit.transform.position;
//			rotationTool.Gizmo.transform.position = pos;
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