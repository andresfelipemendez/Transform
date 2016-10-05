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
		moveComponent = GetComponent<Move> ();

		move.colors = active;
		moveComponent.enabled = true;
		rotateComponent.enabled = false;
	}

	public void MoveMode()
	{
		move.colors = active;
		rotate.colors = normal;
		scale.colors = normal;

		moveComponent.enabled = true;
		rotateComponent.enabled = false;
		rotateComponent.Disable ();
	}

	public void RotateMode()
	{
		move.colors = normal;
		rotate.colors = active;
		scale.colors = normal;

		moveComponent.enabled = false;
		rotateComponent.enabled = true;
		moveComponent.Disable ();
	}

	public void ReplaceMode()
	{
		move.colors = normal;
		rotate.colors = normal;
		scale.colors = active;
	}

}
