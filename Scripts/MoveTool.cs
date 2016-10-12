using UnityEngine;
using System.Collections;

public class MoveTool : ModulationEditionTool 
{
	public GameObject Gizmo { get; set; }
	public GameObject Target { get; set;}

	EditModulationCommand _st;
	EditModulationCommand _tog;

	public EditModulationCommand TurnOnGizmo {
		get {
			if (_tog == null) _tog = new TurnOnGizmoCommand ();
			return _tog;
		}
	}

	public EditModulationCommand SetTransformation {
		get {
			if (_st == null) _st = new SetTransformationCommand ();
			return _st;
		}
	}

	class TurnOnGizmoCommand : EditModulationCommand
	{
		public void Execute (TranformationManager manager, ModulationEditionTool tool, RaycastHit hit) {
			tool.Gizmo.SetActive (true);
			var pos = hit.transform.position;
			tool.Gizmo.transform.position = pos;
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
