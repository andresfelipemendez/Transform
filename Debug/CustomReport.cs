using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Poly2Tri;

public class CustomReport : MonoBehaviour 
{
	public Material wallPanelsMaterial;
	public Material conersMaterial;
	public Material noLimit;
	public Material topLimit;

	// Use this for initialization
	void Start () {
		var report = new ModulationReport();

		var corners = new List<Corner> ();
		var wallFaces = new List<WallFace>();
		var wallPanels = TestAllPanels.panels;
		var externalCorners = TestAllExternalCorners.externalCorners;
		var internalCorners = TestAllInternalCorners.internalCorners;

		wallFaces.Add (new NoLimit(Vector3.zero,Vector3.zero,Vector3.forward, 2,1));

		var modulation = new AIModulator(corners, wallFaces, report, externalCorners, internalCorners, wallPanels);

		WallFacesVisualizer(modulation.wallFaces);
		modulationBuilder(report, gameObject, wallPanelsMaterial, conersMaterial);
	}

	public void modulationBuilder(ModulationReport report, GameObject parent, Material mat, Material corner)
	{
		StartCoroutine("BuildInternalCorners", report);
		StartCoroutine("BuildExternalCorners", report);
		StartCoroutine("BuildWallPanels", report);
	}

	private void WallFacesVisualizer(List<WallFace> wallFaces)
	{
		foreach (var wallFace in wallFaces)
		{
			var panelGO = new GameObject("wallface", typeof(MeshRenderer), typeof(MeshFilter), typeof(WallFaceDebug));
			var debug = panelGO.GetComponent<WallFaceDebug>();
			var panelMeshFilter = panelGO.GetComponent<MeshFilter>();
			var panelMeshRenderer = panelGO.GetComponent<MeshRenderer>();
			string type = wallFace.GetType().ToString();
			switch (type)
			{
				case "NoLimit":
					panelMeshRenderer.material = noLimit;
					break;

				case "TopLimit":

					panelMeshRenderer.material = topLimit;
					break;
			}
			panelMeshFilter.mesh = WallFaceBuilder(wallFace);
			debug.width = wallFace.width;
			debug.height = wallFace.height;
            debug.director = wallFace.direction;
            panelGO.transform.position = wallFace.position;
			panelGO.transform.rotation = Quaternion.Euler(wallFace.rotation);
		}
	}

	private Mesh WallFaceBuilder(WallFace wallFace)
	{
		Mesh mesh = new Mesh();
		List<Vector3> vertices = new List<Vector3>();
		List<int> triangles = new List<int>();

		var w = Utils.Round( wallFace.width ) ;
		var h = wallFace.height;

		if (Utils.IsEqual(w, 0))
			return mesh;

		PolygonPoint[] shape =
		{
			new PolygonPoint(0, 0),
			new PolygonPoint(h, 0),
			new PolygonPoint(h, w),
			new PolygonPoint(0, w),
		};

		Polygon p = new Polygon(shape);

		foreach (var polygonPoint in shape)
		{
			var vertex = new Vector3(0.002f, polygonPoint.Xf, polygonPoint.Yf);
			vertices.Add(vertex);
		}

		P2T.Triangulate(p);

		foreach (var triangle in p.Triangles)
		{
			var points = triangle.Points;
			var f1v1 = new Vector3(0.002f, points._0.Xf, points._0.Yf);
			var f1v2 = new Vector3(0.002f, points._1.Xf, points._1.Yf);
			var f1v3 = new Vector3(0.002f, points._2.Xf, points._2.Yf);
			triangles.Add(vertices.IndexOf(f1v1));
			triangles.Add(vertices.IndexOf(f1v2));
			triangles.Add(vertices.IndexOf(f1v3));
		}

		mesh.vertices = vertices.ToArray();
		mesh.triangles = triangles.ToArray();
		mesh.RecalculateNormals();
		return mesh;
	}


	IEnumerator BuildInternalCorners (ModulationReport report)
	{
		Dictionary<string, GameObject> externalCornersGameObjects = new Dictionary<string, GameObject>();

		foreach (KeyValuePair<string, List<PositionRotation>> cornerback in report.InternallCorners)
		{
			string id = cornerback.Key;
			float height = TestAllInternalCorners.GetByID(cornerback.Key).height;
			var panelGO = new GameObject(name, typeof(MeshRenderer), typeof(MeshFilter), typeof(BoxCollider), typeof(OutterCornerInfo));
			panelGO.name = id;
			panelGO.tag = "InnerCorner";
			var panelMeshFilter = panelGO.GetComponent<MeshFilter>();
			var panelMeshRenderer = panelGO.GetComponent<MeshRenderer>();
			var panelBoxCollider = panelGO.GetComponent<BoxCollider>();
			panelBoxCollider.center = new Vector3(0.075f, height / 2.0f, 0.075f);
			panelBoxCollider.size = new Vector3(0.15f, height, 0.15f);
			var info = panelGO.GetComponent<OutterCornerInfo>();
			info.id = cornerback.Key;
			info.size = 0.15f;
			info.height = height;
			var up = new Vector3 (-1, 0, 0);
			var right = new Vector3 (0, 1, 0);
			var corner = new RightCornerback (up, right, Vector3.zero, height);
			var exteriorCornerBackBuilder = new InternalCornerBuilder (corner);
			panelMeshFilter.mesh = exteriorCornerBackBuilder.Build();
			panelMeshRenderer.material = conersMaterial;
            externalCornersGameObjects.Add (id, panelGO);
			yield return null;
		}

		var k = 0;
		foreach (KeyValuePair<string, List<PositionRotation>> cornerback in report.InternallCorners)
		{
			string id = cornerback.Key;
			GameObject externalCorner = externalCornersGameObjects [id];
			foreach(PositionRotation transform in cornerback.Value)
			{
				Vector3 position = transform.Position;
				Quaternion rotation = Quaternion.Euler( transform.RotationV3) ;
				Instantiate(externalCorner, position, rotation);

				if(++k==5) {
					k = 0;
					yield return null;
				}
			}
		}
	}

	IEnumerator BuildExternalCorners (ModulationReport report)
	{
		Dictionary<string, GameObject> externalCornersGameObjects = new Dictionary<string, GameObject>();

		foreach (KeyValuePair<string, List<PositionRotation>> cornerback in report.ExternalCorners)
		{
			string id = cornerback.Key;
			float height = TestAllExternalCorners.GetByID(cornerback.Key).height;
			var panelGO = new GameObject(name, typeof(MeshRenderer), typeof(MeshFilter), typeof(BoxCollider), typeof(InnerCornerInfo));
			panelGO.name = id;
			panelGO.tag = "OutterCorner";
			var panelMeshFilter = panelGO.GetComponent<MeshFilter>();
			var panelMeshRenderer = panelGO.GetComponent<MeshRenderer>();
			var panelBoxCollider = panelGO.GetComponent<BoxCollider>();
			panelBoxCollider.center = new Vector3(0.025f, height / 2.0f, 0.025f);
			panelBoxCollider.size = new Vector3(0.05f, height, 0.05f);
			var info = panelGO.GetComponent<InnerCornerInfo>();
			info.id = cornerback.Key;
			info.size = 0.05f;
			info.height = height;
			var externalCornerBuilder = new ExternalCornerBuilder();
			var up = new Vector3 (0, 1, 0);
			var right = new Vector3 (-1, 0, 0);
			var corner = new RightCornerback (up, right, Vector3.zero, height);
			panelMeshFilter.mesh = externalCornerBuilder.Build(corner);
			panelMeshRenderer.material = conersMaterial;
			externalCornersGameObjects.Add (id, panelGO);
			yield return null;
		}

		var k = 0 ;
		foreach (KeyValuePair<string, List<PositionRotation>> cornerback in report.ExternalCorners)
		{
			string id = cornerback.Key;
			GameObject externalCorner = externalCornersGameObjects [id];
			foreach(PositionRotation transform in cornerback.Value)
			{
				Vector3 position = transform.Position;
                Quaternion rotation = Quaternion.Euler(transform.RotationV3);
                Instantiate(externalCorner, position, rotation);
				if(++k==5) {
					k = 0;
					yield return null;
				}
			}
		}
	}

	IEnumerator BuildWallPanels (ModulationReport report)
	{
		Dictionary<string, GameObject> wallPanelGameObjects = new Dictionary<string, GameObject>();

		foreach (KeyValuePair<string, List<PositionRotation>> cornerback in report.WallPanels)
		{
			var panel = TestAllPanels.GetByID(cornerback.Key);
			float height = panel.height;
			float width = panel.width;
			var panelGO = new GameObject(name, typeof(MeshRenderer), typeof(MeshFilter), typeof(BoxCollider), typeof(WallPanelInfo));
			panelGO.name = cornerback.Key;
			panelGO.tag = "WallPanel";
			var panelMeshFilter = panelGO.GetComponent<MeshFilter>();
			var panelMeshRenderer = panelGO.GetComponent<MeshRenderer>();
			var panelBoxCollider = panelGO.GetComponent<BoxCollider>();
			panelBoxCollider.center = new Vector3(panel.width / -2.0f, panel.height / 2.0f, 0.025f);
			panelBoxCollider.size = new Vector3(panel.width, panel.height, 0.05f);
			var info = panelGO.GetComponent<WallPanelInfo>();
			info.id = cornerback.Key;
			info.width = width;
			info.height = height;
			var up = new Vector3 (0, 1, 0);
			var right = new Vector3 (-1, 0, 0);
			var wallPanel = new WallPanel (Vector3.zero, up, width, height, cornerback.Key);
			var wallPanelBuilder = new WallPanelBuilder(wallPanel);
			panelMeshFilter.mesh = wallPanelBuilder.Build();
			panelMeshRenderer.material = wallPanelsMaterial;
			wallPanelGameObjects.Add (cornerback.Key, panelGO);
			yield return null;
		}

		foreach (KeyValuePair<string, List<PositionRotation>> cornerback in report.WallPanels)
		{
			string id = cornerback.Key;
			GameObject externalCorner = wallPanelGameObjects [id];
			int k = 0;
			foreach(PositionRotation transform in cornerback.Value)
			{
				Vector3 position = transform.Position;
                Quaternion rotation = Quaternion.Euler(transform.RotationV3);
                Instantiate(externalCorner, position, rotation);

				if(++k==5) {
					k = 0;
					yield return null;
				}
			}
		}
	}
    

}
