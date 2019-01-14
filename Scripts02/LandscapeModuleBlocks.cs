using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LandscapeModuleBlocks : MonoBehaviour {
	
	public string landscapeModuleID;	// Name reference for this Landscape Module

	private Material[] blockType; 

	public Material dirt;
	public Material stone;
	public Material coal;
	public Material iron;
	public Material gold;
	public Material gems;

	private float[] gridPoints;			// 1 row grid coordinates for x / z axis
	private float[] gridLayers;			// 1 row grid coordinates for y axis (height)
	private Vector3[] blockVectors;
	private Vector2[] blockUVs;
	private Vector4[] blockTangents;
	private Dictionary<string,List<int>> trianglesTable;
	private int blockCounter;

	public Mesh blockMesh;
	public MeshCollider blockMeshCollider;
	public int[] blockMap;



	// Pre-Initialisation
	void Awake () {
		
		this.landscapeModuleID = this.gameObject.name + Random.Range (0, 10000);
		blockMesh = GetComponent<MeshFilter>().mesh;
		blockMeshCollider = GetComponent<MeshCollider> ();
	}
	
	// Called once upon Creation
	void Start () {
		
		GameObject getModuleData = GameObject.Find ("ModuleData");
		LandscapeModuleData getData = getModuleData.GetComponent<LandscapeModuleData> ();
		
		// Convert changable module size values into int's for setting array sizes
		int arraySizeX = System.Convert.ToInt16 (getData.moduleDimensions.x);
		int arraySizeY = System.Convert.ToInt16 (getData.moduleDimensions.y);
		
		// Assign size of coordinate storage arrays
		this.gridPoints = new float[arraySizeX + 1];
		this.gridLayers = new float[arraySizeY + 1];
		
		float i = 0 - ((getData.moduleDimensions.x / 2) * getData.blockDimensions.x); // Starting point value
		
		// Populate array for x/z coordinates needed
		for (int j = 0; j < getData.moduleDimensions.x + 1; j++) {
			this.gridPoints[j] = i + (getData.blockDimensions.x * j);
		}
		
		i = 0;// Starting layer value
		
		// Populate array for heights
		for (int j = 0; j < getData.moduleDimensions.y + 1; j++) {
			this.gridLayers[j] = i + ((getData.blockDimensions.y / 8)*j);
		}
		
		buildFloorBlocks ();
		
	}
	
	void buildFloorBlocks(){
		
				GameObject getModuleData = GameObject.Find ("ModuleData");
				LandscapeModuleData getData = getModuleData.GetComponent<LandscapeModuleData> ();
		
				// Initialise counters
				int x1;
				int x2;
				int z1 = 0;
				int z2 = 1;
				int y1 = 1;
				int y2 = 9;
				int l = 0;
		
				// Convert dimension values to int's for array size setting
				int arraySize = System.Convert.ToInt32 ((getData.moduleDimensions.x * getData.moduleDimensions.x) * 8);
		
				// Set arrays to sizes needed
				blockVectors = new Vector3[arraySize];
				blockUVs = new Vector2[arraySize];
				blockTangents = new Vector4[arraySize];

				trianglesTable = new Dictionary<string, List<int>> ();

				// Map each cube's vectors and tangents
				for (int i = 0; i < getData.moduleDimensions.x; i++) {
			
						x1 = 0;
						x2 = 1;
			
						for (int j = 0; j < getData.moduleDimensions.x; j++) {
				
								blockVectors [l] = new Vector3 (gridPoints [x1], gridLayers [y1], gridPoints [z1]);
								blockVectors [l + 1] = new Vector3 (gridPoints [x1], gridLayers [y1], gridPoints [z2]);
								blockVectors [l + 2] = new Vector3 (gridPoints [x2], gridLayers [y1], gridPoints [z2]);
								blockVectors [l + 3] = new Vector3 (gridPoints [x2], gridLayers [y1], gridPoints [z1]);
				
								blockVectors [l + 4] = new Vector3 (gridPoints [x1], gridLayers [y2], gridPoints [z1]);
								blockVectors [l + 5] = new Vector3 (gridPoints [x1], gridLayers [y2], gridPoints [z2]);
								blockVectors [l + 6] = new Vector3 (gridPoints [x2], gridLayers [y2], gridPoints [z2]);
								blockVectors [l + 7] = new Vector3 (gridPoints [x2], gridLayers [y2], gridPoints [z1]);
				
								blockTangents [l] = new Vector4 (gridPoints [x1], gridLayers [y1], gridPoints [z1], 1);
								blockTangents [l + 1] = new Vector4 (gridPoints [x1], gridLayers [y1], gridPoints [z2], 1);
								blockTangents [l + 2] = new Vector4 (gridPoints [x2], gridLayers [y1], gridPoints [z2], 1);
								blockTangents [l + 3] = new Vector4 (gridPoints [x2], gridLayers [y1], gridPoints [z1], 1);
				
								blockTangents [l + 4] = new Vector4 (gridPoints [x1], gridLayers [y2], gridPoints [z1], 1);
								blockTangents [l + 5] = new Vector4 (gridPoints [x1], gridLayers [y2], gridPoints [z2], 1);
								blockTangents [l + 6] = new Vector4 (gridPoints [x2], gridLayers [y2], gridPoints [z2], 1);
								blockTangents [l + 7] = new Vector4 (gridPoints [x2], gridLayers [y2], gridPoints [z1], 1);
				
				
								blockUVs [l] = new Vector2 (gridPoints [x1], gridPoints [z1]);
								blockUVs [l + 1] = new Vector2 (gridPoints [x1], gridPoints [z2]);
								blockUVs [l + 2] = new Vector2 (gridPoints [x2], gridPoints [z2]);
								blockUVs [l + 3] = new Vector2 (gridPoints [x2], gridPoints [z1]);
								blockUVs [l + 4] = new Vector2 (gridPoints [x1], gridPoints [z1]);
								blockUVs [l + 5] = new Vector2 (gridPoints [x1], gridPoints [z2]);
								blockUVs [l + 6] = new Vector2 (gridPoints [x2], gridPoints [z2]);
								blockUVs [l + 7] = new Vector2 (gridPoints [x2], gridPoints [z1]);
				
								x1++;
								x2++;
								l += 8;
			}
			
			z1++;
			z2++;
		}

		mapBlocks ();
	}
		
	void mapTriangles(){

		GameObject getModuleData = GameObject.Find ("ModuleData");
		LandscapeModuleData getData = getModuleData.GetComponent<LandscapeModuleData> ();

		int c = 0; // counter for triangle memory locations
		blockCounter = -1; // block counter


		// Map triangles
		for (int i = 0; i < getData.moduleDimensions.x ; i++) {
			
			for (int j = 0; j < getData.moduleDimensions.x; j++){

				blockCounter++;
				string blockID = "block" + blockCounter;

				trianglesTable.Add(blockID,new List<int>());

				if (blockMap[blockCounter] != 0){
				
					bool drawFront = new bool();
					bool drawBack = new bool();
					bool drawLeft = new bool();
					bool drawRight = new bool();
					
					if (blockCounter > getData.moduleDimensions.x){
						int tempRef = blockCounter - (System.Convert.ToInt16(getData.moduleDimensions.x));
						if(blockMap[tempRef] != 0){
							drawFront = false;
						} else {
							drawFront = true;
						}
					} else {
						drawFront = true;
					}

					if (blockCounter < ((getData.moduleDimensions.x * getData.moduleDimensions.x) - getData.moduleDimensions.x)){
						int tempRef = blockCounter + (System.Convert.ToInt16(getData.moduleDimensions.x));
						if(blockMap[tempRef] != 0){
							drawBack = false;
						}else {
							drawBack = true;
						}
					} else {
						drawBack = true;
					}

					if (j > 0){
						if (blockMap[blockCounter-1] != 0){
							drawLeft = false;
						} else {
							drawLeft = true;
						}
					} else {
						drawLeft = true;
					}

					if (j < getData.moduleDimensions.x - 1){
						if (blockMap[blockCounter+1] != 0){
							drawRight = false;
						} else {
							drawRight = true;
						}
					} else {
						drawRight = true;
					}


				// Top
				// Triangle 1
				trianglesTable[blockID].Add(c + 4);
				trianglesTable[blockID].Add(c + 5);
				trianglesTable[blockID].Add(c + 6);

				// Triangle 2
				trianglesTable[blockID].Add(c + 6);
				trianglesTable[blockID].Add(c + 7);
				trianglesTable[blockID].Add(c + 4);
				
				if (drawFront){
					//Front side
					// Triangle 1
					trianglesTable[blockID].Add(c);
					trianglesTable[blockID].Add(c + 4);
					trianglesTable[blockID].Add(c + 7);

					// Triangle 2
					trianglesTable[blockID].Add(c + 7);
					trianglesTable[blockID].Add(c + 3);
					trianglesTable[blockID].Add(c);
				}
				
				if (drawLeft){
					//Left side
					// Triangle 1
					trianglesTable[blockID].Add(c + 1);
					trianglesTable[blockID].Add(c + 5);
					trianglesTable[blockID].Add(c);
				
					// Triangle 2
					trianglesTable[blockID].Add(c + 5);
					trianglesTable[blockID].Add(c + 4);
					trianglesTable[blockID].Add(c);
				}

				if (drawBack){
					//Back side
					// Triangle 1
					trianglesTable[blockID].Add(c + 6);
					trianglesTable[blockID].Add(c + 5);
					trianglesTable[blockID].Add(c + 1);

					// Triangle 2
					trianglesTable[blockID].Add(c + 1);
					trianglesTable[blockID].Add(c + 2);
					trianglesTable[blockID].Add(c + 6);
				}
				
				if (drawRight){
					//Right side
					// Triangle 1
					trianglesTable[blockID].Add(c + 7);
					trianglesTable[blockID].Add(c + 6);
					trianglesTable[blockID].Add(c + 2);

					// Triangle 2
					trianglesTable[blockID].Add(c + 2);
					trianglesTable[blockID].Add(c + 3);
					trianglesTable[blockID].Add(c + 7);
				}
				
				}

				c += 8;
				
			}
			
		}

		blockCheck ();
	}
	
	// Called once per frame
	void Update (){
		

		
	}

	void mapBlocks(){

		GameObject getModuleData = GameObject.Find ("ModuleData");
		LandscapeModuleData getData = getModuleData.GetComponent<LandscapeModuleData> ();

		blockMap = new int[System.Convert.ToInt16(getData.moduleDimensions.x * getData.moduleDimensions.x)];

		for (int i = 0; i < (getData.moduleDimensions.x * getData.moduleDimensions.x); i++) {

			int type = Random.Range (0, 24);

			if (type == 0 || type == 1 || type == 2 || type == 3 || type == 22 || type == 20 || type == 18) {
					blockMap [i] = 0; //No Block
			}
			if (type == 4 || type == 5 || type == 6 || type == 7 || type == 8 || type == 9 || type == 10 || type == 15 || type == 16 || type == 14) {
					blockMap [i] = 1; //Dirt Block
			}
			if (type == 11 || type == 12 || type == 13) {
					blockMap [i] = 2; //Stone Block
			}
			if (type == 17) {
					blockMap [i] = 3; //Coal Block
			}
			if (type == 19) {
					blockMap [i] = 4; //Iron Block
			}
			if (type == 21) {
					blockMap [i] = 5; //Gold Block
			}
			if (type == 23){
					blockMap [i] = 6; //Diamond Block
			}
		}
	
		mapTriangles ();

	}

	void blockCheck(){

		blockType = new Material[blockCounter+1];

		for (int i = 0; i < blockCounter+1; i++) {

			if (blockMap[i] == 1){
				blockType[i] = dirt;
			}
			if (blockMap[i] == 2){
				blockType[i] = stone;
			}
			if (blockMap[i] == 3){
				blockType[i] = coal;
			}
			if (blockMap[i] == 4){
				blockType[i] = iron;
			}
			if (blockMap[i] == 5){
				blockType[i] = gold;
			} 
			if (blockMap[i] == 6){
				blockType[i] = gems;
			}
		}
		
		MeshUpdate ();
	}


	// Mesh redraw method, will be called to upon any terrain adjustments
	void MeshUpdate(){

		blockMesh.Clear ();
		blockMesh.vertices = blockVectors;
		blockMesh.subMeshCount = blockCounter + 1;
		renderer.sharedMaterials = blockType;

		for (int i = 0; i < blockCounter + 1; i++){
		
		blockMesh.SetTriangles (trianglesTable ["block"+i].ToArray (), i);	
		}

		blockMesh.uv = blockUVs;
		blockMesh.Optimize ();
		blockMesh.RecalculateNormals ();
		blockMesh.tangents = blockTangents;
		blockMeshCollider.sharedMesh = blockMesh;

				
	
	}
}
