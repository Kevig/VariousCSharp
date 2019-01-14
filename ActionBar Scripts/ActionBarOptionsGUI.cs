using UnityEngine;
using System.Collections;

public class ActionBarOptionsGUI : MonoBehaviour {

	public Rect mainWindow;
	public GUIStyle windowStyle;
	public bool displayActionBarOptions = false;

	// Slider min / max values
	private int windowPadding = 20;
	private int maxToolbars = 11;
	public int maxButtonsPerBar = 12;
	private float minButtonsPerBar = 1;
	private float minButtonDimensions = 20;
	private float maxButtonDimensions = 100;
	private float minBarPadding = 0;
	private float maxBarPadding = 25;
	
	// Toolbar changeable options variables
	public string[] toolbarID;
	public Vector3[] toolbarValues; // (width / height / number of buttons)
	public float[] barPadding; // padding value for each bar
	public bool[] barState; // On or Off value for bar

	private float[] toolbarState; // float value for slider switch

	// Control for allowing keybindings to be changed on toolbars
	public bool setKeyBindings = false;

	// Option menu size variables
	private float mainWindowWidth;
	private float mainWindowHeight;

	// toolbarRef tracks which action bars information needs to be displayed in window
	public int toolbarRef;
	
	// Event to broadcast UI settings change
	public delegate void valuesChange ();
	public static event valuesChange OnValuesChange;

	// event to notfiy Actionbar control that keybind mode has been turned off, to catch exception of if system is waiting for keypress whenmode exited
	public delegate void keybindModeExit (bool keyBindMode);
	public static event keybindModeExit OnKeybindMode;

	//Pre-Initialisation function
	void Awake () {

		ActionBarActions.OnBarAction += this.CheckWindowState;

		// Set array sizes
		toolbarID = new string[maxToolbars];
		toolbarValues = new Vector3[maxToolbars];
		toolbarState = new float[maxToolbars];
		barState = new bool[maxToolbars];
		barPadding = new float[maxToolbars];

		// Set default toolbar data to be shown as game menu
		toolbarRef = 0;

		DefaultValues ();
		UpdateWindowSizes ();
		toolBarObjectControl ();
	}

	// Upon game start set all toolbar values to default settings - add file to save values to to keep settings between plays
	void DefaultValues(){

		// set default slider values / toolbar ID's
		toolbarID[0] = "Menu Bar";
		// Set name of hidden bar for holding player movement controls
		toolbarID[1] = "PlayerMovement";
		// Set names of bars
		for (int i = 2; i < maxToolbars; i++) {
			
			toolbarID[i] = "ActionBar" + i;
		}
		// Set default button sizes and buttons per bar - 0 is options bar locked to 5 buttons, 1 and 2 are first 2 action bars
		toolbarValues [0] = new Vector3 (minButtonDimensions, minButtonDimensions, 5);
		toolbarValues [1] = new Vector3 (minButtonDimensions, minButtonDimensions, 5);
		toolbarValues [2] = new Vector3 (minButtonDimensions, minButtonDimensions, maxButtonsPerBar);
		toolbarValues [3] = new Vector3 (minButtonDimensions, minButtonDimensions, maxButtonsPerBar);

		for (int i = 4; i < maxToolbars; i++) {
			toolbarValues [i] = new Vector3 (minButtonDimensions, minButtonDimensions, minButtonsPerBar);
		}

		// Turn on first 4 bars - ie. game menu, movementbar and 2 action bars
		toolbarState [0] = 1;
		toolbarState [1] = 1;
		toolbarState [2] = 1;
		toolbarState [3] = 1;

		// Loop remaining locations of toolbarstate array and set states to 0 for float array of state
		for (int i = 4; i < maxToolbars; i++) {
			toolbarState[i] = 0;
		}

		// Loop float array table for toolbar states and set bool version to appropreate values
		for (int i = 0; i < maxToolbars; i++) {

			if (toolbarState[i] == 0){
				barState[i] = false;
			}else{
				barState[i] = true;
			}
		}

		// Set all bars to use default paddding of 1

		for (int i = 0; i < maxToolbars; i++) {
			barPadding[i] = 1;
		}

	}
	// Called upon any screen size changes to re-calc component values
	void UpdateWindowSizes(){

		mainWindow = new Rect ((Screen.width / 8) * 3, (Screen.height / 8)*2, Screen.width / 4, Screen.height / 2);
		mainWindowWidth = (Screen.width / 4);
		mainWindowHeight = (Screen.height / 2);

	}

	// Called upon action bar event
	void CheckWindowState(string actionCode){

		if (actionCode == "GM3") {

			displayActionBarOptions = !displayActionBarOptions;
		
		} else if (actionCode == "GM1" || actionCode == "GM2" || actionCode == "GM4") {

			displayActionBarOptions = false;

		}

	}

	// Setup of GUI elements
	void OnGUI (){

		if (displayActionBarOptions) {
			mainWindow = GUI.Window (-1, mainWindow, mainWindowDisplay, "Action Bar Options");
		}
	}
	
	// Main window display compnents
	void mainWindowDisplay (int windowID){

		// Allignment values for components inside window
		float verticalSpacing = ((mainWindowHeight - windowPadding) / (maxToolbars*2))+2;
		float horizontalPosition = windowPadding / 2;
		float horizontalAllignment = mainWindowWidth / 2;
		float verticalAllignment = windowPadding + (windowPadding / 2);
		float componentVertical = mainWindowHeight / (maxToolbars * 2);

		// Create a close menu button
		if (GUI.Button (new Rect (mainWindowWidth - 20, 5, 10, 10), "")) {

			displayActionBarOptions = false;
			if (setKeyBindings){
				setKeyBindings = false;
				OnKeybindMode(setKeyBindings);
			}
		}

		// Create list of buttons to tab between each toolbar settings
		for (int i = 0; i < maxToolbars; i++) {

			if (GUI.Button (new Rect (horizontalPosition, (windowPadding + (windowPadding/2)) + (verticalSpacing * i), mainWindowWidth / 3, mainWindowHeight / (maxToolbars*2)), toolbarID [i])) {

				toolbarRef = i;
			}
		}

		// Setup general layout for options that can be changed
		GUI.Label (new Rect (mainWindowWidth / 2, windowPadding + (windowPadding/2), mainWindowWidth / 3, mainWindowHeight / (maxToolbars * 2)), toolbarID[toolbarRef]);

		if (toolbarRef > 1) {
			// Slider bar for turning each bar on or off
			toolbarState [toolbarRef] = Mathf.RoundToInt (GUI.HorizontalSlider (new Rect ((mainWindowWidth / 2) + (windowPadding + (windowPadding / 2)), (windowPadding + (windowPadding / 2)) + (verticalSpacing * 3), mainWindowWidth / 6, mainWindowHeight / (maxToolbars * 2)), toolbarState [toolbarRef], 0.0f, 1.0f));
		}
		// Display state 'Fixer' takes float value used on slider and sets string text and bar visability bool accordingly
		string displayState;

		if (toolbarState[toolbarRef] == 0) {
			barState [toolbarRef] = false;
			displayState = "Off";
		} else {
			barState [toolbarRef] = true;
			displayState = "On";
		}

		// Temp string variables to store float to string conversions for displaying current slider values
		string displayNumOfButtons = System.Convert.ToString (toolbarValues [toolbarRef].z);
		string displayButtonWidth = System.Convert.ToString (toolbarValues [toolbarRef].x);
		string displayButtonHeight = System.Convert.ToString (toolbarValues [toolbarRef].y);
		string displayBarPadding = System.Convert.ToString (barPadding [toolbarRef]);

		if (toolbarRef != 1) {

			// Switch option for turning bar on / off
			GUI.Label (new Rect (horizontalAllignment, (verticalAllignment + (verticalSpacing * 2)) - 5, mainWindowWidth / 3, componentVertical), "Bar State :");
			GUI.Label (new Rect (horizontalAllignment + (mainWindowWidth / 3), (verticalAllignment + (verticalSpacing * 3)) - 5, mainWindowWidth / 6, mainWindowHeight / (maxToolbars * 2)), displayState);

			//Checks if bar is turned on, if bar is on additional options will be displayed
			if (barState [toolbarRef]) {

				// Checks if bar is not options menu bar. If bar is options menu bar, ability to turn off will be disabled.
				if (toolbarRef != 0) {
				// Slider bar for adjusting number of buttons
				GUI.Label (new Rect (horizontalAllignment, (verticalAllignment + (verticalSpacing * 4)) - 10, horizontalAllignment, componentVertical), "Number of Buttons : " + displayNumOfButtons);
				toolbarValues [toolbarRef].z = Mathf.RoundToInt (GUI.HorizontalSlider (new Rect (horizontalAllignment, (verticalAllignment + (verticalSpacing * 5)) - 13, (mainWindowWidth / 3) + windowPadding, mainWindowHeight / (maxToolbars * 2)), toolbarValues [toolbarRef].z, minButtonsPerBar, maxButtonsPerBar));
			}

			// Slider bar for adjusting button width
			GUI.Label (new Rect (horizontalAllignment, (verticalAllignment + (verticalSpacing * 6)) - 10, horizontalAllignment, componentVertical), "Button Width : " + displayButtonWidth);
			toolbarValues [toolbarRef].x = Mathf.RoundToInt (GUI.HorizontalSlider (new Rect (horizontalAllignment, (verticalAllignment + (verticalSpacing * 7)) - 13, mainWindowWidth / 3 + windowPadding, mainWindowHeight / (maxToolbars * 2)), toolbarValues [toolbarRef].x, minButtonDimensions, maxButtonDimensions));

			// Slider bar for adjusting button height
			GUI.Label (new Rect (horizontalAllignment, (verticalAllignment + (verticalSpacing * 8)) - 10, horizontalAllignment, componentVertical), "Button Height : " + displayButtonHeight);
			toolbarValues [toolbarRef].y = Mathf.RoundToInt (GUI.HorizontalSlider (new Rect (horizontalAllignment, (windowPadding + (windowPadding / 2)) + (verticalSpacing * 9) - 13, mainWindowWidth / 3 + windowPadding, mainWindowHeight / (maxToolbars * 2)), toolbarValues [toolbarRef].y, minButtonDimensions, maxButtonDimensions));

			// Slider bar for adjusting bar padding
			GUI.Label (new Rect (horizontalAllignment, (verticalAllignment + (verticalSpacing * 10)) - 10, horizontalAllignment, componentVertical), "Bar Padding : " + displayBarPadding);
			barPadding [toolbarRef] = Mathf.RoundToInt (GUI.HorizontalSlider (new Rect (horizontalAllignment, (windowPadding + (windowPadding / 2)) + (verticalSpacing * 11) - 13, mainWindowWidth / 3 + windowPadding, mainWindowHeight / (maxToolbars * 2)), barPadding [toolbarRef], minBarPadding, maxBarPadding));
		}
		
		} else{

			GameObject getMovementBinds = GameObject.Find("PlayerMovement");
			ActionBarControl actionBarControl = getMovementBinds.GetComponent<ActionBarControl>();

			GameObject getMovementActions = GameObject.Find("PlayerMovement");
			ActionBarActions actionBarActions = getMovementActions.GetComponent<ActionBarActions>();

			int k = 2;

			for (int j = 0; j < actionBarControl.toolbarValues.z; j++){

				GUI.Label (new Rect (horizontalAllignment, (verticalAllignment + (verticalSpacing * k)) - 10, horizontalAllignment, componentVertical), actionBarActions.buttonActions[j] + " : ");
				k+=2;
			}

			int buttonWidth;

			for (int i = 0; i < this.toolbarValues[toolbarRef].z; i++){

				if (actionBarControl.keyBinding[i].Length == 1){
					buttonWidth = 30;
				} else if (actionBarControl.keyBinding[i].Length > 1 && actionBarControl.keyBinding[i].Length < 6){
					buttonWidth = 60;
				} else if (actionBarControl.keyBinding[i].Length == 0){
					buttonWidth = 30;
				} else {
					buttonWidth = 100;
				}

				GUI.Button(new Rect (mainWindowWidth - (windowPadding + buttonWidth), ((verticalAllignment + (verticalSpacing * 2)) - 15) + ((45 * i)-(4*i)), buttonWidth, 30),actionBarControl.keyBinding[i]);
			}

		}
		// Button to turn on / off keybind mode
		string bindMode;

		if (!setKeyBindings) {
			bindMode = "Change Key Bindings";
		} else {
			bindMode = "Exit Key Bind Mode";
		}

		if (GUI.Button (new Rect (horizontalPosition, ((windowPadding + (windowPadding/2)) + (verticalSpacing * 18))-5, mainWindowWidth - windowPadding, (mainWindowHeight / (maxToolbars*2))+5), bindMode)) {
		
			setKeyBindings = !setKeyBindings;
			OnKeybindMode(setKeyBindings);
		}

		// When any change is made within options window toolbarObjectControl is called, and event is broadcast to actionbar class
		if (GUI.changed) {
			toolBarObjectControl();
			OnValuesChange ();
		}

	}

	// Checks Objects and creates / destroys bar's depending on options settings
	void toolBarObjectControl(){

		for (int i = 0; i < maxToolbars; i++) {

			GameObject checkObjectExists = GameObject.Find(toolbarID[i]);
			if (checkObjectExists == null && barState[i]){

					GameObject emptyBar = new GameObject();
					emptyBar.name = toolbarID[i];
					emptyBar.AddComponent<ActionBarControl>();
					emptyBar.AddComponent<ActionBarActions>();

			}else if (checkObjectExists != null && !barState[i]){

					Destroy(checkObjectExists);

			}
		}

	}


}
