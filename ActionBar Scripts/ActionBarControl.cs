using UnityEngine;
using System.Collections;

public class ActionBarControl : MonoBehaviour 
{

public GUIStyle screenText;	// Style setup component

public string toolbarID;		// Reference name for this Object

public Vector3 toolbarValues;	// Pulls this toolbars values and stores them here
public float barPadding;		

public Rect actionbarWindow;	// Pre-setup of window size values
public float barAnchorX;		// Bar anchors for x / y screen coords
public float barAnchorY;

private int barID;			// Number value of barID
private bool unlockBar;		// Unlock bar anchor control

private float barHeight;		// Bar dimensions
private float barWidth;

public string[] keyBinding; 	// Array for storing the keybind for a button
	
public bool setKeyBindings; 	// Control accepting next btn click as btn to bind
public bool waitForKey; 		// Control assigning next key press to asgn button

public string inputKey;		// String for key input recording
private int buttonNumber;       	// Reference to the button number on "this" bar

private int maxButtons;			//Max buttons on a bar ref

private bool displayBoundText = false;		// Display control for onscreen text
private bool displayUnboundText = false;	// 
private bool displayLockedKeyText = false;
private float textTimer;	// Timer ref for how long text will be displayed for
private float textDuration = 0.75f;	//
private string keybindRef;			// keybindRef for comparing input to keybinds

private bool updatingMovementBinds = false;

// Events for class communication

public delegate void keybindChange(string tBarID, string checkString, int btnRef);
public static event keybindChange OnKeybindChange;	// Broadcast a keybind change

public delegate void actionButtonPress(string tBarID, int btnRef);
public static event actionButtonPress OnButtonPress;	// Broadcast when a button is pressed or activated via a binding

	// Use this for initialization
	void Start () {

		ActionBarOptionsGUI.OnValuesChange += this.UpdateBarValues;
		ActionBarOptionsGUI.OnKeybindMode += this.KeybindMode;
		ActionBarControl.OnKeybindChange += this.DuplicateBindCatcher;

		inputKey = "";

		toolbarID = this.gameObject.name;

		// Determine which memory location of each array holds the information needed for this bar
		
		if (this.toolbarID != "Menu Bar" && this.toolbarID != "PlayerMovement") {
			barID = System.Convert.ToInt16 (this.toolbarID.Substring (9));
		} else if (this.toolbarID == "PlayerMovement"){
				
			barID = 1;
		}else{
				
			barID = 0;
		}
		this.screenText = new GUIStyle ();
		this.screenText.normal.textColor = Color.white;
		this.screenText.fontSize = 25;
		this.screenText.alignment = TextAnchor.MiddleCenter;

		// Find and Get values needed from action bar options class
		GameObject getValues = GameObject.Find ("GUIMenuControls");
		ActionBarOptionsGUI actionBarOptionsGUI = getValues.GetComponent<ActionBarOptionsGUI> ();

		// Assign array sizes for bindings / ability storage
		maxButtons = System.Convert.ToInt16 (actionBarOptionsGUI.maxButtonsPerBar);
		
		this.keyBinding = new string[maxButtons];
		//this.buttonAction = new string[maxButtons];

		if (this.toolbarID == "Menu Bar") {

			for (int i = 0; i < actionBarOptionsGUI.toolbarValues[0].z; i++){
				this.keyBinding[i] = "F"+(i+1);
			}

		}

		if (this.toolbarID == "PlayerMovement") {

			this.keyBinding[0] = "w";
			this.keyBinding[1] = "s";
			this.keyBinding[2] = "a";
			this.keyBinding[3] = "d";
			this.keyBinding[4] = "Space";
		
		}

		UpdateBarValues ();

	}

	void UpdateMovementBinds(string tbarID, int btnNumber){

		setKeyBindings = true;

		if (this.toolbarID == tbarID) {
			this.buttonNumber = btnNumber;
			waitForKey = true;
			updatingMovementBinds = true;
		}


	}

	void KeybindMode(bool keybindMode){

		if (waitForKey) {
			waitForKey = keybindMode;
		}
	}

	// Update action bar settings from action bar options
	void UpdateBarValues(){

		// Find and Get values needed from action bar options class
		GameObject getValues = GameObject.Find ("GUIMenuControls");
		ActionBarOptionsGUI actionBarOptionsGUI = getValues.GetComponent<ActionBarOptionsGUI> ();

		this.toolbarValues = actionBarOptionsGUI.toolbarValues [barID];
		this.barPadding = actionBarOptionsGUI.barPadding [barID];

		// Calc width and height of bar based on button size and bar padding
		this.barWidth = (((this.toolbarValues.x + this.barPadding) * this.toolbarValues.z) + barPadding) + 10;
		this.barHeight = this.toolbarValues.y + (this.barPadding * 2);

		// If bar Anchor as already been assigned a location do not reassign
		if (this.barAnchorX == 0) {
			this.barAnchorX = (Screen.width / 2) - this.barWidth / 2;
			if (this.barID == 0) {
				this.barAnchorY = Screen.height - (this.barHeight * (this.barID+1));
			} else {
				this.barAnchorY = Screen.height - (this.barHeight * this.barID);
			}
		}
		// Setup action bar window for this action bar
		this.actionbarWindow = new Rect (this.barAnchorX, this.barAnchorY, this.barWidth, this.barHeight);

		// Determine if bindings are changable
		this.setKeyBindings = actionBarOptionsGUI.setKeyBindings;

		for (int i = 0; i < maxButtons; i++) {

			if (i > this.toolbarValues.z - 1){
				this.keyBinding[i] = "";
			}

		}

	}

	//
	void OnGUI(){

		if (this.toolbarID != "PlayerMovement") {
			this.actionbarWindow = GUI.Window (this.barID, this.actionbarWindow, ActionBarDisplay, this.toolbarID);
		}
		// Display text to show user there in keybind mode
		if (setKeyBindings) {
			GUI.Label (new Rect (0, 10, Screen.width, 50), "Key Bind Mode", screenText);	
		}
		// Display text to ask user to press a key for binding
		if (waitForKey) {	
			GUI.Label (new Rect	(0, 40, Screen.width,50),"Press a key to bind", screenText);
			displayBoundText = false;
		}
		// Display text to inform user there keybind was successful
		if (displayBoundText) {
			GUI.Label (new Rect	(0, 70, Screen.width,50),"Key Bound", screenText);	
		}
		// Display text to inform user there keybind was removed
		if (displayUnboundText) {
			GUI.Label (new Rect	(0, 70, Screen.width,50),"Key Unbound", screenText);	
		}
		// Display text to inform user that this key cannot be used as a keybind - ie. is locked to a movement key
		if (displayLockedKeyText) {
			GUI.Label (new Rect (0, 100, Screen.width, 50), "Key Cannot be used as Keybind", screenText);	
		}
	}

	//
	void ActionBarDisplay(int windowID){

	if (this.toolbarID != "PlayerMovement") {
		// Small left alligned button for allowing unlocking of action bar position
		if (GUI.Button (new Rect (1, (this.barHeight / 2) - 5, 10, 10), "")) {
		
			this.unlockBar = true;
		} 
	
	// Loop for creating button on action bar
	for (int i = 0; i < this.toolbarValues.z; i++) {
		
	if (GUI.Button (new Rect ((10 + this.barPadding) + ((this.barPadding * i) + (this.toolbarValues.x * i)), this.barPadding, this.toolbarValues.x, this.toolbarValues.y), keyBinding [i])) {

		if (setKeyBindings && this.toolbarID != "Menu Bar" && this.toolbarID != "PlayerMovement") {

			waitForKey = true;
			this.buttonNumber = i;
		} else {

			OnButtonPress (this.toolbarID, i);
		}
		}
	}
	}
}

	void Update(){

		// Checks for text timer, so that text control variable can be reverted after set time has passed
		if (Time.time > textTimer) {
			displayBoundText = false;
			displayUnboundText = false;
			displayLockedKeyText = false;
		}

		// Upon left click will bar moving is true, unlock bar will be false and bar will stay at current position
		if (Input.GetButton ("LeftClick") && this.unlockBar) {
			this.unlockBar = false;
		}
		
		// If UnlockBar is true bar anchors will track to mouse x / y location
		if (this.unlockBar){
			this.barAnchorX = Input.mousePosition.x;
			this.barAnchorY = Screen.height - Input.mousePosition.y;
			UpdateBarValues();
		}
		
		// While in keybind mode and target button selected, next keypress will log to keybind array. After bind set, reset control variables
		if (waitForKey) {

			inputKey = Input.inputString;
			
			if (Input.GetKeyDown(KeyCode.Escape)){
				
				waitForKey = false;
				this.keyBinding[buttonNumber] = "";
				displayUnboundText = true;
				textTimer = Time.time + textDuration;

			}

			if (Input.GetButton("Forward") || Input.GetButton("Backward") || Input.GetButton("StrafeLeft") || Input.GetButton("StrafeRight") || Input.GetButton("Jump")){
				displayLockedKeyText = true;
				textTimer = Time.time + textDuration;
			}

			if (inputKey != "" && !Input.GetButton("Forward") && !Input.GetButton("Backward") && !Input.GetButton("StrafeLeft") && !Input.GetButton("StrafeRight") && !Input.GetButton("Jump")){
				
				waitForKey = false;
				
				// Catches double keypresses being mapped to a button and stores only the first keypress for the binding
				if (inputKey.Length > 1){
					inputKey = inputKey.Substring(0);
				}
				
				this.keyBinding[buttonNumber] = inputKey;
				inputKey = "";
				displayBoundText = true;
				textTimer = Time.time + textDuration;
				OnKeybindChange(this.toolbarID, this.keyBinding[buttonNumber], this.buttonNumber);

			}

		}

		CheckKeyInput ();

	}


	// Key capture method
	void CheckKeyInput(){


		// while not in set keybindings mode catch key presses
		if (!setKeyBindings) {
		
			string checkKey = Input.inputString;

			if (checkKey.Length > 1){
				checkKey = checkKey.Substring(0);
			}

			// If this toolbar is not the menu bar, take the exact key match and pass to OnButtonPress event to check for action
			if (this.toolbarID != "Menu Bar" && this.toolbarID != "PlayerMovement"){
			
				for (int i = 0; i < this.toolbarValues.z; i++){

					if (checkKey == this.keyBinding[i]){
						OnButtonPress(this.toolbarID, i);
					}

				}
			
			} else { // If this is the menu bar, then catch special key presses via keycodes and match string ref, to send to OnbuttonPress

				// Trashy method of catching keys, will look into changing bind system to wholey work with keycodes
				if (Input.GetKeyDown(KeyCode.F1)){ keybindRef = "F1";}
				if (Input.GetKeyDown(KeyCode.F2)){ keybindRef = "F2";}
				if (Input.GetKeyDown(KeyCode.F3)){ keybindRef = "F3";}
				if (Input.GetKeyDown(KeyCode.F4)){ keybindRef = "F4";}
				if (Input.GetKeyDown(KeyCode.F5)){ keybindRef = "F5";}

				for (int i = 0; i < this.toolbarValues.z; i++){

					if (keybindRef == this.keyBinding[i]){
						OnButtonPress(this.toolbarID,i);
						keybindRef = ""; // Reset keybindRef string, so that event is constantly repeated.
					}
				}
			}
		}
	}

	// Called after any keybind change, checks all class instances for a matching keybind and removes the old location in favour of the new
	void DuplicateBindCatcher(string tBarID, string checkString, int btnRef){

		for (int i = 0; i < this.toolbarValues.z; i++) {

			if (this.keyBinding[i] == checkString){

				if (tBarID == this.toolbarID && i != btnRef){
					this.keyBinding[i] = "";
				} else if (tBarID != this.toolbarID){
					this.keyBinding[i] = "";
				}
			}
		}
	}
}

