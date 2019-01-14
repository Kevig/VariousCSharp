using UnityEngine;
using System.Collections;

public class WorldMapping : MonoBehaviour {


	public int worldSize; // Public for in unity editor setting
	public float[] gridCoordinates; // 1D array for grid coordinates system
	public int[,] gridSquareState; // Grid state mapping array so objects can be assigned to that grid coordinate based on state
	public float floorHeight = 0.0f; // static floor height value
	public int totalGridSquares;

	private Vector3 worldOrigins; // static world origins vector
	private Vector3 startGridPosition; // Grid point mapping start value
	
	void Awake(){

		worldOrigins = new Vector3 (0.0f,floorHeight,0.0f); // Setting Origin Point of world

		// Calculating grid start coordinate, based on each grid square using a scale value of 1
		startGridPosition = new Vector3 (((worldOrigins.x + 0.5f) - (worldSize / 2)), worldOrigins.y, ((worldOrigins.z + 0.5f) - (worldSize / 2)));
		//

		BuildWorldCoordinatesTable ();

	}

	// Builds 2 tables, gridCoordinates table is 1D, size is defined by worldSize
	// will act as a 2D table for coordinates system.
	// gridSquareState table is total grid size -1 to account for loops starting at 0
	// assigns a starting state value to each grid square, start state 0 = empty;
	void BuildWorldCoordinatesTable() {

		totalGridSquares = (worldSize * worldSize) - 1;

		gridCoordinates = new float[worldSize];
		gridSquareState = new int[totalGridSquares,totalGridSquares];

		for (int i = 0; i < worldSize; i++) {

			gridCoordinates[i] = startGridPosition.x + i;

		}

		for (int iz = 0; iz < worldSize; iz++) {

			for (int ix = 0; ix < worldSize; ix++) {

					gridSquareState[ix,iz] = 0;

			}
		}
	}

}
