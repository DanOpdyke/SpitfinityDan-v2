/*
 * Filename: SpawnZoneScript.cs
 * 
 * Author:
 * 		Programming: Daniel Opdyke
 * 
 * Last Modified: 7/6/2012
 * 
 * */
using UnityEngine;
using System.Collections;

/// <summary>
/// The Spawn Zone script provides the funcationality of areas which, when entered,
/// will cause enemies to spawn. Currently, once a spawn zone has been triggered, it
/// cannot again be trigged until a future playthrough.
/// </summary>
public class SpawnZoneScript : MonoBehaviour {
	
	/// <summary>
	/// List of spawn points for this spawn zone. To save space, the y component of each vector3 is an index
	/// indicated the type of enemy to be spawned.
	/// </summary>
	private ArrayList spawnPoints;
	
	/// <summary>
	/// Array containing possible GameObjects that can be instantiated.
	/// </summary>
	private GameObject[] instantiateObjects;
	
	/// <summary>
	/// The number possible GameObject types.
	/// </summary>
	private int numPossibleTypes = 3;
	
	/// <summary>
	/// The enemy type identifiers.
	/// "M" Specifies Melee minions.
	/// "C" Specifies Caster minions.
	/// "B1" Specifies the first boss, Cassiopeia
	/// This array is used to provide indexes into the instantiate object array when
	/// reading from a spawn file.
	/// </summary>
	private string[] enemyTypeIds = {"M", "C", "B1"};
	
	/// <summary>
	/// The unique identifier of this spawn location, which must be the name of a 
	/// text file containing spawn location data.
	/// </summary>
	public string locationID;
	
	/// <summary>
	/// Determines if this spawn zone has already been triggered. Once a spawn zone has been triggered, it must be manually
	/// reset before it will spawn more enemies.
	/// </summary>
	private bool triggered;
	
	
	// Use this for initialization
	void Start () {
		instantiateObjects = new GameObject[numPossibleTypes];
		instantiateObjects[0] = GameObject.FindGameObjectWithTag("MeleeMinion");
		instantiateObjects[1] = GameObject.FindGameObjectWithTag("CasterMinion");
		instantiateObjects[2] = GameObject.FindGameObjectWithTag("Cassiopeia");
		
		
		spawnPoints = new ArrayList();
		System.IO.StreamReader reader = new System.IO.StreamReader("SpawnLocations/"+locationID+".txt");
		while(!reader.EndOfStream){
			string[] locations = reader.ReadLine().Split(',');
			Vector3 data = new Vector3(float.Parse(locations[1]), (float) System.Array.IndexOf(enemyTypeIds, locations[0]), float.Parse(locations[2]));
			spawnPoints.Add(data);
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	/// <summary>
	/// Spawns the units corresponding to this spawn zone, using the spawn location data file.
	/// </summary>
	private void spawnUnits(){
		foreach(Vector3 positionData in spawnPoints){
			//Instantiate new object
			GameObject newEnemy = (GameObject) Instantiate(instantiateObjects[(int)positionData.y]);
			
			//Each enemy has a different y position. As such, we ensure that only the x and z positions
			//are set.
			Vector3 newPosition = positionData;
			newPosition.y = newEnemy.transform.position.y;
			newEnemy.transform.position = newPosition;
		}
	}
	
	/// <summary>
	/// Sets this spawn zone as triggered.
	/// </summary>
	/// <param name='triggered'>
	/// If the spawn zone has been triggered.
	/// </param>
	public void setTriggered(bool triggered){
		this.triggered = triggered;	
	}
	
	/// <summary>
	/// Triggered when the Player enters the spawn zone, instantiating the appropriate
	/// minions.
	/// </summary>
	/// <param name='other'>
	/// The Player's collider.
	/// </param>
	void OnTriggerEnter(Collider other){
		//Only trigger once
		if(triggered)
			return;
		PlayerScript player = other.gameObject.GetComponent(typeof(PlayerScript)) as PlayerScript;
		if(player != null){
			spawnUnits();
			triggered = true;
		}
	}
}
