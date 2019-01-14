using UnityEngine;
using System.Collections;

public class PortalPlacement : MonoBehaviour {

	public GameObject playerPortal;

	public int portalX;
	public int portalZ;

	public bool playerPortalReady = false;


	void Awake(){

		// Create an Object instance of Class worldMapping and get component"script" so variables can be used.
		GameObject wMapping = GameObject.FindWithTag ("worldMapping");
		WorldMapping worldMapping = wMapping.GetComponent <WorldMapping> ();
		
		portalX = Random.Range (3, worldMapping.worldSize - 3);
		portalZ = Random.Range (3, (worldMapping.worldSize / 4));
		
		Vector3 floorGridPosition = new Vector3 (worldMapping.gridCoordinates[portalX], worldMapping.floorHeight, worldMapping.gridCoordinates[portalZ]);
		GameObject PlayerPortal = (GameObject)Instantiate (playerPortal, floorGridPosition,Quaternion.identity);
		PlayerPortal.name = "playerPortal";
		
		playerPortalReady = true;
		

	
	}

	// Use this for initialization
	void Start () {
	
	
	}
	

}
