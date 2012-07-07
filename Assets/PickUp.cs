/*
 * Filename: PickUp.cs
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
/// The Pick Up interface specifies required methods of any Items which should automatically be picked up
/// when colliding with the Player.
/// </summary>
public interface PickUp {

	// Use this for initialization
	void Start ();
	
	// Update is called once per frame
	void Update ();
	
	// Primarily function to be implemented by child classes.
	void trigger(PlayerScript player);
	
}
