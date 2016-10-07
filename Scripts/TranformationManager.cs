using UnityEngine;
using System.Collections;

public class TranformationManager : MonoBehaviour 
{
	
	ModulationEditionTool moveTool = new MoveTool();
	ModulationEditionTool rotateTool  = new RotationTool();
	ModulationEditionTool activeTool;

	public GameObject RotateGizmo;
	public GameObject MoveGizmo;
	public GameObject Target = null;
	public State state;

	public Material selectedMaterial;

	Command _setTarget;

	public enum State {
		SET_TARGET,
		SET_TRANSFORMATION,
	};

	void Start () {
		activeTool = moveTool;
		_setTarget = new SetTargetCommand (this, selectedMaterial);
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
			Command command = HandleInput ();
			if(command != null)
				command.Execute (hit);

		}

	}

	Command HandleInput()
	{
		switch(state)
		{
		case State.SET_TARGET:
			if (Input.GetMouseButtonUp (0))
				return _setTarget;
			break;
		case State.SET_TRANSFORMATION:
			if(Input.GetMouseButtonDown (0)){}
			if(Input.GetMouseButton (0)){}
			if(Input.GetMouseButtonUp (0)){}
			break;
		}
		return null;
	}



}
