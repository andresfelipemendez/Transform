using UnityEngine;
using System.Collections;

public interface ModulationEditionTool 
{
	GameObject Gizmo { get; set;}
	GameObject Target { get; set;}

	void TurnOnGizmo();
	void SetGizmoPosition();
	void TurnOffGizmo();
	void SetTargetTransformation ();
}