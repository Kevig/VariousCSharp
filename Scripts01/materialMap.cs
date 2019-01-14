using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class materialMap : MonoBehaviour {
	
	// Constructor
	private List<int> blockNumbers;
	private int blockNumbersLength;
	private Dictionary <String, List<Vector3>> materialMapVectors;
	private Dictionary <String, List<int>> materialMapTriangles;
		
	void Start()
	{
		// Enable Listener for Creation Event
		worldData.OnCreation += this.CheckBlockMap;
			
	}
		
	void CheckBlockMap()
	{
			
		// Get block map data from world map class
		GameObject getData = GameObject.Find (this.gameObject.name);
		worldMap WorldMap = getData.GetComponent<worldMap>();

		blockNumbers = new List<int> ();

		// Itterate blockMap, find which material is assigned to which mesh and add to blockNumbers List
		foreach(var gridSQRNumber in worldMap.gridMap)
		{
			if (worldMap.gridMap.Values == this.gameObject.name)
			{
				this.blockNumbersLength++;
				this.blockNumbers.Add (System.Convert.ToInt16(gridSQRNumber.Key.Substring("D")));
			}
				
		}
		this.BuildVectors();
	}
		 
		// Build Material Mesh Vectors Dictionary
		
	void BuildVectors()
	{
			
		// Reference Object where Grid vector data is stored 
		GameObject getData = GameObject.Find ("worldData");
		worldData WorldData = getData.GetComponent<worldData>();
			
			materialMapVectors = new Dictionary<string, List<Vector3>>();
			
			// Loop to compile materials Vector List
			for (int i = 0; i < this.blockNumbersLength; i++)
			{
				
				// Grid Square reference Name
				string gridRef = “Grid” + this.blockNumbers[i];
				
				// Create new Dictionary Entry.
				// May have to add exception handler after blockMap changing takes effect
				
				This.materialMapVectors.Add(gridRef, New List<Vector3>(8));
				
				// If Grid Vector dictionary contains this grid square “Which it should”, pull info and map Vectors to // this material mesh vectors
				
				vectorList<vector3> = new list <vector3>();
				
				if (worldData.gridSqrVectors.ContainsKey(gridRef, out vectorList)
				    {
					
					this.materialMapVectors[gridRef].Add(worldData.gridSqrVectors[gridRef](vectorList[0]);
					this.materialMapVectors[gridRef].Add(worldData.gridSqrVectors[gridRef](vectorList[1]);
					this.materialMapVectors[gridRef].Add(worldData.gridSqrVectors[gridRef](vectorList[2]);
					this.materialMapVectors[gridRef].Add(worldData.gridSqrVectors[gridRef](vectorList[3]);
					this.materialMapVectors[gridRef].Add(worldData.gridSqrVectors[gridRef](vectorList[4]);
					this.materialMapVectors[gridRef].Add(worldData.gridSqrVectors[gridRef](vectorList[5]);
					this.materialMapVectors[gridRef].Add(worldData.gridSqrVectors[gridRef](vectorList[6]);
					this.materialMapVectors[gridRef].Add(worldData.gridSqrVectors[gridRef](vectorList[7]);
					                                     
					}
				 }
			This.TriangleMapping(); // Call tri mapping method
			This.OtherElementsMapping(); // Call tangent and UV mapping methods
			} 
			// Empty block check for Tri drawing. Need to check 4 blocks surrounding current block and map tri’s based on result.
					                                     
	void TriangleMapping()
	{
						// Get block map data from world map class
						GameObject getData = GameObject.Find (“WorldData”);
						worldMap WorldMap = getData.GetComponent<worldMap>();
						
						bool drawFront ; // 0,3,7,4 – Memory Reference Locations
						bool drawLeft; // 0,1,5,4
						bool drawRight; // 2,3,6,7
						bool drawback; // 1,2, 5,6
						
						for (int i =0; i < this.blockNumbers; i++)
						{
							
							// NEED TO ADD EXCEPTION HANDLING FOR WORLD EDGES!!!
							
							// Conditional Check Left Block
							If (var result as value in worldMap.gridMap.containsKey(“Grid”+(blockNumbers[i]-1))
							    {
								If (result != “Empty”)
								{
									drawLeft = false;
								} else {
									drawLeft = true;
								}
							} 
							
							// Conditional Check Right Block
							If (var result as value in worldMap.gridMap.containsKey(“Grid”+(blockNumbers[i]+1))
							    {
								If (result != “Empty”)
								{
									drawRight = false;
								} else {
									drawRight = true;
								}
							}
							
							// Conditional Check front Block
							If (var result as value in worldMap.gridMap.containsKey(“Grid”+(blockNumbers[i]-10))
							    {
								If (result != “Empty”)
								{
									drawFront = false;
								} else {
									drawFront = true;
								}
							}
							// Conditional Check back Block
							If (var result as value in worldMap.gridMap.containsKey(“Grid”+(blockNumbers[i]+10))
							    {
								If (result != “Empty”)
								{
									drawBack = false;
								} else {
									drawBack = true;
								}
							}
						}
							// EXCEPTIONS NEED TO BE HANDLED AT CONDITIONAL LEVEL…
							
							// if the block doesn’t exist in the dictionary then its drawstate will always be true… that handles exceptions.
							
							// Exception notes, exceptions are any blocks with a value of worldSize in multiples
							// Any block with a reference lower than a value of worldSize and greater than 0
							// Any block with a reference higher than (worldSize * worldSize) – worldSize
							// Any block on the 0 / Z Axis 
							
							// Use control Booleans to build materialMapTriangles diction, add relevant grid reference and int values of vector memory locations, to form each polygon.


}
