using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LandscapeModule : MonoBehaviour {

	public string landscapeModuleID;	// Name reference for this Landscape Module
	
	private float[] gridPoints;			// 1 row grid coordinates for x / z axis
	private float[] gridLayers;			// 1 row grid coordinates for y axis (height)
	private Vector3[] floorVectors;
	private List<int> floorTriangles;
	private Vector2[] floorUVs;
	private Vector4[] intFloorTangents;

	public Mesh moduleMesh;
	public MeshCollider moduleMeshCollider;

	// Pre-Initialisation
	void Awake () {

		this.landscapeModuleID = this.gameObject.name + Random.Range (0, 10000);
		moduleMesh = GetComponent<MeshFilter>().mesh;
		moduleMeshCollider = GetComponent<MeshCollider> ();
	}

	// Called once upon Creation
	void Start () {

		GameObject getModuleData = GameObject.Find ("ModuleData");
		LandscapeModuleData getData = getModuleData.GetComponent<LandscapeModuleData> ();

		// Assign size of coordinate storage arrays
		this.gridPoints = new float[2];
		this.gridLayers = new float[2];

		float i = 0 - ((getData.moduleDimensions.x / 2) * getData.blockDimensions.x); // Starting point value

		// Populate array for x/z coordinates needed
		this.gridPoints [0] = i;
		this.gridPoints [1] = i + (getData.moduleDimensions.x * getData.blockDimensions.x);

		// Populate array for heights
		this.gridLayers [0] = 0;
		this.gridLayers [1] = 0 + (getData.blockDimensions.y / 8);


		buildFloorBlocks ();

	}

	void buildFloorBlocks(){

		GameObject getModuleData = GameObject.Find ("ModuleData");
		LandscapeModuleData getData = getModuleData.GetComponent<LandscapeModuleData> ();

		// Set arrays to sizes needed
		floorVectors = new Vector3[8];
		floorUVs = new Vector2[8];
		intFloorTangents = new Vector4[8];

		floorTriangles = new List<int> ();

		// Map each cube's vectors and tangents


		floorVectors [0] = new Vector3 (gridPoints[0], gridLayers[0], gridPoints[0]);
		floorVectors [1] = new Vector3 (gridPoints[0], gridLayers[0], gridPoints[1]);
		floorVectors [2] = new Vector3 (gridPoints[1], gridLayers[0], gridPoints[1]);
		floorVectors [3] = new Vector3 (gridPoints[1], gridLayers[0], gridPoints[0]);

		floorVectors [4] = new Vector3 (gridPoints[0], gridLayers[1], gridPoints[0]);
		floorVectors [5] = new Vector3 (gridPoints[0], gridLayers[1], gridPoints[1]);
		floorVectors [6] = new Vector3 (gridPoints[1], gridLayers[1], gridPoints[1]);
		floorVectors [7] = new Vector3 (gridPoints[1], gridLayers[1], gridPoints[0]);

		intFloorTangents [0] = new Vector4 (gridPoints[0], gridLayers[0], gridPoints[0], 1);
		intFloorTangents [1] = new Vector4 (gridPoints[0], gridLayers[0], gridPoints[1], 1);
		intFloorTangents [2] = new Vector4 (gridPoints[1], gridLayers[0], gridPoints[1], 1);
		intFloorTangents [3] = new Vector4 (gridPoints[1], gridLayers[0], gridPoints[0], 1);
		
		intFloorTangents [4] = new Vector4 (gridPoints[0], gridLayers[1], gridPoints[0], 1);
		intFloorTangents [5] = new Vector4 (gridPoints[0], gridLayers[1], gridPoints[1], 1);
		intFloorTangents [6] = new Vector4 (gridPoints[1], gridLayers[1], gridPoints[1], 1);
		intFloorTangents [7] = new Vector4 (gridPoints[1], gridLayers[1], gridPoints[0], 1);


		floorUVs[0] = new Vector2 (gridPoints[0], gridPoints[0]);
		floorUVs[1] = new Vector2 (gridPoints[0], gridPoints[1]);
		floorUVs[2] = new Vector2 (gridPoints[1], gridPoints[1]);
		floorUVs[3] = new Vector2 (gridPoints[1], gridPoints[0]);
		floorUVs[4] = new Vector2 (gridPoints[0], gridPoints[0]);
		floorUVs[5] = new Vector2 (gridPoints[0], gridPoints[1]);
		floorUVs[6] = new Vector2 (gridPoints[1], gridPoints[1]);
		floorUVs[7] = new Vector2 (gridPoints[1], gridPoints[0]);

		// Map triangles

		// Base
		// Triangle 1
		floorTriangles.Add(2);
		floorTriangles.Add(1);
		floorTriangles.Add(0);

		// Triangle 2
		floorTriangles.Add(0);
		floorTriangles.Add(3);
		floorTriangles.Add(2);

		// Top
		// Triangle 1
		floorTriangles.Add(4);
		floorTriangles.Add(5);
		floorTriangles.Add(6);

		// Triangle 2
		floorTriangles.Add(6);
		floorTriangles.Add(7);
		floorTriangles.Add(4);

		//Front side
		// Triangle 1
		floorTriangles.Add(0);
		floorTriangles.Add(4);
		floorTriangles.Add(7);

		// Triangle 2
		floorTriangles.Add(7);
		floorTriangles.Add(3);
		floorTriangles.Add(0);

		//Left side
		// Triangle 1
		floorTriangles.Add(1);
		floorTriangles.Add(5);
		floorTriangles.Add(0);
	
		// Triangle 2
		floorTriangles.Add(5);
		floorTriangles.Add(4);
		floorTriangles.Add(0);


		//Back side
		// Triangle 1
		floorTriangles.Add(6);
		floorTriangles.Add(5);
		floorTriangles.Add(1);
		
		// Triangle 2
		floorTriangles.Add(1);
		floorTriangles.Add(2);
		floorTriangles.Add(6);

		//Right side
		// Triangle 1
		floorTriangles.Add(7);
		floorTriangles.Add(6);
		floorTriangles.Add(2);
	
		// Triangle 2
		floorTriangles.Add(2);
		floorTriangles.Add(3);
		floorTriangles.Add(7);

		MeshUpdate ();

	}

	// Called once per frame
	void Update () {
	


	}

	// Mesh redraw method, will be called to upon any terrain adjustments
	void MeshUpdate(){
		
		moduleMesh.Clear ();
		moduleMesh.vertices = floorVectors;
		moduleMesh.triangles = floorTriangles.ToArray();
		moduleMesh.uv = floorUVs;
		moduleMesh.Optimize ();
		moduleMesh.RecalculateNormals ();
		moduleMesh.tangents = intFloorTangents;
		moduleMeshCollider.sharedMesh = moduleMesh;
	}
}
