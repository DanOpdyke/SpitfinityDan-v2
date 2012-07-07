/*
 * Filename: SwiftDeathDebuff.cs
 * 
 * Author:
 * 		Programming: Daniel Opdyke
 * 		Design: David Spitler
 * 
 * Last Modified: 7/6/2012
 * 
 * */
using UnityEngine;
using System.Collections;

/// <summary>
/// The Swift death debuff is applied when Vayne auto-attacks, and causes period
/// damage over time to its target.
/// </summary>
public class SwiftDeathDebuff : MonoBehaviour, Debuff {
	
	/// <summary>
	/// The time at which the Swift Death Debuff should be expired.
	/// </summary>
	private float expirationTime;
	
	/// <summary>
	/// The total duration of the Swift Death Debuff.
	/// </summary>
	private float duration = 5;
	
	/// <summary>
	/// The damage of the Swift Death Debuff.
	/// </summary>
	private float damage;
	
	/// <summary>
	/// The time at which the Swift Death Debuff will next do its damage.
	/// </summary>
	private float nextTick;
	
	/// <summary>
	/// The enemy afflicted by the SwiftDeathDebuff.
	/// </summary>
	private EnemyScript enemy;
	
	/// <summary>
	/// The texture of the SwiftDeathDebuff.
	/// </summary>
	private Texture2D texture;
	
	/// <summary>
	/// Initializes a new instance of the <see cref="SwiftDeathDebuff"/> class.
	/// </summary>
	/// <param name='enemy'>
	/// Enemy afflicted by the SwiftDeathDebuff.
	/// </param>
	public SwiftDeathDebuff(EnemyScript enemy){
		this.enemy = enemy;
		texture = Resources.Load("Debuffs/SwiftDeath") as Texture2D;
		expirationTime = Time.time + duration;
		damage = (GameObject.FindGameObjectWithTag("Player").GetComponent(typeof(PlayerScript)) as PlayerScript).WeaponDamage;
	}
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	#region Debuff implementation
	bool Debuff.hasExpired ()
	{
		return Time.time > expirationTime;
	}

	float Debuff.applyDebuff (float damage)
	{
		return damage;
	}
	
	public void update(){
		if(Time.time >= nextTick){
			nextTick = Time.time + 1;
			enemy.damage(damage / 5);
		}
	}

	void Debuff.applyStack (int numAdditionalStacks)
	{
		;//This debuff cannot be stacked
	}

	Texture2D Debuff.getTexture ()
	{
		return texture;
	}

	void Debuff.apply (PlayerScript player)
	{
		damage = player.WeaponDamage * 0.1f;
	}

	void Debuff.expire (PlayerScript player)
	{
		;
	}

	bool Debuff.stackable ()
	{
		return false;
	}

	bool Debuff.prolongable ()
	{
		return true;
	}

	void Debuff.refresh ()
	{
		expirationTime = Time.time + duration;
	}

	string Debuff.description ()
	{
		return "The target is bleeding for " + damage + " damage over 5 seconds.";
	}

	string Debuff.name ()
	{
		return "Shadow Death";
	}
	#endregion
}
