using UnityEngine;
using System.Collections;

public class CameraTargetController : MonoBehaviour {

	public float speed;
	
	private bool camButtonDown = false; // Right mouse
	private Transform targetPosition;


	void Awake(){

	}


	// Use this for initialization
	void Start () {

		GameObject wMapping = GameObject.FindWithTag ("worldMapping");
		WorldMapping worldMapping = wMapping.GetComponent <WorldMapping> ();

		GameObject portal = GameObject.FindWithTag ("playerPortal");
		PortalPlacement portalPlacement = portal.GetComponent<PortalPlacement>();

		transform.Translate (new Vector3 (worldMapping.gridCoordinates [portalPlacement.portalX], worldMapping.floorHeight, worldMapping.gridCoordinates [portalPlacement.portalZ]));

	}

	void FixedUpdate (){

	}

	// Update is called once per frame
	void Update () {
	
		if (Input.GetButtonDown ("mouseScrollLock")) // Mouse inputs 0 = left button, 1 = Right, 2 = Middle
		{ 
			camButtonDown = true;
		}
		
		if(Input.GetButtonUp("mouseScrollLock"))
		{
			camButtonDown = false;
		}
	

	}

	void LateUpdate(){

		if (camButtonDown == true) {
			
			float moveHorizontal = Input.GetAxis ("Mouse X");
			float moveVertical = Input.GetAxis ("Mouse Y");
			
			Vector3 movement = new Vector3 (-moveHorizontal, 0.0f, -moveVertical);
			
			rigidbody.velocity = movement * speed;
		}

		if (camButtonDown == false) {

			Vector3 stopScroll = new Vector3 (0.0f,0.0f,0.0f);

			rigidbody.velocity = stopScroll;
		}

	}
	
}
