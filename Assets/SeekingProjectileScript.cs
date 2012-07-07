/*
 * Filename: SeekingProjectileScript.cs
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
/// The Seeking Projectile script provides the functionality of a projectile spell which follows
/// its target. In contrast to PhysicsProjectScript, this spell will "lock on" to the
/// opponent. When instantiated, the projectile should immediately have its target, damage, 
/// and speed set, or it will not properly function.
/// </summary>
public class SeekingProjectileScript : MonoBehaviour {
	
	/// <summary>
	/// The movement speed of the projectile.
	/// </summary>
	private float speed;
	
	/// <summary>
	/// The damage this projectile will do when it collides with its target.
	/// </summary>
	private float damage;
	
	/// <summary>
	/// The target of the projectile.
	/// </summary>
	private GameObject target;
	
	/// <summary>
	/// The maximum distance at which the projectile will "hit' its target.
	/// </summary>
	private float hitDistance = 1;
	
	/// <summary>
	/// The time at which the projectile will dissapear if it does not have a target.
	/// </summary>
	private float nullTimeout = 0;
	
	/// <summary>
	/// The delay between when the projectile does not have a target, and when the nullTimeout
	/// is specified.
	/// </summary>
	private float nullTimeoutDelay = 10;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(target == null){
			if(nullTimeout > 0 && Time.time > nullTimeout)
				Destroy(gameObject);
			else if (nullTimeout == 0)
				nullTimeout = Time.time + nullTimeoutDelay;
		}
		else
			nullTimeout = 0;
		
		if(Vector3.Distance(gameObject.transform.position, target.transform.position) < hitDistance){
			PlayerScript player = target.GetComponent(typeof(PlayerScript)) as PlayerScript;
			if(player != null)
				player.damage(damage);
			else{
				EnemyScript enemy = target.GetComponent(typeof(EnemyScript)) as EnemyScript;
				enemy.damage(damage);
			}
				Destroy(gameObject);
		}
		else{
			//Face target
			Vector3 lookat = target.transform.position;
			lookat.y = gameObject.transform.position.y;
			gameObject.transform.LookAt(lookat);
			//Move towards target
			gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, target.transform.position, speed);
		}
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
	/// Sets the damage of the projectile.
	/// </summary>
	/// <param name='damage'>
	/// Damage.
	/// </param>
	public void setDamage(float damage){
		this.damage = damage;	
	}
	
	/// <summary>
	/// Sets the target of the projectile.
	/// </summary>
	/// <param name='target'>
	/// Target.
	/// </param>
	public void setTarget(GameObject target){
		this.target = target;
	}
}
