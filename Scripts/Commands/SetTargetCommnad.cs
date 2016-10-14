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

	public void Execute (TranformationManager manager, ModulationEditionTool tool, GameObject target) 
	{
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

	public void Undo(ModulationEditionTool tool)
	{
		
	}
}
