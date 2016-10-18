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

	public void Execute (RaycastHit hit) 
	{
		if (_tm.Target != null)
			_tm.Target.GetComponent<MeshRenderer> ().material = _pm;

		var target = hit.collider.gameObject;
		_pm = target.GetComponent<MeshRenderer> ().material;
		target.GetComponent<MeshRenderer> ().material = _sm;
		_tm.Target = target;
		Debug.Log ("set target " + hit.collider.name);
	}
}
