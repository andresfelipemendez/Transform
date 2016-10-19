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

	Collider hitCollider;

	Vector2 start;

	public RotationTool() {
		_tog = new TurnOnGizmoCommand ();
		_st = new SetTransformationCommand ();
	}

	public void DisableInactiveAxis (GameObject target, RaycastHit hit, Vector2 mousePos, Camera viewportCamera) {
		var axisName = hit.collider.name;
		if (axisName != "x" && axisName != "y" && axisName != "z") return;
		switch (axisName) {
		case "x":
			_direction = Vector3.down;
			_rotationDirection = Vector3.right;
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

		allAxis.SetActive (false);
		singleAxis.SetActive (true);
		singleAxis.transform.rotation = axisRotation;
		_originalRotation = target.transform.rotation.eulerAngles;

		hitCollider = target.GetComponent<BoxCollider> ();
		hitCollider.enabled = false;
		start = mousePos;
	}

	public void UpdateTransformation (GameObject target, RaycastHit hit, Vector2 mousePosition, Camera viweportCamera)
	{
		var current = mousePosition;
		var angle = current.magnitude - start.magnitude;
//		var rotation = Quaternion.FromToRotation (_direction, hit.point - target.transform.position).eulerAngles;
//		rotation.Scale (_rotationDirection);
//		var angle = rotation.x + rotation.y + rotation.z;
		target.transform.rotation = Quaternion.Euler (_originalRotation + angle * _rotationDirection);
		return;
	}

	public void EnableAllAxis () {
		allAxis.SetActive (true);
		singleAxis.SetActive (false);
		hitCollider.enabled = true;
	}

	class TurnOnGizmoCommand : EditModulationCommand
	{
		public void Execute (TranformationManager manager, ModulationEditionTool tool, RaycastHit hit) {
			if (hit.transform == null)
				return;

			var rotationTool = tool as RotationTool;
			rotationTool.Gizmo.SetActive (true);
			rotationTool.allAxis.SetActive (true);
			rotationTool.singleAxis.SetActive (false);

			rotationTool.Gizmo.transform.position = manager.Target.transform.position;
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