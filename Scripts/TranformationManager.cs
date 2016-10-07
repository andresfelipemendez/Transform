using UnityEngine;
using System.Collections;

public class TranformationManager : MonoBehaviour 
{
	
	ModulationEditionTool moveTool = new MoveTool();
	ModulationEditionTool rotateTool  = new RotationTool();
	ModulationEditionTool activeTool;

	public GameObject RotateGizmo;
	public GameObject MoveGizmo;

	GameObject _target;

	public State state;

	public enum State {
		SET_TARGET,
		SET_TRANSFORMATION,
	};

	void Start () {
		activeTool = moveTool;
	}

	public void MoveMode()
	{
		activeTool = moveTool;
	}

	public void RotateMode()
	{
		activeTool = rotateTool;
	}

	void Update ()
	{
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		RaycastHit hit;

		if (Physics.Raycast (ray, out hit, 100)) 
		{
			switch (state) {
			case State.SET_TARGET:
				SetTarget (hit);
				break;
			case State.SET_TRANSFORMATION:
				SetTransformation (hit);
				break;
			}

		}

	}

	void ChangeTarget (){}

	void SetTarget(RaycastHit hit)
	{
		if(Input.GetMouseButtonDown (0))
		{
			
			activeTool.SetGizmoPosition ();
		}
			
		if(Input.GetMouseButton (0))
		if(Input.GetMouseButtonUp (0)){}
	}

	void SetTransformation(RaycastHit hit)
	{
		if(Input.GetMouseButtonDown (0)){}
		if(Input.GetMouseButton (0))
		if(Input.GetMouseButtonUp (0)){}
	}

}
