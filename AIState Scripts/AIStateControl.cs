// "AI State Control" system
// Date: 17/03/2014
// Scripted by: Kevin Groundwater
// Description: Decides state of this unit, controls activation of behaviour scripts based on
// other factors
using UnityEngine;
using System.Collections;

public class AIStateControl : MonoBehaviour {

	public int movementState;

	private string unitName;
	private string[] movementStateCheck = new string[2];
	private bool stateActivate;

	// Initialization
	void Start () {

		ThreatZoneControl.OnThreatBroadcast += this.ChangeMovementState;
		AI_MovementControl.OnThreatBroadcast += this.ChangeMovementState;

		this.unitName = this.gameObject.name;

		movementStateCheck [0] = "AIState_Roam";
		movementStateCheck [1] = "AIState_Scared";

		this.movementState = 0;

		CheckMovementState (this.movementState, this.unitName);

	}

	void ChangeMovementState(string unitName, int threatLevel, string threatName){

		if (unitName == this.unitName) {
			this.movementState = threatLevel;
			CheckMovementState(this.movementState, this.unitName);
		}

	}
	// Check for current movement state
	void CheckMovementState(int movementState, string unitName){


		if (unitName == this.unitName){

			for (int i = 0; i < movementStateCheck.Length; i++) {

				if (i == this.movementState){
					this.stateActivate = true;
				}else{
					this.stateActivate = false;
				}
				GameObject stateText = transform.FindChild(movementStateCheck[i]).gameObject;
				stateText.SetActive(stateActivate);

			}

		}

	}

	// Update is called once per frame
	void Update () {
	
		

	}
}
