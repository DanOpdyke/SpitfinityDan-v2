/*
 * Filename: CheckpointScript.cs
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
/// The Checkpoint script provides a zone which, when triggered, saves the Player's current state. Unlike
/// spawn zones, checkpoints can be triggered multiple times within a single playthrough.
/// </summary>
public class CheckpointScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	/// <summary>
	/// Saves the Player's current game state.
	/// </summary>
	/// <param name='other'>
	/// The collider of the Player.
	/// </param>
	void OnTriggerEnter(Collider other){
		PlayerScript player = other.GetComponentInChildren(typeof(PlayerScript)) as PlayerScript;
		if(player != null){
			player.Save();	
		}
		else{
			Debug.Log("Entered area, but could not find PlayerScript");
		}
	}
}
