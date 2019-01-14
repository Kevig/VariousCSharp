// "UnitHighlight" - part 3 of 3 of Unit / Target Selection
// Date: 16/03/2014
// Scripted by: Kevin Groundwater
// Description: Final class of the selection process, receives names of objects to have there
// child objects activate / deactivated for highlighting purposes
using UnityEngine;
using System.Collections;

public class UnitHightlight : MonoBehaviour {
	
	private string unitID; // ID of this unit

	void Start(){

		// Listener to receive highlighting events from UnitSelection class 
		UnitSelectionHandler.OnHighlight += this.HighlightUnit;

		// Gets name of unit from attached gameobject name
		this.unitID = this.gameObject.name;

	}
	
	// Handles highlighting method for this unit
	// Actions preformed on a unit highlight
	// Enable / Disable graphical highlight options
	// Enable / Disable floating objects such as Health Bar
	void HighlightUnit(bool isHighlighted, string unitName){

		if (unitName == this.unitID) {

			// Find child gameobjects and temp store for enable / disable actions
			GameObject highlight = transform.FindChild ("Highlight").gameObject;
		
			highlight.SetActive (isHighlighted);
		}
	}
}
