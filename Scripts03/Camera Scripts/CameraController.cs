using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	public Transform target;
	public float height;
	public float xSpeed = 250.0f;
	public float ySpeed = 250.0f;
	public float camDistance;
	public float zoomFactor;
	public int minCamZoom;
	public int maxCamZoom;

	private Transform _myTransform;
	private float x;
	private float y;
	private int camZoom = 0;


	private bool CamButtonDown = false;

	void Start(){

		_myTransform = transform;
		CameraSetup();

	}

	void Update(){

		if (Input.GetMouseButtonDown (2)) {
			CamButtonDown = true;
		}

		if (Input.GetMouseButtonUp (2)) {
			CamButtonDown = false;
		}
	
		if (Input.GetAxis ("Mouse ScrollWheel") > 0) // set camera zoom variable to forward position
		{
			camZoom = 2;
			CameraZoom();
		}
		
		if (Input.GetAxis ("Mouse ScrollWheel") < 0) // set camera zoom variable to backward position
		{
			camZoom = 1;
			CameraZoom ();
		}

	}

	void LateUpdate(){

		if (CamButtonDown) { // scroll button down.
			x += Input.GetAxis("Mouse X") * xSpeed * 0.02f;
			y -= Input.GetAxis("Mouse Y") * ySpeed * 0.02f;

			Quaternion rotation = Quaternion.Euler(y,x,0.0f);
			Vector3 Position = rotation * new Vector3(0.0f,0.0f, -camDistance)+target.position;

			_myTransform.rotation = rotation;
			_myTransform.position = Position;

		}


	}

	public void CameraSetup(){
		_myTransform.position = new Vector3 (target.position.x, target.position.y + height, target.position.z - camDistance);
		_myTransform.LookAt (target);
	}

	public void CameraZoom()
	{
		if(camZoom == 2) // Mouse scroll forward
		{
			if (camDistance <=minCamZoom)
			{
				return;
			}
			else
			{
				camDistance = camDistance - zoomFactor;
			}
		}
		
		if(camZoom == 1) // Mouse scroll backward
		{
			if (camDistance >=maxCamZoom)
			{
				return;
			}
			else
			{
				camDistance = camDistance + zoomFactor;
			}
		}
		
	}
}