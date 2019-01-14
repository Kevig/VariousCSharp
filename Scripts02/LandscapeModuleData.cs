using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LandscapeModuleData : MonoBehaviour {

	public Vector2 blockDimensions;
	public Vector2 moduleDimensions;

	public int totalModules;
	private int moduleCount = 0;

	private Dictionary<string, int> moduleRef = new Dictionary<string, int> ();

	public List<float>moduleCoordsX;
	public List<float>moduleCoordsZ;

	private Vector2 startCoords;
	private Vector2 spawnCoords;

	void Start(){

		startCoords = new Vector2 (0 + (moduleDimensions.x / 2),0 + (moduleDimensions.x / 2));
		spawnCoords = startCoords;

		//deployModule ();

	}

	void deployModule(){

		moduleCoordsX = new List<float> ();
		moduleCoordsZ = new List<float> ();

		int countX = 1;
		int countZ = 1;

		float modulesPerRow = Mathf.Sqrt (totalModules);

		for (int i = 1; i < totalModules + 1; i++) {
	
			string moduleName = "Module" + moduleCount;
			moduleRef.Add (moduleName, moduleCount);
			moduleCoordsX.Add (spawnCoords.x);
			moduleCoordsZ.Add (spawnCoords.y);

			if (countX != modulesPerRow) {
				spawnCoords.x += moduleDimensions.x;
			} else {
				countZ++;
				countX = 0;
				spawnCoords.y += moduleDimensions.x;
				spawnCoords.x = startCoords.x;
			}
			countX++;
			moduleCount++;
		}
	}


}
