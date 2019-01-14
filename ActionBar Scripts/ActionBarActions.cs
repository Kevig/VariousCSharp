using UnityEngine;
using System.Collections;

public class ActionBarActions : MonoBehaviour {

	public Rect mainWindow;
	private float mainWindowWidth;
	private float mainWindowHeight;

	public string toolbarID;
	public string[] buttonActions;

	public bool actionsListOptions = false;

	public delegate void actionbarAction (string actionCode);
	public static event actionbarAction OnBarAction; 

	// Use this for initialization
	void Start () {
	
		ActionBarControl.OnButtonPress += this.ReceiveInput;
		ActionBarActions.OnBarAction += this.CheckWindowState;

		GameObject getMaxButtons = GameObject.Find ("GUIMenuControls");
		ActionBarOptionsGUI actionBarOptionsGUI = getMaxButtons.GetComponent<ActionBarOptionsGUI> ();

		buttonActions = new string[actionBarOptionsGUI.maxButtonsPerBar];

		this.toolbarID = this.gameObject.name;

		if (this.toolbarID == "Menu Bar") {

			for (int i = 0; i < actionBarOptionsGUI.toolbarValues[0].z;i++){
				this.buttonActions[i] = "GM"+(i+1);
			}
		
		}

		if (this.toolbarID == "PlayerMovement") {

			this.buttonActions[0] = "Forward";
			this.buttonActions[1] = "Backward";
			this.buttonActions[2] = "StrafeLeft";
			this.buttonActions[3] = "StrafeRight";
			this.buttonActions[4] = "Jump";
		}

		UpdateWindowSizes ();

	}

	void ReceiveInput(string barID, int BtnID){

		if (this.toolbarID == barID) {
		
			OnBarAction(this.buttonActions[BtnID]);
		}

	}


	// Make a class that just calcs these values and pull from that class, since all main windows will be of same size
	void UpdateWindowSizes(){

		mainWindow = new Rect ((Screen.width / 8) * 3, (Screen.height / 8)*2, Screen.width / 4, Screen.height / 2);
		mainWindowWidth = (Screen.width / 4);
		mainWindowHeight = (Screen.height / 2);

	}

	// Setup of GUI elements
	void OnGUI (){
		
		if (actionsListOptions) {
			mainWindow = GUI.Window (-2, mainWindow, mainWindowDisplay, "Actions List Options");
		}
	}

	void mainWindowDisplay(int windowID){

		

	}

	// Called upon on a button press, checks if window should be open or not and acts accordingly
	void CheckWindowState(string actionCode){

		if (actionCode == "GM2") {
			
			actionsListOptions = !actionsListOptions;
			
		} else if (actionCode == "GM1" || actionCode == "GM3" || actionCode == "GM4") {
			
			actionsListOptions = false;
			
		}

	}

	// Update is called once per frame
	void Update () {
	
	}
}
