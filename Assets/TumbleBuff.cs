/*
 * Filename: TumbleBuff.cs
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
/// The Tumble Buff script is one of the few "dummy" Debuff subclasses. Currently, the Vayne
/// class determines if the Tumble buff is active, and modifies damage dealt accordingly. This script
/// merely ensures that the buff icon is displayed above the Player's health bar.
/// </summary>
public class TumbleBuff : MonoBehaviour, Debuff {
	
	/// <summary>
	/// Determines if the Tumble Buff has expired.
	/// </summary>
	private bool expired;
	
	/// <summary>
	/// The texture of the Tumble Buff.
	/// </summary>
	private Texture2D texture;
	
	/// <summary>
	/// The current Player.
	/// </summary>
	private VayneScript player;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	#region Debuff implementation
	bool Debuff.hasExpired ()
	{
		return expired;
	}

	float Debuff.applyDebuff (float damage)
	{
		
		expired = true;
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
		if(texture == null)
			texture = Resources.Load("Debuffs/Tumble") as Texture2D;;
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
		return "Vayne's next attack will do \n 30% additional damage.";
	}

	string Debuff.name ()
	{
		return "TumbleBuff";
	}
	
	public void update(){
		;
	}
	#endregion
}
