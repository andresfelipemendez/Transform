using UnityEngine;
using System.Collections;

public class Move : MonoBehaviour {
	GameObject empty;

	TranformationManager manager;
	public GameObject moveGizmo;
	GameObject _moveGizmo;
	GameObject _target;

	public enum State
	{
		SET_TARGET
	};

	void Start () {
		manager = GetComponent<TranformationManager> ();		
		_moveGizmo = Instantiate (moveGizmo);

		empty = new GameObject ();
		_target = empty;
		_moveGizmo.SetActive (false);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
