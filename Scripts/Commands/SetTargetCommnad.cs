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
		if (target.name == "x" || target.name == "y" || target.name == "z")
			return;

		if (_tm.Target != null) // note this _tm its redundant, and the material can use a setter
			_tm.Target.GetComponent<MeshRenderer> ().material = _pm;

		_pm = target.GetComponent<MeshRenderer> ().material;
		target.GetComponent<MeshRenderer> ().material = _sm;
		_tm.Target = target;
	}

	public void Undo(ModulationEditionTool tool)
	{

	}
}
