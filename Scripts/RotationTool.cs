﻿using UnityEngine;
using System.Collections;

public class RotationTool : ModulationEditionTool 
{
	public GameObject Gizmo { get; set; }
	public GameObject Target { get; set; }

	public RotationTool(GameObject gizmo)
	{
		Gizmo = gizmo;
		Gizmo.SetActive (false);
	}

	public void SetAxis(TranformationManager tm, RaycastHit hit)
	{
		var target = hit.collider.gameObject;
		if(target.name != "x" && target.name != "y" && target.name != "z")
		{
			tm.state = TranformationManager.State.SET_TARGET;
			return;
		}
	}
}