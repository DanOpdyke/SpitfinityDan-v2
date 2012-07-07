/*
 * Filename: PhysicsProjectileScript.cs
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
/// The Physics Projectile script provides the functionality of a projectile spell which moves in
/// a set direction. In contrast to SeekingProjectileScript, this spell will not "lock on" to the
/// opponent. When instantiated, the projectile should immediately have its destination, damage, 
/// and speed set, or it will not properly function.
/// </summary>
public class PhysicsProjectileScript : MonoBehaviour {
	
	/// <summary>
	/// The physics rigidbody of the projectile.
	/// </summary>
	private Rigidbody rigidbody;
	
	/// <summary>
	/// The destination position of the projectile.
	/// </summary>
	private Vector3 destination;
	
	/// <summary>
	/// The time at which the projectile should be destroyed.
	/// </summary>
	private float timeToLive;
	
	/// <summary>
	/// The movement speed of the projectile.
	/// </summary>
	private float speed;
	
	/// <summary>
	/// The damage this projectile will do if it collides with its target.
	/// </summary>
	private float damage;
	
	// Use this for initialization
	void Start () {
		this.rigidbody = gameObject.GetComponent(typeof(Rigidbody)) as Rigidbody;
		timeToLive = Time.time + 10;
	}
	
	/// <summary>
	/// Sets the damage of the projectile.
	/// </summary>
	/// <param name='damage'>
	/// Damage.
	/// </param>
	public void setDamage(float damage){
		this.damage = damage;
	}
	
	/// <summary>
	/// Sets the speed of the projectile.
	/// </summary>
	/// <param name='speed'>
	/// Speed.
	/// </param>
	public void setSpeed(float speed){
		this.speed = speed;
	}
	
	/// <summary>
	/// Sets the time to live of the projectile.
	/// </summary>
	/// <param name='timeToLive'>
	/// Time to live.
	/// </param>
	public void setTimeToLive(float timeToLive){
		this.timeToLive = timeToLive;	
	}
	
	/// <summary>
	/// Sets the destination of the projectile.
	/// </summary>
	/// <param name='destination'>
	/// Destination.
	/// </param>
	public void setDestination(Vector3 destination){
		this.destination = destination;
		gameObject.transform.LookAt(destination);
		if(rigidbody == null)
			rigidbody = gameObject.GetComponent(typeof(Rigidbody)) as Rigidbody;
		rigidbody.velocity = transform.forward * speed;
	}
	
	// Update is called once per frame
	void Update () {
		if(timeToLive < Time.time)
			Destroy(gameObject);
	}
	
	/// <summary>
	/// Called when the projectile collides with a Player, resulting in damage done to
	/// the Player, and the destruction of the projectile.
	/// </summary>
	/// <param name='collision'>
	/// Collision.
	/// </param>
	void OnTriggerEnter(Collider collision){
		if(collision.gameObject == GameObject.FindGameObjectWithTag("Player")){
			PlayerScript player = collision.gameObject.GetComponent(typeof(PlayerScript)) as PlayerScript;
			player.damage(damage);
			Destroy(gameObject);
		}
	}
		
}
