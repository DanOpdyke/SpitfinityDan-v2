/*
 * Filename: HealthOrbScript.cs
 * 
 * Author:
 * 		Programming: Daniel Opdyke
 * 
 * Last Modified: 7/6/2012
 * 
 * NOTE: All Models, Original Character Concepts, and Icons are property of Riot Games.
 * */
using UnityEngine;
using System.Collections;

/// <summary>
/// The Health Orb script mimics the behavior of a simple health pickup, dropped upon
/// minion dropped.
/// </summary>
public class HealthOrbScript : MonoBehaviour, PickUp {
	
	/// <summary>
	/// The amount of health restored when the orb is picked up.
	/// </summary>
	private float healAmount = 20;
	
	// Use this for initialization
	public void Start () {
	
	}
	
	// Update is called once per frame
	public void Update () {
	
	}
	
	/// <summary>
	/// Awards the Player health.
	/// </summary>
	/// <param name='player'>
	/// The Player whom will receive the health.
	/// </param>
	public void trigger(PlayerScript player){
		player.awardHealth(healAmount);
		Destroy(gameObject);
	}
}
