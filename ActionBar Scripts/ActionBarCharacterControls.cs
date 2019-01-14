using UnityEngine;
using System.Collections;

public class ActionBarCharacterControls : MonoBehaviour {

	public Rect mainWindow;
	private float mainWindowWidth;
	private float mainWindowHeight;
	private float mainWindowPadding = 2.0f;
	private float mainWindowHeader = 15.0f; 

	public bool displayCharacterControls = false;


	// Use this for initialization
	void Start () {
	
		ActionBarGameOptions.OnBarAction += this.CheckWindowState;
		ActionBarActions.OnBarAction += this.CheckWindowState;
	}

	void CheckWindowState(string actionCode){

		if (actionCode == "GM7") {
			this.displayCharacterControls = true;
			UpdateWindowSizes ();
		} else {
			if (actionCode != "GM5"){
				this.displayCharacterControls = false;
			}
		}

	}

	// Make a class that just calcs these values and pull from that class, since all main windows will be of same size
	void UpdateWindowSizes(){
		
		mainWindowWidth = (Screen.width / 16) * 2;
		mainWindowHeight = (Screen.height / 8) * 2;
		
		mainWindow = new Rect ((Screen.width / 16) * 7, (Screen.height / 8)*3, mainWindowWidth, mainWindowHeight);

		
	}
	
	// Setup of GUI elements
	void OnGUI (){
		
		if (displayCharacterControls) {
			mainWindow = GUI.Window (-6, mainWindow, mainWindowDisplay, "Character Control Setup");
		}
	}
	
	void mainWindowDisplay(int windowID){
		
		// Small right alligned button for closing Game Menu
		if (GUI.Button (new Rect (mainWindowWidth - 15, (mainWindowHeader / 2) - 5, 10, 10), "")) {
			
			displayCharacterControls = false;
		} 

		
	}

	// Update is called once per frame
	void Update () {
	
	}
}
