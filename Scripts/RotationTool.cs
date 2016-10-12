using UnityEngine;
using System.Collections;

public class RotationTool : ModulationEditionTool 
{
	public GameObject Gizmo { get; set; }
	public GameObject Target { get; set; }
	public EditModulationCommand TurnOnGizmo { get { return _tog; } }
	public EditModulationCommand SetTransformation { get { return _st; } }
	EditModulationCommand _st;
	EditModulationCommand _tog;

	public RotationTool() {
		_tog = new TurnOnGizmoCommand ();
		_st = new SetTransformationCommand ();
	}

	public void UpdateTransformation(GameObject target, RaycastHit hit) {
		Debug.Log ("rotation " + hit.point);
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