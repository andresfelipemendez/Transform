using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ActiveTool : MonoBehaviour 
{
	public Button move;
	public Button rotate;
	public Button scale;

	public ColorBlock active = new ColorBlock ();
	public ColorBlock normal = new ColorBlock ();

	void Start () {
		move.colors = active;
	}

	public void RotateMode ()
	{
		move.colors = normal;
		rotate.colors = active;
	}

	public void MoveMode ()
	{
		move.colors = active;
		rotate.colors = normal;
	}
}
