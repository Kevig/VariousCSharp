using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	public Transform cameraTarget; // Target of camera

	public float camDistance; // Camera distance on Z axis from CameraTarget
	public float camHeight; // Camera Height Y axis from CameraTarget
	public float xSpeed;	// x Axis Rotation speed
	public float ySpeed;	// y Axis Rotation speed
	public float rotationDamping;	//
	public float heightDamping;		//
	public float zoomFactor;		// distance to zoom per zoom event
	public int minCamZoom;			// Closet zoom distance from CameraTarget
	public int maxCamZoom;			// Maximum zoom distance from CameraTarget

	public bool cameraFollow = true;	// Is camera following Target, boolean

	private float x;				// Mouse screen X coordinate storage
	private float y;				// Mouse screen Y coordinate storage
	private float currentRotationAngle; // Current rotation angle of camera
	private int camZoom = 0;			// Camera scroll zoom control:
										// 0 = null, 1 = Backwards, 2 = Forward.
	
	private bool camButtonDown = false; //Boolean value for button to activate camera rotation
	
	private Transform defaultTransform;	// Default position of Camera behind player object
	private Vector3 position;			// Vector storage for new worldspace position coordinates

	void Awake(){

		cameraTarget = GameObject.Find ("PlayerUnit1").transform;

		defaultTransform = transform;

	}

	// Use this for initialization
	void Start () {

		// Listener for broadcasts from SelectionManager
		SelectionManager.OnBroadcast += this.CheckUnitSelected;

		PositionCamera ();
	}

	// Changing the transform of camera
	void CheckUnitSelected(string unitName, string targetName){
		if (unitName != null) {
			cameraTarget = GameObject.Find (unitName).transform;
			PositionCamera ();
		}

	}

	// Update is called once per frame
	void Update () {
	
		if (Input.GetButtonDown ("turnLock")) {
			camButtonDown = true;
		}

		if (Input.GetButtonUp ("turnLock")) {
			camButtonDown = false;
		}

		//if (Input.GetButtonDown ("LeftClick")) {
		//	camButtonDown = true;
		//}
		
		//if (Input.GetButtonUp ("LeftClick")) {
		//	camButtonDown = false;
		//}

		if (camButtonDown) {
			Screen.lockCursor = true;
		} else {
			Screen.lockCursor = false;
		}

		if (Input.GetButtonDown ("DetachCamera")) {
			cameraFollow = !cameraFollow;
		}

		if (cameraFollow == true) {

			if (Input.GetAxis ("MouseScrollWheel") > 0) { // set camera zoom variable to forward position
			camZoom = 2;
			CameraZoom ();
			}
		
			if (Input.GetAxis ("MouseScrollWheel") < 0) { // set camera zoom variable to backward position
				camZoom = 1;
				CameraZoom ();
			}
		}

	}

	void LateUpdate(){

		if (camButtonDown == true) {

			x += Input.GetAxis ("Mouse X") * xSpeed * 0.02f;
			y -= Input.GetAxis ("Mouse Y") * ySpeed * 0.02f;

			Quaternion rotation = Quaternion.Euler (y + 33, x + currentRotationAngle, 0);

			position = rotation * new Vector3 (0.0f, 0.0f, -camDistance) + cameraTarget.position;

			defaultTransform.rotation = rotation;
			defaultTransform.position = position;
		
		} else if (cameraFollow == true) {

			x = 0;
			y = 0;

			float wantedRotationAngle = cameraTarget.eulerAngles.y;
			float wantedHeight = cameraTarget.position.y + camHeight;

			currentRotationAngle = defaultTransform.eulerAngles.y;
			float currentHeight = defaultTransform.position.y;

			currentRotationAngle = Mathf.LerpAngle (currentRotationAngle, wantedRotationAngle, rotationDamping * Time.deltaTime);
			currentHeight = Mathf.Lerp (currentHeight, wantedHeight, heightDamping * Time.deltaTime);

			Quaternion currentRotation = Quaternion.Euler (0,currentRotationAngle,0);

			defaultTransform.position = cameraTarget.position;
			defaultTransform.position -= currentRotation * Vector3.forward * camDistance;

			defaultTransform.position = new Vector3(defaultTransform.position.x, currentHeight, defaultTransform.position.z);

			defaultTransform.LookAt(cameraTarget);


		}

	}

	void PositionCamera(){

		//defaultTransform.position = new Vector3 (cameraTarget.position.x, cameraTarget.position.y + camHeight, cameraTarget.position.z - camDistance);
		defaultTransform.LookAt (cameraTarget);
	}

	void CameraZoom(){

		if(camZoom == 2) // Mouse scroll forward
		{
			if (camHeight <=minCamZoom)
			{
				return;
			}
			else
			{
				camDistance = camDistance - zoomFactor;
				camHeight = camHeight - zoomFactor;
			}

		}
		
		if(camZoom == 1) // Mouse scroll backward
		{
			if (camHeight >=maxCamZoom)
			{
				return;
			}
			else
			{
				camDistance = camDistance + zoomFactor;
				camHeight = camHeight + zoomFactor;
			}
		}

		if (camZoom != 0) {
			camZoom = 0;
			PositionCamera();
		}
	}

}
