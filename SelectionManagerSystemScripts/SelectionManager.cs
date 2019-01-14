// "Selection Manager" - part 1 of 3 of Unit / Target selection
// Date: 16/03/2014
// Scripted by: Kevin Groundwater
// Description: Main Hub of selection system - Broadcasts / controls selection changes
// Stores player units names / targets and currently selected player unit

using UnityEngine;
using System.Collections;

public class SelectionManager : MonoBehaviour 
{
	
public string[] playerUnitSelected; // Playerunit reference, size changable
public string[] otherUnitSelected; // Array for storing playerunit targets
public string playerUnitCurrentlySelected; // Current selected player unit reference
	
private int playerUnitNumber; // Reference location of a variable within an array
	
// Broadcast event for passing name of current unit and unit's target if exists
// to the UnitSelectionHandler class
public delegate void PlayerTargetHandler(string playerName, string targetName);
public static event PlayerTargetHandler OnBroadcast;

// Broadcast event for renaming threat zones relative to amount of player units
public delegate void TriggerZonesSetup(string playerName);
public static event TriggerZonesSetup SetTriggerZones;

// Initialization
void Start () 
{

// Listener for events of selection changes passed from UnitSelectionHandler class
UnitSelectionHandler.OnSelectionChange += this.CheckTag;
playerUnitCurrentlySelected = null;

// Initialization loop for 'null'ing' all array locations
// Locations set to null for purpose of logic statements
for (int i = 0; i < playerUnitSelected.Length; i++) 
{
	playerUnitSelected[i] = "PlayerUnit"+(i+1);
	otherUnitSelected[i] = null;
	SetTriggerZones(playerUnitSelected[i]);
}
}

// Called per frame
void Update()
{

// Listens for input event 'Deselect' and calls the deselect method upon this event
if (Input.GetButtonDown ("Deselect")) {
	DeselectMethod ();
}
}

// Called upon a selection change from UnitSelectionHandler class
void CheckTag(string unitName, string unitTag)
{
// Sets currently selected player to the unitName passed from UnitSelection
// If unit passed in is a player unit then the playerunit number is 
// determined and stored, playerunit / ref location is set to unitName
// iF unit passed in is not a player unit and a playerunit is selected
// the target / ref location is set to the unitName
// Broadcast method is then called



if (unitTag == "PlayerUnit")
{

	playerUnitCurrentlySelected = unitName;

if (unitName == "PlayerUnit1")
{
        playerUnitNumber = 0;
}else if (unitName == "PlayerUnit2")
{
	playerUnitNumber = 1;
}else if (unitName == "PlayerUnit3")
{
	playerUnitNumber = 2;
}else if (unitName == "PlayerUnit4")
{
	playerUnitNumber = 3;
}
        playerUnitSelected [playerUnitNumber] = unitName;
}else
{
	if (playerUnitCurrentlySelected != null)
        {
		otherUnitSelected [playerUnitNumber] = unitName;
	}
}

BroadcastSelected ();
}
	
// Broadcast method sends playerUnitCurrentlySelected and unit's target to 
// the UnitSelection handler class for highlighting
void BroadcastSelected()
{
	OnBroadcast(playerUnitCurrentlySelected, otherUnitSelected[playerUnitNumber]);
}


// Deselect method handles the order in which the deselection process is handled
// If a player unit is selected then a loop is run to determine the array location
// of the selected unit, ref location is then used to determine if the player unit
// currently has a target.
// If the player unit has a target on 1 event the target is deselected
// On the next event or if player has no target, the player is deselected
// Broadcast is then called to update UnitSelection

void DeselectMethod()
{

	if (playerUnitCurrentlySelected != null) 
        {
		int i = 0;
	while (playerUnitCurrentlySelected != playerUnitSelected[i]) {
		i++;
	}

	if (otherUnitSelected[i] == null){
		playerUnitCurrentlySelected = null;
	}else if (otherUnitSelected[i] != null){
		otherUnitSelected[i] = null;
	}
		BroadcastSelected();
		
	}
	}
}
