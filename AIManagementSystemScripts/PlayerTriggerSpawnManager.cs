// "PlayerTriggerSpawnManager"
// Date: 17/03/2014
// Scripted by: Kevin Groundwater
// Description: Spawns required amount of collider objects from prefabs of these objects
// Receives broadcast of player name from selection manager loop and names the objects with
// the playerunits name.
using UnityEngine;
using System.Collections;

public class PlayerTriggerSpawnManager : MonoBehaviour {

	public GameObject ThreatTrigger; //Reference to prafabs
	public GameObject MeleeTrigger;	//

	// Initialization
	void Start () {
		// Listener for broadcasts from selection manager	
		SelectionManager.SetTriggerZones += this.CreateTriggerZones;

	}
	// Method to Create Objects, called upon selection manager broadcasts
	void CreateTriggerZones(string playerID){

		// creates 2 new gameobjects and renames them to the name of the playerunit there created for
		GameObject placeZone1 = (GameObject) Instantiate (ThreatTrigger, this.gameObject.transform.position, this.gameObject.transform.rotation);
		placeZone1.name = playerID;
		GameObject placeZone2 = (GameObject) Instantiate (MeleeTrigger, this.gameObject.transform.position, this.gameObject.transform.rotation);
		placeZone2.name = playerID;
	}
	
}
