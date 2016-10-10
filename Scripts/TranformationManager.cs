using UnityEngine;
using System.Collections;

public class TranformationManager : MonoBehaviour 
{
	ModulationEditionTool _moveTool;
	ModulationEditionTool _rotateTool;
	ModulationEditionTool _activeTool;

	EditModulationCommand _setTarget;
	EditModulationCommand _turnOnGizmo;

	EditModulationCommand _command;
	RaycastHit _hit;

	public State state;
	public GameObject RotateGizmo; // the rotate axis needs to be a childn
	public GameObject MoveGizmo;
	public GameObject Target = null;
	public Material selectedMaterial;

	public enum State {
		SET_TARGET,
		SET_TRANSFORMATION,
	};

	void Start () {
		var moveGizmo = Instantiate (MoveGizmo);
		var rotateGizmo = Instantiate (RotateGizmo);
		_moveTool = new MoveTool(moveGizmo);
		_rotateTool = new RotationTool (rotateGizmo);

		_setTarget = new SetTargetCommand (this, selectedMaterial);
		_turnOnGizmo = new TurnOnGizmoCommand (this);

		_activeTool = _moveTool;
	}

	public void MoveMode()
	{
		Undo ();
		_activeTool = _moveTool;
		ReDo ();
	}

	public void RotateMode()
	{
		Undo ();
		_activeTool = _rotateTool;
		ReDo ();
	}

	void Undo ()
	{
		if(_command != null) {
			_command.Undo ();
		}
	}

	void ReDo(){
		if(_command != null) {
			Debug.Log ("re do");
			_command.Execute (_activeTool, _hit);
		}
	}

	void Update ()
	{
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		RaycastHit hit;

		if (Physics.Raycast (ray, out hit, 100)) 
		{
			var command = HandleInput (hit);
			if(command != null)
			{
				_command = command;
				_hit = hit;
				command.Execute (_activeTool, hit);
			}
		}
	}


	EditModulationCommand HandleInput(RaycastHit hit)
	{
		EditModulationCommand command = null;
		switch(state)
		{
		case State.SET_TARGET:
			if (Input.GetMouseButtonDown (0))
			{
				command = _setTarget;
			}

			if (Input.GetMouseButtonUp (0))
			{
				command = _turnOnGizmo;
			}
			break;
		case State.SET_TRANSFORMATION:
			if(Input.GetMouseButtonDown (0)){
				_activeTool.SetAxis (this, hit);
			}

			if(Input.GetMouseButton (0)){}
			if(Input.GetMouseButtonUp (0)){}
			break;
		}
		return command;
	}
}