/*
 * Filename: CourageBuff.cs
 * 
 * Author:
 * 		Programming: Daniel Opdyke
 * 		Design: David Spitler
 * 
 * Last Modified: 7/6/2012
 * 
 * NOTE: All Models, Original Character Concepts, and Icons are property of Riot Games.
 * */
using UnityEngine;
using System.Collections;

/// <summary>
/// The Courage Buff script is one of the few "dummy" Debuff subclasses. Currently, the GarenScript
/// class determines if the courage buff is active, and modifies damage taken accordingly. This script
/// merely ensures that the buff icon is displayed above the Player's health bar.
/// </summary>
public class CourageBuff : MonoBehaviour, Debuff {
	private float expirationTime;
	private float duration = 3;
	private Texture2D texture;
	
	/// <summary>
	/// Initializes a new instance of the <see cref="CourageBuff"/> class.
	/// </summary>
	public CourageBuff(){
		texture = Resources.Load("GarenTextures/Courage") as Texture2D;
		expirationTime = Time.time + duration;
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
		expirationTime = Time.time + duration;
	}

	string Debuff.description ()
	{
		return "Garen is taking 30% less damage.";
	}

	string Debuff.name ()
	{
		return "Courage";
	}

	void Debuff.update ()
	{
		;
	}
	#endregion
}
