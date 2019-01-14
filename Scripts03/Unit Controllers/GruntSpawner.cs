using UnityEngine;
using System.Collections;

public class GruntSpawner : MonoBehaviour {

	public GameObject Grunt;

	public int gruntCounter = 0;


	public Vector3 gruntSpawnPos;


	// Use this for initialization
	void Start () {
	
		GameObject wMapping = GameObject.FindWithTag ("worldMapping");
		WorldMapping worldMapping = wMapping.GetComponent <WorldMapping> ();
		
		GameObject portal = GameObject.FindWithTag ("playerPortal");
		PortalPlacement portalPlacement = portal.GetComponent<PortalPlacement>();
		
		Vector3 portaltemp = new Vector3(worldMapping.gridCoordinates[portalPlacement.portalX], worldMapping.floorHeight, worldMapping.gridCoordinates[portalPlacement.portalZ]);

		gruntSpawnPos = new Vector3 (portaltemp.x + 1.5f, portaltemp.y + 0.25f, portaltemp.z);

		SpawnNewGrunt ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void SpawnNewGrunt(){
		
		GameObject grunt = (GameObject)(GameObject)Instantiate (Grunt, gruntSpawnPos, Quaternion.identity);
		gruntCounter++;
		grunt.name = "grunt";
		
	}

}
