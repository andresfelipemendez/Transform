using UnityEngine;
using System.Collections;

public class TurnOnGizmoCommand : EditModulationCommand 
{
	TranformationManager _tm;
	ModulationEditionTool _activeTool;

	public TurnOnGizmoCommand(TranformationManager tm)
	{
		_tm = tm;
	}

	public void Execute (ModulationEditionTool activeTool, RaycastHit hit) {
		_activeTool = activeTool;
		_activeTool.Gizmo.SetActive (true);
		var target = hit.collider.gameObject;
		_activeTool.Gizmo.transform.position = target.transform.position;
		_tm.state = TranformationManager.State.SET_TRANSFORMATION;
	}

	public void Undo()
	{
		_activeTool.Gizmo.SetActive (false);
	}
}
