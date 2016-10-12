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

	public MoveTool() {
		_tog = new TurnOnGizmoCommand ();
		_st = new SetTransformationCommand ();
	}

	public void UpdateTransformation(GameObject target, RaycastHit hit) {
		Debug.Log ("move " + hit.point);
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
//			if (manager.Target == null) return;
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
