// "UnitSelectionHandler" - part 2 of 3 of Unit / Target Selection
// Date: 16/03/2014
// Scripted by: Kevin Groundwater
// Description: Control class of unit / target selection process, attached to all player / target
// objects. Receives player interaction, Gets data from Selection Manager and allows communication
// between Selection Manager and UnitHighlight Classes
using UnityEngine;
using System.Collections;

public class UnitSelectionHandler : MonoBehaviour {
	
	public string unitName; //Name of unit
	public string unitTag; //Units Tag

	public bool selected; // Selected control

	private bool isHighlighted; // Control for deciding if a state change should be called 
	private bool highlighted;	// Highlighted control
	private bool stateChange;	// Control for if UnitHighlight class should be called
	
	// Broadcast event for selection changes - calls SelectionManager
	public delegate void SelectionHandler(string unitName, string unitTag);
	public static event SelectionHandler OnSelectionChange;

	// Broadcast event for highlight changes - calls UnitHighlight
	public delegate void HighlightHandler(bool isHighlighted, string unitName);
	public static event HighlightHandler OnHighlight;

	// Use this for initialization
	void Start () {

		// Listener for broadcasts from SelectionManager
		SelectionManager.OnBroadcast += this.TargetHandler;

		this.unitName = this.gameObject.name; // Gets this Objects name
		this.unitTag = this.gameObject.tag;	// Gets this Objects tag
	
	}
	
	// Update is called once per frame
	void Update () {

		// If statechange is true HighControl can be called
		if (stateChange) {
			HighlightControl ();
			stateChange = false;
		}
	}

	// On mouseover object, highlight is called to be activated
	void OnMouseEnter(){

		this.highlighted = true;
		stateChange = true;
	}

	// On mouse exit, highlight is called to be deactivated
	void OnMouseExit(){
	
		this.highlighted = false;
		stateChange = true;
	}
	
	// On left click on object, selection change message sent to selection manager class
	// with name of unit clicked on and its tag
	void OnMouseDown(){

		OnSelectionChange (this.unitName, this.unitTag);

	}

	// Hihglight control determines if object needs to be highlighted or de-highlighted
	// Broadcasts OnHighlight event to UnitHighlight
	void HighlightControl(){

		if (this.highlighted || this.selected) {
			this.isHighlighted = true;
			} else {
			this.isHighlighted = false;
			}
	
		OnHighlight (this.isHighlighted, this.unitName);
	
	}

	// TargetHandler determines the process of selection
	// Is called upon via broadcast of event from Selection manager
	void TargetHandler(string playerName, string TargetName){
		
		if (playerName == this.unitName) {
			this.selected = true;
		} else if (TargetName == this.unitName) {
			this.selected = true;
		}else{
			this.selected = false;
		}

		stateChange = true;
	}
	
}
