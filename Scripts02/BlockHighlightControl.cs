using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BlockHighlightControl : MonoBehaviour {

	public bool showHighlight = false;

	private Mesh highlighterMesh;
	private MeshCollider highlighterCollider;

	public string moduleRef;
	public string gridSqrRef;
	public Vector3 objOrigin;

	private Vector3[] highlighterVectors;
	private List<int> highlighterTriangles;
	private Vector2[] highlighterUVs;
	private Vector4[] highlighterTangents;

	// Use this for initialization
	void Start () {
	

		highlighterMesh = GetComponent<MeshFilter>().mesh;
		highlighterCollider = GetComponent<MeshCollider> ();

		highlighterVectors = new Vector3[8];
		highlighterTriangles = new List<int>();
		highlighterUVs = new Vector2[8];
		highlighterTangents = new Vector4[8];

		mapHighlighter ();

	}

	void mapHighlighter(){

		GameObject getModuleData = GameObject.Find ("ModuleData");
		LandscapeModuleData getData = getModuleData.GetComponent<LandscapeModuleData> ();

		//Plot vectors
		float xz = getData.blockDimensions.x; // Block width / depth
		float y = (getData.blockDimensions.y / 2) + 0.0005f; // Block height
		float ht = getData.blockDimensions.x / 10; // Highlighter Thickness

		//Outter Edge
		highlighterVectors [0] = new Vector3 (-(xz / 2), y, -(xz / 2));
		highlighterVectors [1] = new Vector3 (-(xz / 2), y, (xz / 2));
		highlighterVectors [2] = new Vector3 ((xz / 2), y, (xz / 2));
		highlighterVectors [3] = new Vector3 ((xz / 2), y, -(xz / 2));

		//Inner Edge
		highlighterVectors [4] = new Vector3 (-(xz / 2) + ht, y, -(xz / 2) + ht);
		highlighterVectors [5] = new Vector3 (-(xz / 2) + ht, y, (xz / 2) - ht);
		highlighterVectors [6] = new Vector3 ((xz / 2) - ht, y, (xz / 2) - ht);
		highlighterVectors [7] = new Vector3 ((xz / 2) - ht, y, -(xz / 2) + ht);

		//Map Triangles
		highlighterTriangles.Add (0);
		highlighterTriangles.Add (1);
		highlighterTriangles.Add (4);

		highlighterTriangles.Add (4);
		highlighterTriangles.Add (1);
		highlighterTriangles.Add (5);

		highlighterTriangles.Add (1);
		highlighterTriangles.Add (2);
		highlighterTriangles.Add (5);

		highlighterTriangles.Add (5);
		highlighterTriangles.Add (2);
		highlighterTriangles.Add (6);

		highlighterTriangles.Add (3);
		highlighterTriangles.Add (6);
		highlighterTriangles.Add (2);

		highlighterTriangles.Add (7);
		highlighterTriangles.Add (6);
		highlighterTriangles.Add (3);

		highlighterTriangles.Add (7);
		highlighterTriangles.Add (3);
		highlighterTriangles.Add (0);

		highlighterTriangles.Add (0);
		highlighterTriangles.Add (4);
		highlighterTriangles.Add (7);

		// Map UV's

		//Outter Edge
		highlighterTangents [0] = new Vector2 (-(xz / 2), -(xz / 2));
		highlighterTangents [1] = new Vector2 (-(xz / 2), (xz / 2));
		highlighterTangents [2] = new Vector2 ((xz / 2), (xz / 2));
		highlighterTangents [3] = new Vector2 ((xz / 2), -(xz / 2));
		
		//Inner Edge
		highlighterTangents [4] = new Vector2 (-(xz / 2) + ht, -(xz / 2) + ht);
		highlighterTangents [5] = new Vector2 (-(xz / 2) + ht, (xz / 2) - ht);
		highlighterTangents [6] = new Vector2 ((xz / 2) - ht, (xz / 2) - ht);
		highlighterTangents [7] = new Vector2 ((xz / 2) - ht,-(xz / 2) + ht);

		// Map Tangents

		//Outter Edge
		highlighterTangents [0] = new Vector4 (-(xz / 2), y, -(xz / 2),1);
		highlighterTangents [1] = new Vector4 (-(xz / 2), y, (xz / 2),1);
		highlighterTangents [2] = new Vector4 ((xz / 2), y, (xz / 2),1);
		highlighterTangents [3] = new Vector4 ((xz / 2), y, -(xz / 2),1);
		
		//Inner Edge
		highlighterTangents [4] = new Vector4 (-(xz / 2) + ht, y, -(xz / 2) + ht,1);
		highlighterTangents [5] = new Vector4 (-(xz / 2) + ht, y, (xz / 2) - ht,1);
		highlighterTangents [6] = new Vector4 ((xz / 2) - ht, y, (xz / 2) - ht,1);
		highlighterTangents [7] = new Vector4 ((xz / 2) - ht, y, -(xz / 2) + ht,1);

		MeshUpdate ();

	}

	void UpdatePosition(Vector3 cursorPos, string moduleName, string gridSqrNumber){

		this.objOrigin = cursorPos;
		this.moduleRef = moduleName;
		this.gridSqrRef = gridSqrNumber;

	}

	// Mesh redraw method, will be called to upon any terrain adjustments
	void MeshUpdate(){
		
		highlighterMesh.Clear ();
		highlighterMesh.vertices = highlighterVectors;
		highlighterMesh.triangles = highlighterTriangles.ToArray();
		highlighterMesh.uv = highlighterUVs;
		highlighterMesh.Optimize ();
		highlighterMesh.RecalculateNormals ();
		highlighterMesh.tangents = highlighterTangents;
		highlighterCollider.sharedMesh = highlighterMesh;

	}

	// Update is called once per frame
	void Update () {
	

		this.transform.position = this.objOrigin;

	}
}
