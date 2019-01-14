using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class worldMap : MonoBehaviour {

	public Dictionary <string, string> gridMap; // Dictionary containing grid map, key is "Grid" + square number reference, contains material
	
	private string materialRef; // Name of material for grid square block
	private string gridSqrRef; // Reference of the grid square

	void Awake()
	{

		MapGrid ();

	}

	// Using random rolls and a percentage chance of different materials, build a material dictionary for the grid
	void MapGrid()
	{

		GameObject getData = GameObject.Find ("WorldData");
		worldData getWorldData = getData.GetComponent<worldData> ();

		int totalGridSquares = getWorldData.worldSize * getWorldData.worldSize;

		gridMap = new Dictionary<string, string> (totalGridSquares);

		for (int i = 0; i < totalGridSquares; i++) 
		{

			gridSqrRef = "Grid" + (i+1);

			int materialRange = Random.Range(1,100);

			if (materialRange >= 1 && materialRange <= 55)
			{
				materialRef = "dirt";
			}

			if (materialRange > 55 && materialRange <= 75)
			{
				materialRef = "stone";
			}

			if (materialRange > 75 && materialRange <= 85)
			{
				materialRef = "coal";
			}

			if (materialRange > 85 && materialRange <= 95)
			{
				materialRef = "iron";
			}

			if (materialRange > 95 && materialRange <= 99)
			{
				materialRef = "gold";
			}

			if (materialRange == 100)
			{
				materialRef = "gems";
			}

			gridMap.Add(gridSqrRef,materialRef);

		}

	}



}
