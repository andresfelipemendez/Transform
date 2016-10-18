using UnityEngine;
using System.Collections;

public class SetTargetCommand : EditModulationCommand 
{
	TranformationManager _tm;
	Material _sm;
	Material _pm;

	public SetTargetCommand(TranformationManager tm, Material selectedMaterial)
	{
		_sm = selectedMaterial;
		_tm = tm;
	}

	public void Execute (TranformationManager manager, ModulationEditionTool tool, RaycastHit hit) 
	{
		var target = hit.collider.gameObject;
		if (target.transform.parent != null && target.transform.parent.name == "Snaps") return;
		if (target.name == "x" || target.name == "y" || target.name == "z" || target.name == "RotationAxis" || target.name == "SingleAxis") return;

		if (_tm.Target != null) 
			_tm.Target.GetComponent<MeshRenderer> ().material = _pm;

		_pm = target.GetComponent<MeshRenderer> ().material;
		target.GetComponent<MeshRenderer> ().material = _sm;
		_tm.Target = target;
		_tm.TargetPosition = target.transform.position;
		_tm.TargetRotation = target.transform.rotation;
		_tm.state = TranformationManager.State.SET_TARGET;
	}

	// new panel target
	public void Execute (TranformationManager manager, ModulationEditionTool tool, GameObject newPanel) 
	{
		if (newPanel.name == "x" || newPanel.name == "y" || newPanel.name == "z" || newPanel.name == "RotationAxis" || newPanel.name == "SingleAxis") return;

		if (_tm.Target != null) 
			_tm.Target.GetComponent<MeshRenderer> ().material = _pm;

		_pm = newPanel.GetComponent<MeshRenderer> ().material;
		newPanel.GetComponent<MeshRenderer> ().material = _sm;
		_tm.Target = newPanel;
		_tm.TargetPosition = newPanel.transform.position;
		_tm.TargetRotation = newPanel.transform.rotation;
		_tm.state = TranformationManager.State.SET_TARGET;
	}

	public void Undo(ModulationEditionTool tool)
	{
		
	}
}
