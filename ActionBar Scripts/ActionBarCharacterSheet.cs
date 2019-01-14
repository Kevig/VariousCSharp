using UnityEngine;
using System.Collections;

public class ActionBarCharacterSheet : MonoBehaviour {

	public Rect mainWindow;
	private float mainWindowWidth;
	private float mainWindowHeight;

	public bool characterSheetOptions = false;

	public float playerMovementSpeed;
	public float playerJumpHeight;

	// Use this for initialization
	void Start () {
	
		ActionBarActions.OnBarAction += this.CheckWindowState;

		UpdateWindowSizes ();

	}

	// Make a class that just calcs these values and pull from that class, since all main windows will be of same size
	void UpdateWindowSizes(){
		
		mainWindow = new Rect ((Screen.width / 8) * 3, (Screen.height / 8)*2, Screen.width / 4, Screen.height / 2);
		mainWindowWidth = (Screen.width / 4);
		mainWindowHeight = (Screen.height / 2);
		
	}

	// Setup of GUI elements
	void OnGUI (){
		
		if (characterSheetOptions) {
			mainWindow = GUI.Window (-4, mainWindow, mainWindowDisplay, "Character Sheet");
		}
	}
	
	void mainWindowDisplay(int windowID){
		
		
		
	}

	void CheckWindowState(string actionCode){

		if (actionCode == "GM1") {
			
			characterSheetOptions = !characterSheetOptions;
			
		} else if (actionCode == "GM2" || actionCode == "GM3" || actionCode == "GM4") {
			
			characterSheetOptions = false;
			
		}

	}

	// Update is called once per frame
	void Update () {
	
	}
}
