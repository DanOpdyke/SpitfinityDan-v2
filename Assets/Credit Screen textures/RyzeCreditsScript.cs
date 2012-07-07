/*
 * Filename: RyzeCreditsScript.cs
 * 
 * Author:
 * 		Programming: David Spitler
 * 
 * Last Modified: 7/6/2012
 * 
 * NOTE: All Models, Original Character Concepts, and Icons are property of Riot Games.
 * */
using UnityEngine;
using System.Collections;

/// <summary>
/// The Ryze Credits Script loops different Ryze animations while the Player views
/// the credits screen.
/// </summary>
public class RyzeCreditsScript : MonoBehaviour {
	
	/// <summary>
	/// Ryze's animation object.
	/// </summary>
	Animation animator;
	
	/// <summary>
	/// The name of the animation clips to be randomly looped.
	/// </summary>
	string[] clips = {"Idle1", "Idle2", "Laugh", "Taunt"};
	
	// Use this for initialization
	void Start () {
		animator = GetComponent(typeof(Animation)) as Animation;
	}
	
	// Update is called once per frame
	void Update () {
		if(!animator.isPlaying)
		{
			animator.Play(clips[Random.Range(0, 4)]);	
		}
	}
}