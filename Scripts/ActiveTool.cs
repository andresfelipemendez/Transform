using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ActiveTool : MonoBehaviour 
{
	public Button move;
	public Button rotate;
	public Button insert;

	public ColorBlock active = new ColorBlock ();
	public ColorBlock normal = new ColorBlock ();

	void Start () {
		move.colors = active;
	}
	
	public void MoveMode ()
	{
		move.colors = active;
		insert.colors = normal;
		rotate.colors = normal;
	}

	public void RotateMode ()
	{
		move.colors = normal;
		rotate.colors = active;
		insert.colors = normal;
	}

	public void InsertMode()
	{
		move.colors = normal;
		rotate.colors = normal;
		insert.colors = active;
	}
}
