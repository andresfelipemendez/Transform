using UnityEngine;
using System.Collections;

public interface ModulationEditionTool 
{
	GameObject Gizmo { get; set;}
	GameObject Target { get; set;}
	EditModulationCommand TurnOnGizmo{ get; }
	EditModulationCommand SetTransformation{ get; }
	void UpdateTransformation (GameObject target, RaycastHit hit, Vector2 mousePos);
	void DisableInactiveAxis (GameObject target, RaycastHit hit, Vector2 mousePos);
	void EnableAllAxis ();

	// function that return instances
}