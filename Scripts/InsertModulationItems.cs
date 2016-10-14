using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class InsertModulationItems : MonoBehaviour {
	public GameObject ItemsListPrefab;
	public GameObject Item;
	public GameObject content;
	public Material panelMat;
	public Material cornerMat;

	string panelId;

	void Start () {
		List<AllSteelWallPanel> panels = TestAllPanels.panels;
		foreach (var panel in TestAllPanels.panels) {
			var btn = Instantiate (Item, content.transform) as GameObject;
			btn.name = panel.id;
			var t = btn.GetComponentInChildren<Text> ();
			t.text = panel.id;
			var button = btn.GetComponent<Button> ();
			button.onClick.AddListener (() => {setFormwork (btn.name);});
		}

	}

	public void setFormwork(string id) {
		Debug.Log (id);
		panelId = id;
	}

	public void AddFormwork()
	{
		BuildPanel (panelId);
	}

	public void Close()
	{

	}
	
	public void BuildPanel(string name)
	{
		var panel = TestAllPanels.GetByID(name);
			float height = panel.height;
			float width = panel.width;
			var panelGO = new GameObject(name, typeof(MeshRenderer), typeof(MeshFilter), typeof(BoxCollider), typeof(WallPanelInfo));
			panelGO.name = name;
			panelGO.tag = "WallPanel";
			var panelMeshFilter = panelGO.GetComponent<MeshFilter>();
			var panelMeshRenderer = panelGO.GetComponent<MeshRenderer>();
			var panelBoxCollider = panelGO.GetComponent<BoxCollider>();
			panelBoxCollider.center = new Vector3(panel.width / -2.0f, panel.height / 2.0f, 0.025f);
			panelBoxCollider.size = new Vector3(panel.width, panel.height, 0.05f);
			var info = panelGO.GetComponent<WallPanelInfo>();
			info.id = name;
			info.width = width;
			info.height = height;
			var up = new Vector3 (0, 1, 0);
			var right = new Vector3 (-1, 0, 0);
			var wallPanel = new WallPanel (Vector3.zero, up, width, height, name);
			var wallPanelBuilder = new WallPanelBuilder(wallPanel);
			panelMeshFilter.mesh = wallPanelBuilder.Build();
			panelMeshRenderer.material = panelMat;
	}
}
