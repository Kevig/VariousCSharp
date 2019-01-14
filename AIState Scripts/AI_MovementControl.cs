using UnityEngine;
using System.Collections;

public class AI_MovementControl : MonoBehaviour 
{
	
public Vector3 moveTo;	// Storage for world coordinates to move object to

private float reCalcTimer; // Timer that defines when a moveTo coordinate calculation should occur
private float panicTimer;
private float tempX;	// Temp x/z coordinates storage used for moveTo recalculations
private float tempZ;
private float movementSpeed;
private int movementMode;

public float minRouteChangeTimer;	// Min value recalc time can be
public float MaxRouteChangeTimer;	// Max value recalc time can be

public string unitName;	// Name of this unit

public NavMeshAgent civilian; // Navmesh agent reference

public delegate void ThreatBroadcast(string unitName, int threatLevel, string threatName);
public static event ThreatBroadcast OnThreatBroadcast;

// Use this for initialization
void Start () 
{

// Listener for threatzone broadcasts, used for turning off this scripts action
// upon receiving a state change event
ThreatZoneControl.OnThreatBroadcast += this.StateChangeEvent;
AI_MovementControl.OnThreatBroadcast += this.StateChangeEvent;

this.civilian = this.gameObject.GetComponent<NavMeshAgent>();
this.unitName = this.gameObject.name;
		
this.reCalcTimer = 1.0f;
this.panicTimer = 0.0f;

StateChangeEvent (this.unitName, 0, null);

}

void StateChangeEvent(string unitName, int threatLevel, string threatName)
{

        if (unitName == this.unitName && movementMode != 1) 
        {
		this.movementMode = threatLevel;
		this.panicTimer = Time.time + 10.0f;
		MovementSpeedCheck();
        }
	
}

// Update is called once per frame
void Update () 
{

	if (Time.time > reCalcTimer) 
        {
		MovementSpeedCheck ();
	}

	if (Time.time > panicTimer) 
        {
		movementMode = 0;
		OnThreatBroadcast(this.unitName, 0, null);
	}

}




void MovementSpeedCheck()
{
        GameObject CheckUnitData = GameObject.Find (this.unitName);
	CivilianData civilianData = CheckUnitData.GetComponent<CivilianData> ();
		
	if (this.movementMode == 0)
        {
	
        this.movementSpeed = civilianData.walkSpeed;
	this.reCalcTimer = (Random.Range(this.minRouteChangeTimer,this.MaxRouteChangeTimer))+ Time.time;
	
        }
	
        if (this.movementMode == 1){
	
        this.movementSpeed = civilianData.runSpeed;
	this.reCalcTimer = Time.time + 2.0f;

	}

	MoveCivilian ();

}

void MoveCivilian()
{

        GameObject CheckWorldSize = GameObject.Find ("WorldData");
	WorldData worldData = CheckWorldSize.GetComponent<WorldData> ();

	this.tempX = Random.Range (worldData.minCitySizeX, worldData.maxCitySizeX);
	this.tempZ = Random.Range (worldData.minCitySizeZ, worldData.maxCitySizeZ);

	this.moveTo = new Vector3 (tempX, transform.position.y, tempZ);

	this.civilian.SetDestination (this.moveTo);
	this.civilian.speed = this.movementSpeed;
	
	}

}

