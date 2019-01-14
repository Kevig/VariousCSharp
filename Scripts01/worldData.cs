using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class worldData : MonoBehaviour {

	public int worldSize;
	public Vector2 gridSqrSize;
	
	public Material dirt;
	public Material stone;
	public Material coal;
	public Material iron;
	public Material gold;
	public Material gems;

	private float[] gridPoints;
	private int gridSqrVectorSize;
	
	public Dictionary<string, List<Vector3>> gridSqrVectors;

	public delegate void meshMap();
	public static event meshMap OnCreation;

	void Start()
	{

		gridSqrVectorSize = (worldSize * worldSize) * 8;

		MapGridPoints ();

	}

	void MapGridPoints()
	{

		gridPoints = new float[worldSize + 1];
		
		for (int i = 0; i < worldSize + 1; i++) 
		{
		
			gridPoints[i] = (0-((worldSize / 2) * gridSqrSize.x)) + (gridSqrSize.x * i);
		
		}

		MapGridSqrVectors ();
	}

	void MapGridSqrVectors()
	{

		gridSqrVectors = new Dictionary<string, List<Vector3>> (gridSqrVectorSize);

		int gridSgrCounter = 0;

		// Loop z values of grid
		for (int z = 0; z < worldSize; z++) 
		{

			// Loop x values of grid 
			for (int x = 0; x < worldSize; x++)
			{

				gridSgrCounter++;
				string gridRef = "Grid" + gridSgrCounter; // Dictionary Key

				// Create new list under assigned Key
				gridSqrVectors.Add(gridRef, new List<Vector3>(8));

				// Add coordinates for each block, blocks are constructed from ground level and points mapped clockwise

				gridSqrVectors[gridRef].Add(new Vector3(gridPoints[x],0.0f, gridPoints[z])); // Point 1 floor level
				gridSqrVectors[gridRef].Add(new Vector3(gridPoints[x],0.0f, gridPoints[z+1])); // Point 2 floor level
				gridSqrVectors[gridRef].Add(new Vector3(gridPoints[x+1],0.0f, gridPoints[z+1])); // Point 3 floor level
				gridSqrVectors[gridRef].Add(new Vector3(gridPoints[x+1],0.0f, gridPoints[z])); // Point 4 floor level

				gridSqrVectors[gridRef].Add(new Vector3(gridPoints[x], gridSqrSize.y, gridPoints[z])); // Point 1 ceiling level
				gridSqrVectors[gridRef].Add(new Vector3(gridPoints[x], gridSqrSize.y, gridPoints[z+1])); // Point 2 ceiling level
				gridSqrVectors[gridRef].Add(new Vector3(gridPoints[x+1], gridSqrSize.y, gridPoints[z+1])); // Point 3 ceiling level
				gridSqrVectors[gridRef].Add(new Vector3(gridPoints[x+1], gridSqrSize.y, gridPoints[z])); // Point 4 ceiling level

				// Testing output
				//foreach (var output in gridSqrVectors[gridRef])
				//{
				//	Debug.Log(gridRef + " : " + output);
				//}
				// Remove in final version
			}
		}
	}
}
