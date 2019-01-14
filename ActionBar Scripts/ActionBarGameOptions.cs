using UnityEngine;
using System.Collections;

public class ActionBarGameOptions : MonoBehaviour {

	public Rect mainWindow;
	private float mainWindowWidth;
	private float mainWindowHeight;
	private float mainWindowPadding = 5.0f;
	private float mainWindowHeader = 15.0f; 

	private float buttonWidth;
	private float buttonHeight;
	private int numberOfButtons = 5;

	public string[] buttonRef;
	public string[] actionCode;

	public bool gameOptions = false;

	public delegate void actionbarAction (string actionCode);
	public static event actionbarAction OnBarAction;
	
	// Use this for initialization
	void Start () {
		
		ActionBarActions.OnBarAction += this.CheckWindowState;
		ActionBarGameOptions.OnBarAction += this.CheckWindowState;

		buttonRef = new string[numberOfButtons];
		actionCode = new string[numberOfButtons];

		buttonRef [0] = "Game Options";
		buttonRef [1] = "Character Controls";
		buttonRef [2] = "...";
		buttonRef [3] = "Exit to Main Menu";
		buttonRef [4] = "Exit Game";

		for (int i = 0; i < numberOfButtons; i++) {
			actionCode[i] = "GM"+(i+6);
		}

		UpdateWindowSizes ();
		
	}
	
	// Make a class that just calcs these values and pull from that class, since all main windows will be of same size
	void UpdateWindowSizes(){

		mainWindowWidth = (Screen.width / 16) * 2;
		mainWindowHeight = (Screen.height / 8) * 2;

		mainWindow = new Rect ((Screen.width / 16) * 7, (Screen.height / 8)*3, mainWindowWidth, mainWindowHeight);

		buttonWidth = mainWindowWidth - (mainWindowPadding * 4);
		buttonHeight = (mainWindowHeight - ((mainWindowHeader * 2)+ (mainWindowPadding * numberOfButtons))) / numberOfButtons;

	}
	
	// Setup of GUI elements
	void OnGUI (){
		
		if (gameOptions) {
			mainWindow = GUI.Window (-3, mainWindow, mainWindowDisplay, "Game Options");
		}
	}
	
	void mainWindowDisplay(int windowID){

		// Small right alligned button for closing Game Menu
		if (GUI.Button (new Rect (mainWindowWidth - 15, (mainWindowHeader / 2) - 5, 10, 10), "")) {
			
			gameOptions = false;
		} 
		// Draw buttons to window
		for (int i = 0; i < numberOfButtons; i++) {

			if (GUI.Button (new Rect(mainWindowPadding * 2, (mainWindowHeader + mainWindowPadding) + (i * (buttonHeight + mainWindowPadding)), buttonWidth, buttonHeight), buttonRef[i])){
				OnBarAction(actionCode[i]);
				this.gameOptions = false;
			}

		}
		
	}
	// Check state of window and act accordingly
	void CheckWindowState(string actionCode){
		
		if (actionCode == "GM4") {
			
			gameOptions = !gameOptions;
			
		} else if (actionCode == "GM1" || actionCode == "GM2" || actionCode == "GM3"||actionCode == "GM6"||actionCode == "GM7"||actionCode == "GM8"||actionCode == "GM9"||actionCode == "GM10") {
			
			gameOptions = false;
			
		}
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
