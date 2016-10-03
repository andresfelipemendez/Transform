using UnityEngine;
using System.Collections;

public class MoveCoordinator : MonoBehaviour {

	public GameObject x;
	public GameObject y;
	public GameObject z;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void ActiveAxis(string axis) {
		switch (axis) {
		case "x":
			y.SetActive (false);
			z.SetActive (false);
			break;
		case "y":
			x.SetActive (false);
			z.SetActive (false);
			break;
		case "z":
			x.SetActive (false);
			y.SetActive (false);
			break;
		}
	}

	public void Finish()
	{
		x.SetActive (true);
		y.SetActive (true);
		z.SetActive (true);
	}

}
