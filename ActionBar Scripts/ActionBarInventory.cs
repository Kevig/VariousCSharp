using UnityEngine;
using System.Collections;

public class ActionBarInventory : MonoBehaviour {

	public Rect mainWindow;
	
	public bool playerInventory = false;

	private int inventorySlot;
	private int inventorySize;
	private int inventoryPadding;
	private int inventoryRows;
	private int inventoryHeader;
	private int inventoryWindowSizeX;
	private int inventoryWindowSizeY;

	private float inventoryAnchorX;
	private float inventoryAnchorY;

	public int[] inventorySlotKey;
	private int[] tempSlotStorage;

	private bool unlockInventory = false;

	// Use this for initialization
	void Start () {
		
		ActionBarActions.OnBarAction += this.CheckWindowState;

		inventorySlot = 30;
		inventorySize = 6;
		inventoryPadding = 2;
		inventoryRows = 2;
		inventoryHeader = 20;

		inventoryAnchorX = (Screen.width / 8) * 6;
		inventoryAnchorY = Screen.height / 2;

		inventorySlotKey = new int[inventorySize];

		UpdateWindowSizes ();
		
	}
	
	// Make a class that just calcs these values and pull from that class, since all main windows will be of same size
	void UpdateWindowSizes(){

		inventoryWindowSizeX = ((inventorySize / inventoryRows) * inventorySlot)+ (inventoryPadding * ((inventorySize / inventoryRows)+1))+ (inventoryPadding * 2);
		inventoryWindowSizeY = (inventoryHeader + ((inventorySlot * inventoryRows)+ (inventoryPadding * inventoryRows)))+ (inventoryPadding + inventoryHeader);

		mainWindow = new Rect (inventoryAnchorX, inventoryAnchorY, inventoryWindowSizeX, inventoryWindowSizeY);
	
		// Take current key values and store in temp slot key, so that can be copied back to inventoryslotkey after a resize of array
		tempSlotStorage = new int[inventorySize];
		tempSlotStorage = inventorySlotKey;
		inventorySlotKey = new int[inventorySize];
		inventorySlotKey = tempSlotStorage;

	}
	
	// Setup of GUI elements
	void OnGUI (){
		
		if (playerInventory) {
			mainWindow = GUI.Window (-5, mainWindow, mainWindowDisplay, "Inventory");
		}
	}
	
	void mainWindowDisplay(int windowID){

		// Small left alligned button for allowing Inventory positioning
		if (GUI.Button (new Rect (5, (inventoryHeader / 2) - 5, 10, 10), "")) {
			
			unlockInventory = true;
		} 

		// Small right alligned button for closing Inventory
		if (GUI.Button (new Rect (inventoryWindowSizeX - 15, (inventoryHeader / 2) - 5, 10, 10), "")) {
			
			playerInventory = false;
		} 

		for (int i = 0; i < inventoryRows; i++) {

			for (int j = 0; j < (inventorySize / inventoryRows); j++) {

				GUI.Button(new Rect((inventoryPadding * 2) + ((inventorySlot + inventoryPadding)*j), inventoryHeader + ((inventorySlot + inventoryPadding)* i), inventorySlot, inventorySlot),"");
			}
		}
		
	}
	
	void CheckWindowState(string actionCode){
		
		if (actionCode == "GM5") {
			
			playerInventory = !playerInventory;
			
		}
	}

	// Update is called once per frame
	void FixedUpdate () {
		
		// Upon left click will bar moving is true, unlock bar will be false and bar will stay at current position
		if (Input.GetButton ("LeftClick") && unlockInventory) {
			unlockInventory = false;
		}
		
			// If UnlockBar is true bar anchors will track to mouse x / y location
		if (unlockInventory) {
				
			inventoryAnchorX = Input.mousePosition.x;
			inventoryAnchorY = Screen.height - Input.mousePosition.y;
			UpdateWindowSizes ();
		}
	}
	// Update is called once per frame
	void Update () {
		
	}
}