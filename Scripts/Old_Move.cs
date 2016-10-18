using UnityEngine;
using System.Collections;

public class Old_Move : MonoBehaviour 
{
	public GameObject moveGizmo;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnMouseDown() {
		var bx = GetComponent<BoxCollider> ();
		bx.enabled = false;
		Instantiate (moveGizmo, transform.position,transform.rotation,transform);
	}
}
