/*
 * Filename: FinalHourBuff.cs
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
/// The Final Hour Buff script is one of the few "dummy" Debuff subclasses. Currently, the Vayne
/// class determines if the Final Hour buff is active, and modifies damage dealt accordingly. This script
/// merely ensures that the buff icon is displayed above the Player's health bar.
/// </summary>
public class FinalHourBuff : MonoBehaviour, Debuff {
	
	/// <summary>
	/// The time at which the Final Hour Buff should be expired.
	/// </summary>
	private float expireTime;
	
	/// <summary>
	/// The total duration of the Final Hour Buff.
	/// </summary>
	private float duration = 8.4f;
	
	/// <summary>
	/// The texture of the Final Hour Buff.
	/// </summary>
	private Texture2D texture;
	
	/// <summary>
	/// Initializes a new instance of the <see cref="FinalHourBuff"/> class.
	/// </summary>
	public FinalHourBuff(){
		texture = Resources.Load("Debuffs/FinalHour") as Texture2D;
		expireTime = Time.time + duration;
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
		return expireTime < Time.time;
	}

	float Debuff.applyDebuff (float damage)
	{
		return damage;
	}

	void Debuff.applyStack (int numAdditionalStacks)
	{
		;
	}

	Texture2D Debuff.getTexture ()
	{
		return texture;
	}

	void Debuff.apply (PlayerScript player)
	{
		;
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
		return false;
	}

	void Debuff.refresh ()
	{
		;
	}

	string Debuff.description ()
	{
		return "Vayne deals 40% bonus damage and \n heals for 50% of damage caused.";
	}

	string Debuff.name ()
	{
		return "FinalHour";
	}

	void Debuff.update ()
	{
		;
	}
	#endregion
}
