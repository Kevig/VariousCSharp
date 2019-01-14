using UnityEngine;
using System.Collections;

public class ThreatZoneControl : MonoBehaviour {

	private Transform player;	//Reference to player Object this object has been created for
	private string unitName;	// This Units name;
	public string[] detectedUnits; //List of all units currently in threatzone
	public int numberOfDetectedUnits = 0;
	public int	unitThreatLevel; //Threat level value of this unit

	public delegate void ThreatBroadcast(string unitName, int threatLevel, string threatName);
	public static event ThreatBroadcast OnThreatBroadcast;

	// Use this for initialization
	void Start () {

		// Set size of detectedUnits
		detectedUnits = new string[50];

		// On start renames object to playerunit name + Zone type
		// Sets player ref to required unit name
		this.player = GameObject.Find (this.gameObject.name).transform;
		this.unitName = this.player.gameObject.name;
		this.gameObject.name = this.gameObject.name + "ThreatZone";

		// Starting threat level of this zone
		this.unitThreatLevel = 0;

		// Loop to 'null' out all positions of detectedUnits array
		for (int i = 0; i < detectedUnits.Length; i++) {
			detectedUnits [i] = null;
		}

	}

	void DetermineUnitThreatLevel(){



	}

	// Method handles notifying units within threatzone this units threat lvl if
	// threat level and units in threat zone are above 0
	void BroadcastThreatLevel(){

		// Checks through detectedUnits array and broadcasts the unitName and this units
		// threat level
		for (int i = 0; i < numberOfDetectedUnits; i++) {
			if (detectedUnits[i] != null){
			OnThreatBroadcast(this.detectedUnits[i], this.unitThreatLevel, this.unitName);
			}
		}

	}

	void FixedUpdate(){

		// Uses player ref to keep position updated
		this.transform.position = this.player.position;
	
	}

	// Method to handle trigger enter events
	void OnTriggerEnter(Collider other){

		// Determines what objects are in trigger area via tag checking
		// If object tag is not a player unit or untagged (ie. world building blocks)
		// Detected units value is increased and Objects name is added to detected units array
		if (other.gameObject.tag != "Untagged" && other.gameObject.tag != this.player.gameObject.tag) {
			detectedUnits[numberOfDetectedUnits] = other.gameObject.name;
			numberOfDetectedUnits++;
		}
	}
	// Method to handle trigger exit events
	void OnTriggerExit(Collider other){

		// Determines what object has left trigger area by loop checking detectedUnits array
		// for a name that matches the unit leaving then removes if from the array
		// if the unit exists, subtracts 1 from numberOfDetectedUnits value
		for (int i = 0; i < numberOfDetectedUnits; i++) {
			if (detectedUnits[i] == other.gameObject.name){
				detectedUnits[i] = null;
				numberOfDetectedUnits--;
			}
		}

	}

	// Update is called once per frame
	void Update () {

		if (numberOfDetectedUnits != 0 && unitThreatLevel != 0) {
			BroadcastThreatLevel();
		}

	}
}
