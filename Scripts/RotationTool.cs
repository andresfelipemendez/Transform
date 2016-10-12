using UnityEngine;
using System.Collections;

public class RotationTool : ModulationEditionTool 
{
	public GameObject Gizmo { get; set; }
	public GameObject Target { get; set; }
	public EditModulationCommand TurnOnGizmo{ get; private set;}
	public EditModulationCommand SetTransformation{ get; private set;}

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
		public void Execute (TranformationManager manager, ModulationEditionTool tool, RaycastHit hit) {}

		public void Undo(ModulationEditionTool tool) {}
	}
}