using UnityEngine;
using System.Collections;

public class PlayerPortalControl : MonoBehaviour {

	public GameObject selectionHighlight;
	public GameObject PlayerPortal;
	public GameObject Grunt;

	public int buildingTileSize;

	public int maxHp = 1000;
	public int currentHp = 1000;


	public bool _PortalSelected = false;
	private bool highlight = false;
	private bool highlightTracker = false;

	public Vector3 portalPos;

	private float windowOffset = 10;
	private const int portalWindow_ID = 0;
	private Rect portalWindowRect = new Rect(0,0,0,0);
	
	public float buttonHeight = 50;
	public float buttonWidth = 50;
	public float portalWindowHeight = 100;
	public float portalWindowWidth  =150;

	public float healthBarLength;

	// Use this for initialization
	void Start () {
	
		GameObject wMapping = GameObject.FindWithTag ("worldMapping");
		WorldMapping worldMapping = wMapping.GetComponent <WorldMapping> ();
		
		GameObject portal = GameObject.FindWithTag ("playerPortal");
		PortalPlacement portalPlacement = portal.GetComponent<PortalPlacement>();

		Vector3 portaltemp = new Vector3(worldMapping.gridCoordinates[portalPlacement.portalX], worldMapping.floorHeight, worldMapping.gridCoordinates[portalPlacement.portalZ]);
		portalPos = new Vector3 (portaltemp.x, portaltemp.y, portaltemp.z);

		healthBarLength = (portalWindowWidth - 10) / (maxHp/currentHp);

	}
	
	// Update is called once per frame
	void Update () {

		GameObject Grunt = GameObject.Find ("grunt");
		GruntController gruntController = Grunt.GetComponent<GruntController> ();

		if (gruntController._UnitSelected == true) {
			_PortalSelected = false;
			if (highlightTracker == false){
			DestroyHighlight();
			}
		}

		AdjustCurrentHealth (0);

		if (highlight == true && highlightTracker == false) {

			highlightTracker = true;
			if (_PortalSelected == false)
			{
				HighlightSelection();
			}
		}

		if (Input.GetButtonDown ("mouseScrollLock"))
		{ 

			if (_PortalSelected == true)
			{
				_PortalSelected = false;
				DestroyHighlight();
			}

		}

	}

	void LateUpdate(){
	
	
	}
	
	public void OnMouseEnter()
	{
		highlight = true;
	}

	public void OnMouseExit()
	{
		highlight = false;
		highlightTracker = false;
		if (_PortalSelected == false) {
		
			DestroyHighlight ();
		}
	}

	public void OnMouseDown()
	{
		_PortalSelected = true;
	
	}

	void HighlightSelection()
	{
			
		GameObject SelectionHighlight = (GameObject)Instantiate (selectionHighlight, portalPos,Quaternion.identity);
		SelectionHighlight.transform.localScale += new Vector3(-1, 0,-1);
		SelectionHighlight.name = "TempHighlight";

	}

	void DestroyHighlight()
	{
		GameObject TempHighlight = GameObject.Find ("TempHighlight");
		DestroyObject (TempHighlight);
	}

	void OnGUI() {

		if (_PortalSelected == true) {

		portalWindowRect = GUI.Window (portalWindow_ID, new Rect (windowOffset, windowOffset, windowOffset + portalWindowWidth, windowOffset + portalWindowHeight), PortalWindow, "Portal");
		
		// Text display for health bar
		GUI.Box ( new Rect( windowOffset + 10, (windowOffset * 2) + 10, healthBarLength, 20), "Portal Health");
		
		// Health Bar
		GUI.Box ( new Rect( windowOffset + 10, (windowOffset * 4) + 15, healthBarLength, 15),"");
		
		// Button for buying Grunt
		GUI.Button ( new Rect( windowOffset + 10, portalWindowHeight - (portalWindowHeight/4), portalWindowWidth - 10, (portalWindowHeight/4)),"Buy Grunt Button");
		
		

		}


	}

	void PortalWindow(int id){
	
	}

	public void AdjustCurrentHealth(int adj){
	
		currentHp += adj;

		if (currentHp < 0) 
			currentHp = 0;

		

		if (currentHp > maxHp)
			currentHp = maxHp;

		if (maxHp < 1)
			maxHp = 1;

		healthBarLength = (portalWindowWidth - 10) * (currentHp / (float)maxHp);

		
	}
	
}
