/*
 * Filename: ProjectileScript.cs
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
/// The Projectile script provides the basic funcationality of a projectile spell. This script
/// has become obselete, as its functionality has been split between PhysicsProjectileScript and
/// SeekingProjectileScript. However, we decided to keep the script for future reference.
/// </summary>
public class ProjectileScript : MonoBehaviour {
	
	/// <summary>
	/// The destination of the projectile.
	/// </summary>
	Vector3 destination;
	
	/// <summary>
	/// The movespeed of the projectile.
	/// </summary>
	private float movespeed = 0.2f;
	
	/// <summary>
	/// The time at which the projectile should be destroyed.
	/// </summary>
	private float deathTime;
	
	/// <summary>
	/// The radius at which objects will interact with the projectile.
	/// </summary>
	private float radius = 5;
	
	/// <summary>
	/// The damage of the projectile.
	/// </summary>
	private float damage = 5;
	
	/// <summary>
	/// The current Player.
	/// </summary>
	private PlayerScript player;

	// Use this for initialization
	void Start () {
		deathTime = Time.time + 15;
	}
	
	// Update is called once per frame
	void Update () {
		if(Time.time > deathTime){
			Destroy(gameObject);
			return;
		}
		
		Vector3 playerPosition = player.getGameObject().transform.position;
		Vector3 misslePosition = gameObject.transform.position;
		
		misslePosition.y = playerPosition.y; //Do not worry about differences in Y
		
		if(Vector3.Distance(playerPosition, misslePosition) < radius)
		{
			player.damage(damage);
			Destroy(gameObject);
		}
		
		//TODO Implement pathfinding algorithm to avoid running through objects
			int directionX = destination.x > gameObject.transform.position.x ? 1 : -1;
			
			float deltaX = destination.x - gameObject.transform.position.x;
			if(Mathf.Abs(deltaX) > movespeed)
				deltaX = movespeed;
			
			int directionZ = destination.z > gameObject.transform.position.z ? 1 : -1;
			
			float deltaZ = destination.z - gameObject.transform.position.z;
			if(Mathf.Abs(deltaZ) > movespeed)
				deltaZ = movespeed;
			
			Vector3 newPos = new Vector3(gameObject.transform.position.x + (deltaX * directionX), gameObject.transform.position.y, gameObject.transform.position.z + (deltaZ * directionZ));
			
			gameObject.transform.position = newPos;
	}
	
	/// <summary>
	/// Sets the destination of the projectile.
	/// </summary>
	/// <param name='dest'>
	/// Destination.
	/// </param>
	public void setDest(Vector3 dest){
		destination = dest;	
	}
	
	/// <summary>
	/// Sets the current Player.
	/// </summary>
	/// <param name='player'>
	/// Current Player.
	/// </param>
	public void setPlayer(PlayerScript player){
		this.player = player;	
	}
	
}
