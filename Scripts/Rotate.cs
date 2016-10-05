using UnityEngine;
using System.Collections;

public class Rotate : MonoBehaviour
{
	TranformationManager manager;
	public GameObject completeRotationGizmo;
	public GameObject generalRotationGizmo;

	GameObject rotateGizmo;
	GameObject axisGizmo;
	GameObject target;
	Vector3 oldRotation;

	GameObject empty;

	State _state;
	public string axis;
	public Vector3 direction;
	public Vector3 rotationDirection;
	public Vector3 newRotation;
	public float angle;
	public float dot;

	enum State {
		SET_TARGET,
		SET_AXIS,
		SET_ROTATION
	};

	void Awake () {
		manager = GetComponent<TranformationManager> ();		
		rotateGizmo = Instantiate (generalRotationGizmo);
		axisGizmo = Instantiate (completeRotationGizmo);
		empty = new GameObject ();
		target = empty;
		rotateGizmo.SetActive (false);
		axisGizmo.SetActive (false);
	}

	void Update () {
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		RaycastHit hit;

		if (Physics.Raycast (ray, out hit, 100)) {
			Debug.DrawLine (ray.origin, hit.point);

			switch (_state) {
			case State.SET_TARGET:
				SetTarget (hit);
				break;
			case State.SET_AXIS:
				SetAxis (hit);
				break;
			case State.SET_ROTATION:
				SetRotation (hit);
				break;
			}
		} 



	}

	void SetTarget (RaycastHit hit) {
		if (Input.GetMouseButtonDown (0)) {
			var name = hit.collider.name;
			if (name != "x" && name != "y" && name != "z")
				target = hit.collider.gameObject;
		}

		if (Input.GetMouseButtonUp (0)) {
			target.GetComponent <Collider> ().enabled = false;
			oldRotation = target.transform.rotation.eulerAngles;
			rotateGizmo.SetActive (true);
			rotateGizmo.transform.position = target.transform.position;
			_state = State.SET_AXIS;
		}
	}

	void SetAxis (RaycastHit hit) {
		axis = hit.collider.name;
		//axis hover

		if (Input.GetMouseButtonDown (0)) {
			switch (axis) {
			case "x":
				direction = Vector3.forward;
				rotationDirection = Vector3.left;
				break;
			case "y":
				direction = Vector3.right;
				rotationDirection = Vector3.up;
				break;
			case "z":
				direction = Vector3.up;
				rotationDirection = Vector3.back;
				break;
			default:
				direction = new Vector3 ();
				break;
			}
		}

		if (Input.GetMouseButtonUp (0)) {
			rotateGizmo.SetActive (false);
			axisGizmo.SetActive (true);
			axisGizmo.transform.position = target.transform.position;
			_state = State.SET_ROTATION;
		}
	}

	void SetRotation(RaycastHit hit) {
		if (Input.GetMouseButtonDown (0)) {
		}

		if (Input.GetMouseButton (0)) {
			angle =  Vector3.Angle ( hit.point , direction);
			dot = Vector3.Dot (axisGizmo.transform.right, hit.point);
			if (dot > 0)
				angle += 180;

			target.transform.rotation = Quaternion.AngleAxis (angle, rotationDirection);
		}

		if(_state == State.SET_ROTATION && Input.GetMouseButtonUp (0)) {
			axisGizmo.SetActive (false);
			target = empty;
			_state = State.SET_TARGET;
		}
	}

}
