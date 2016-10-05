using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TranformationManager : MonoBehaviour 
{
	public Button move;
	public Button rotate;
	public Button scale;

	public ColorBlock active = new ColorBlock ();
	public ColorBlock normal = new ColorBlock ();

	Rotate rotateComponent;
	Move moveComponent;

	void Start () {
		rotateComponent = GetComponent<Rotate> ();
	}

	public void SetMoveMode()
	{
		move.colors = active;
		rotate.colors = normal;
		scale.colors = normal;
	}

	public void Rotate()
	{
		move.colors = normal;
		rotate.colors = active;
		scale.colors = normal;
	}

	public void Scale()
	{
		move.colors = normal;
		rotate.colors = normal;
		scale.colors = active;
	}

}
