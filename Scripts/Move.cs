using UnityEngine;
using System.Collections;

public class Move : MonoBehaviour {
	GameObject empty;

	TranformationManager manager;
	public GameObject moveGizmo;
	GameObject _moveGizmo;
	GameObject _target;
	Collider _targetCollider = null;
	State _state;

	GameObject _axis;
	Collider _axisCollider;
	Collider _axisPlane;


	bool setHighlightMaterial = false;
	bool isTargetSelected = false;
	MeshRenderer _renderer;
	public Material original;
	public Material highlight;
	public Material selected;

	public enum State
	{
		SET_TARGET,
		SET_AXIS,
		SET_POSITION
	};

	void Start () {
		manager = GetComponent<TranformationManager> ();		
		_moveGizmo = Instantiate (moveGizmo);

		empty = new GameObject ();
		_target = empty;
		_moveGizmo.SetActive (false);
	}

	void Update ()
	{
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		RaycastHit hit;

		if (Physics.Raycast (ray, out hit, 100)) {
			Debug.DrawLine (ray.origin, hit.point);

			if (!setHighlightMaterial && !isTargetSelected) {
				_renderer = hit.transform.gameObject.GetComponent<MeshRenderer> ();
				original = _renderer.material;
				_renderer.material = highlight;
				setHighlightMaterial = true;
			}

			switch (_state) {
			case State.SET_TARGET:
				SetTarget (hit);
				break;
			case State.SET_AXIS:
				SetAxis (hit);
				break;
			case State.SET_POSITION:
				SetPosition (hit);
				break;
			}
		} else {
			if (Input.GetMouseButtonDown (0))
				Disable ();

			if (setHighlightMaterial && !isTargetSelected) {
				_renderer.material = original;
				setHighlightMaterial = false;
			}else{
				
			}
		}
	}

	void NewTarget (RaycastHit hit)
	{
		_targetCollider.enabled = true;
		_target = hit.collider.gameObject;
		_targetCollider = _target.GetComponent <Collider> ();
		_moveGizmo.SetActive (true);
		_moveGizmo.transform.position = _target.transform.position;
		_state = State.SET_AXIS;
		
	}

	void SetTarget (RaycastHit hit)
	{
		if (Input.GetMouseButtonDown (0)) {
			var name = hit.collider.name;
			if (name != "x" && name != "y" && name != "z"){
				_target = hit.collider.gameObject;
				_targetCollider = _target.GetComponent <Collider> ();
				_renderer.material = selected;
				isTargetSelected = true;
			}
		}

		if (Input.GetMouseButtonUp (0)) {
			_targetCollider.enabled = false;
			_moveGizmo.SetActive (true);
			_moveGizmo.transform.position = _target.transform.position;
			_state = State.SET_AXIS;
		}
	}

	void SetAxis (RaycastHit hit)
	{
		var axisName = hit.collider.name;
		if (Input.GetMouseButtonDown (0)) {
			if (IsNewTarget (axisName)) {
				NewTarget (hit);
				_state = State.SET_TARGET;
				return;
			}
			else
			{
				RepositionPlane (hit);
				TurnOffAxis (hit);
				_state = State.SET_POSITION;
			}

		}

	}
	void SetPosition (RaycastHit hit)
	{
		if (Input.GetMouseButton (0)) {
			Reposition (hit);
		}

		if (Input.GetMouseButtonUp (0)) {
			TurnOnAxis (hit);
			ResetAxisColliders ();
		}
	}

	bool IsNewTarget (string name)
	{
		return (name != "x" && name != "y" && name != "z");
	}

	void Reposition(RaycastHit hit)
	{
		var axisName = hit.collider.name;
		var gp = _target.transform.localPosition;
				var hp = hit.point;
				var pos = gp;

				switch (axisName) {
				case "x":
					pos = new Vector3 (hp.x, gp.y, gp.z);
					break;
				case "y":
					pos = new Vector3 (gp.x, hp.y, gp.z);
					break;
				case "z":
					pos = new Vector3 (gp.x, gp.y, hp.z);
					break;
				}

		_moveGizmo.transform.position = pos;
		_target.transform.position = pos;
	}

	void RepositionPlane (RaycastHit hit)
	{
		_axis = hit.transform.gameObject;
		_axisCollider = _axis.GetComponent<CapsuleCollider> ();
		_axisPlane = _axis.GetComponent<BoxCollider> ();

		_axisCollider.enabled = false;
		_axisPlane.enabled = true;
	}

	void TurnOffAxis(RaycastHit hit)
	{
		foreach(Transform sibling in _axis.transform.parent)
		{
			if (sibling.name != hit.collider.name)
				sibling.gameObject.SetActive (false);
		}
	}

	void ResetAxisColliders()
	{
		_axisCollider.enabled = true;
		_axisPlane.enabled = false;
	}

	void TurnOnAxis(RaycastHit hit)
	{
		foreach(Transform sibling in hit.transform.parent)
		{
			sibling.gameObject.SetActive (true);
		}
	}

	public void Disable ()
	{
		_targetCollider.enabled = true;
		_moveGizmo.SetActive (false);
		_state = State.SET_TARGET;
	}
}
