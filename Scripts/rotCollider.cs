using UnityEngine;
using System.Collections;

public class rotCollider : MonoBehaviour {

	SpriteRenderer empty = new SpriteRenderer();
	bool setOriginalColor = true;
	bool hover = false;

	public Vector3 pos;
	public Vector3 hitPoint;
	public float angle;

	public SpriteRenderer prev = new SpriteRenderer();
	public Color prevColor = new Color();

	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;



		if (Physics.Raycast (ray, out hit, 100)) 
		{	
			SpriteRenderer sp = hit.transform.gameObject.GetComponent<SpriteRenderer> ();
			if (setOriginalColor) 
			{
				prev = sp;
				prevColor = sp.color;

				sp.color = Color.yellow;

				hover = true;
				setOriginalColor = false;
			}
			if (Input.GetMouseButton (0)) {
				hit.transform.position = hit.transform.position + hit.transform.forward * 2;
			}
			if(Input.GetMouseButton(0))
			{
				pos = hit.transform.parent.transform.position;

				hitPoint = hit.point;
				sp.color = Color.yellow;
				Vector3 targetDir = hit.transform.parent.transform.position - hit.point;
				angle = Vector3.Angle( targetDir, transform.forward );
			}
		} else {
			if (!setOriginalColor && hover) {
				hover = false;
				setOriginalColor = true;
				prev.color = prevColor;
			}
		}

	}
}
