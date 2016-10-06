using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Projection : MonoBehaviour {
	public Camera cam;
	public Dropdown projections;

	public void SetProjection()
	{
		switch (projections.value) {
		case 0: //Orthographic
			cam.orthographic = true;
			break;
		case 1: //Perspective
			cam.orthographic = false;
			break;
		default:
			break;
		}
	}
}
