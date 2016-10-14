using UnityEngine;
using System.Collections;

public class TranformationManager : MonoBehaviour 
{
	
	ModulationEditionTool _moveTool;
	ModulationEditionTool _rotateTool;

	ModulationEditionTool _activeTool;
	ModulationEditionTool _inactiveTool;

	SetTargetCommand _setTarget;
	EditModulationCommand _deleteModulationItem;
	EditModulationCommand _command;
	RaycastHit _hit;

	public State state;
	public Vector3 TargetPosition;
	public Quaternion TargetRotation;
	public GameObject RotateGizmo;
	public GameObject MoveGizmo;
	public GameObject Target = null;
	public Material selectedMaterial;

	public enum State {
		SET_TARGET,
		SET_TRANSFORMATION,
		INSERT_ITEM
	};

	void Start () {
		SetGizmos ();
		_setTarget = new SetTargetCommand (this, selectedMaterial);
		_deleteModulationItem = new DeleteModulationItem (this);
	}

	void SetGizmos()
	{
		_moveTool = new MoveTool();
		_rotateTool = new RotationTool ();
		_moveTool.Gizmo = Instantiate (MoveGizmo);
		_rotateTool.Gizmo = Instantiate (RotateGizmo);
		_moveTool.Gizmo.SetActive (false);
		_rotateTool.Gizmo.SetActive (false);
		_activeTool = _moveTool;
		_inactiveTool = _rotateTool;
	}

	public void MoveMode()
	{
		if(Target != null)
			_activeTool.TurnOnGizmo.Undo (_activeTool);
		_activeTool = _moveTool;
		_activeTool.TurnOnGizmo.Execute (this, _activeTool, _hit);
	}

	public void RotateMode()
	{
		_activeTool.Gizmo.SetActive (false);
		_activeTool = _rotateTool;
		_activeTool.TurnOnGizmo.Execute (this, _activeTool, _hit);
	}

	public void InsertMode()
	{
		_moveTool.Gizmo.SetActive (false);
		_rotateTool.Gizmo.SetActive (false);
		state = State.INSERT_ITEM;
	}

	public void SetNewPanelAsTarget(GameObject newpanel)
	{
		_setTarget.Execute (this, _activeTool, newpanel);
		return;
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
				_hit = hit;
				_command = command;
				command.Execute (this, _activeTool, hit);
			}
		}
	}

	EditModulationCommand HandleInput(RaycastHit hit)
	{
		EditModulationCommand command = null;

			if (Input.GetKeyDown (KeyCode.Backspace)) {
			return _deleteModulationItem;
			}

		switch(state)
		{
		case State.SET_TARGET:
			if (Input.GetMouseButtonDown (0)){
				command = _setTarget;
			}
			if (Input.GetMouseButtonUp (0)) command = _activeTool.TurnOnGizmo;
			break;

		case State.SET_TRANSFORMATION:
			if (Input.GetMouseButtonDown (0)){
				command = _setTarget;
				_activeTool.DisableInactiveAxis (Target, hit);
			}
			if (Input.GetMouseButton (0))
				_activeTool.UpdateTransformation (Target, hit);
			if(Input.GetMouseButtonUp (0)) {
				_activeTool.EnableAllAxis ();
			}
			break;
		}

		return command;
	}

	public class DeleteModulationItem : EditModulationCommand {

		TranformationManager _tm;
		GameObject _target;

		public DeleteModulationItem(TranformationManager tm)
		{
			_tm = tm;
		}

		public void Execute (TranformationManager manager, ModulationEditionTool tool, RaycastHit hit) 
		{
			_target = manager.Target;
			Destroy (manager.Target);
		}

		public void Undo(ModulationEditionTool tool)
		{}
	}
}