using UnityEngine;
using System.Collections;

public class GruntController : MonoBehaviour {
	
	public Transform Grunt; // Assign transform control to prefab object "Grunt"
	public GameObject selectionHighlight; // Assign GameObject / name to Selection highlight prefab - Red target select Square
	public GameObject MoveToTag; // Assign GameObject / name to MoveToTag highlight prefab - blue move to target object
	public GameObject playerPortal;


	public int maxHp = 100;
	public int currentHp = 100;

	public bool _UnitSelected = false; // Logic - Is "Grunt selected"
	private bool highlight = false; // Logic - temp highlight for mouse over target square tracking
	private bool highlightTracker = false; // Logic - highlight tracker for if target is selected 
	private bool moveSelecter = false; // Logic - tracker for is MovetoTag placed

	private float windowOffset = 10; // Offset for popup unit window

	private const int gruntWindow_ID = 0; // Grunt window ID not in use yet
	private Rect gruntWindowRect = new Rect(0,0,0,0); // null values for grunt window initialisation


	public float buttonHeight = 50; // Button data
	public float buttonWidth = 50;
	public float gruntWindowHeight = 100; // Window data
	public float gruntWindowWidth = 150;
	public float healthBarLength;

	public float smooth; // Unit movement smoothing value

	public Vector3 unitPos; // x,y,z Unit current location, updated per frame
	public Vector3 portalPos; // Portal position storing variable, populated after pulling grid coords from mapping / portalplacement classes

	private Vector3 moveTo; // x,y,z coords of Mouse click, populated by raycasting
	private float moveToDistance; // Variable used in raycast calc

	// Called upon object initialization
	void Start () {
		
		// Find scripts by referencing the tags of the "fake" objects scripts are attached to.
		// Get data from those scripts, data used here is grid position of portal x,z and the x,y,z coordinates of the given grid position.
		GameObject wMapping = GameObject.FindWithTag ("worldMapping");
		WorldMapping worldMapping = wMapping.GetComponent <WorldMapping> ();

		GameObject portal = GameObject.FindWithTag ("playerPortal");
		PortalPlacement portalPlacement = portal.GetComponent<PortalPlacement>();

		// portaltemp - temp storage of x,y,z values of portal, calculated by x,z values of portals grid location.
		Vector3 portaltemp = new Vector3(worldMapping.gridCoordinates[portalPlacement.portalX], worldMapping.floorHeight, worldMapping.gridCoordinates[portalPlacement.portalZ]);

		healthBarLength = (gruntWindowWidth - 10) / (maxHp/currentHp); // set value of HealthBarLength, variable size based on current HP.

	}

	void FixedUpdate(){



	}

	// Update, called once per frame.
	void Update () {

		GameObject playerPortal = GameObject.Find ("playerPortal");
		PlayerPortalControl playerPortalControl = playerPortal.GetComponent<PlayerPortalControl> ();

		if (playerPortalControl._PortalSelected == true) {
			_UnitSelected = false;
		}

		unitPos = new Vector3 (Grunt.position.x, Grunt.position.y ,Grunt.position.z); // Refreshing Units current world position

		MoveUnit (); // call MoveUnit to check if movement required

		AdjustCurrentHealth (0); // call AdjustCurrentHealth to check if Unit health change is required


		////////////////////////////////////////////////////////////////////
		// 'mouseActivate' is set to left click, if left click with a Non Building Unit selected, will activate unit movement
		// First action is to detect if a MovetoTag object already exists and destroy it if needed.
		// Second action is to ray cast to the world coordinates of the mouse click location and store the x,y,z in variable 'hit'
		// MoveTo Vector3 Variable is then populated with the 'hit' x / z and the units existing y value.
		// A call is then made to the 'CalculateMove' Method.

		if (Input.GetButtonDown ("mouseActivate")&& _UnitSelected == true) {
			
			if (moveSelecter == true){
				
				DestoryMoveToHighlight ();
				
			}
			
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			
			if (Physics.Raycast (ray, out hit, Mathf.Infinity)){
				
				moveTo = new Vector3(hit.point.x, unitPos.y ,hit.point.z);
				
				CalculateMove ();
				
			}
			
		}
		//////////////////////////////////////////////////////////////////


		if (_UnitSelected == true || moveSelecter == true) {

			HighlightSelection();

		}

		if (highlightTracker == false && _UnitSelected == false) {

			DestroyHighlight();

		}

		if (highlight == true && highlightTracker == false) {
			
			highlightTracker = true;
			if (_UnitSelected == false)
			{
				HighlightSelection();
			}
		}
		
		if (Input.GetButtonDown ("mouseScrollLock")) // mouseScrollLock refers to Right mouse holding, set in input manager, for scrolling world.
		{ 
			
			if (_UnitSelected == true)
			{
				_UnitSelected = false;
				DestroyHighlight();
			}
			
		}


	}

	// Updated every frame, but after Update has preformed its actions
	void LateUpdate(){

		if (_UnitSelected == true || moveSelecter == true) {
			
			DestroyHighlight();
			
		}

		if (_UnitSelected == false && moveSelecter == true) {

			DestroyHighlight ();
		}

	}

	public void OnMouseEnter() // If mouse over
	{
		highlight = true;
	}
	
	public void OnMouseExit() // if mouse NOT over
	{
		highlight = false;
		highlightTracker = false;
		if (_UnitSelected == false) {
			
			DestroyHighlight ();
		}
	}
	
	public void OnMouseDown() // left click on Object
	{
		_UnitSelected = true;
	}
	
	void HighlightSelection() // method for highlighting an object with highlighter object prefab
	{

		Vector3 tempSelectorLoc = new Vector3 (unitPos.x, 0.0f, unitPos.z); // Get unit's current location and store in temp vectorand setting y value to 0 so new object is place at floor level.

		//////////////////////////////////////////////////////////////////////
		// Creates a new GameObject, using temp x,y,z values just calculated, re-scales the object to match the selected objects scale.
		// This allows buildings, units or other sized objects to be selected and selection object to adjust size in relation to the selected object.
		// rename the newly created object for referencing
		GameObject SelectionHighlight = (GameObject)Instantiate (selectionHighlight, tempSelectorLoc,Quaternion.identity);
		SelectionHighlight.transform.localScale += new Vector3(0, 0, 0);
		SelectionHighlight.name = "unitHighlight";
		//////////////////////////////////////////////////////////////////////

	}
	
	void DestroyHighlight() // destroy highlight method, removes highlight object
	{
		GameObject unitHighlight = GameObject.Find ("unitHighlight"); // finds object with the renamed name.
		DestroyObject (unitHighlight); // destorys the object

	}

	// Unity function, called on implementation for the setup of GUI's, also called every frame.
	void OnGUI() {
		

		// If statement, so that Object menu will only appear when object is selected.
		if (_UnitSelected == true) {

			// Window setup information
			gruntWindowRect = GUI.Window (gruntWindow_ID, new Rect (windowOffset, windowOffset, windowOffset + gruntWindowWidth, windowOffset + gruntWindowHeight), GruntWindow, "Grunt");
			
			// Text display for health bar
			GUI.Box ( new Rect( windowOffset + 10, (windowOffset * 2) + 10, healthBarLength, 20), "Grunt Health");
			
			// Health Bar
			GUI.Box ( new Rect( windowOffset + 10, (windowOffset * 4) + 15, healthBarLength, 15),"");
			
		}
		
		
	}

	// Currently unused, will populate when setup working menu's system
	void GruntWindow(int id){
		
	}

	// change unit HP
	public void AdjustCurrentHealth(int adj){
		
		currentHp += adj;
		
		if (currentHp < 0) 
			currentHp = 0;
		
		
		
		if (currentHp > maxHp)
			currentHp = maxHp;
		
		if (maxHp < 1)
			maxHp = 1;
		
		healthBarLength = (gruntWindowWidth - 10) * (currentHp / (float)maxHp);
		
		
	}

	// pre-Move calc, creates visual object MoveToTag at pre-calc'd click location
	public void CalculateMove(){

		if (moveSelecter == true) {

			DestoryMoveToHighlight ();

			} else {

			moveSelecter = true;

			GameObject moveToTag = (GameObject)Instantiate (MoveToTag, moveTo, Quaternion.identity);
			moveToTag.name = "tempMoveToHighlight";
			}



	}

	// Destroy moveToTag visual object.
	public void DestoryMoveToHighlight(){

		GameObject tempMoveToHighlight = GameObject.Find ("tempMoveToHighlight");
		DestroyObject (tempMoveToHighlight);
		
		moveSelecter = false;
	
	}

	// Method handles unit Movement, called fromm Update every frame.
	public void MoveUnit(){

		// If statement for testing if MoveToTag object is placed and if MoveTo coordinates have been updated.
		// If conditions met, preforms movement action. Variable 'smooth' set in Unity, controls movement speed.
		// transform.position - 'assigned transform object at start of class'.position - 'world x,y,z'
		// is deleted and assigned a new Vector3 using 'MoveTowards' action.
		// Data in brackets = transform.position - Grunt Position, moveTo - Mouse Click x,y,z, smooth - speed, Time.deltaTime - Unity function.
		if (moveSelecter == true) {
	
		transform.position = Vector3.MoveTowards(transform.position, moveTo, smooth * Time.deltaTime);
		
		}
	}

	// Automatically called when 'Grunt' Object comes in contact with another objects collider
	void OnTriggerEnter(Collider other){

		// If statement for testing if entered collider belongs to MoveToTag Object - MoveToTag object is renamed 'tempMoveToHighlight' upon creation.
		// if true "is move selecter placed" - moveSelecter boolean variable, then value is set to false, and DestroyMoveToHighlight method is called.
		if (other.gameObject.name == "tempMoveToHighlight") {
			moveSelecter = false;
			DestoryMoveToHighlight ();
		}
	}

}

