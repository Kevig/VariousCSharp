using UnityEngine;
using System.Collections;

public class WorldBuilder : MonoBehaviour {
	
	public GameObject floorTile;
	public GameObject WorldGrid;

	void Start() {

		BuildWorldFloor ();
	}


	void Update () {
	

	}

	void BuildWorldFloor(){
	
		// Create an Object instance of Class worldMapping and get component"script" so variables can be used.
		GameObject wMapping = GameObject.FindWithTag ("worldMapping");
		WorldMapping worldMapping = wMapping.GetComponent <WorldMapping> ();

		int floorTileNumber = 0; 

		for (int iz = 0; iz < worldMapping.worldSize; iz++) {
			
			for (int ix = 0; ix < worldMapping.worldSize; ix++) {

				// Create a new floorTile @ position mapped out in worldMapping Class
				Vector3 floorGridPosition = new Vector3 (worldMapping.gridCoordinates[ix], worldMapping.floorHeight - 0.06125f, worldMapping.gridCoordinates[iz]);
				GameObject FloorTile = (GameObject)Instantiate (floorTile, floorGridPosition,Quaternion.identity);

				// Rename tile to tile number - x,z coordinates - floortile and parent to empty WorldGrid object
				FloorTile.name = floorTileNumber +" "+ worldMapping.gridCoordinates[ix] +" "+ worldMapping.gridCoordinates[iz] +" FloorTile";
				FloorTile.transform.parent = WorldGrid.transform;
				floorTileNumber++;
			}
		}

	}
}
