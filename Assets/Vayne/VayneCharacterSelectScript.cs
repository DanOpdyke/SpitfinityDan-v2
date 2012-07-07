/*
 * Filename: VayneCharacterSelectScript.cs
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
/// The Vayne Character Select script is used to loop Vayne animations in the character selection screen.
/// </summary>
public class VayneCharacterSelectScript : MonoBehaviour {
	
	/// <summary>
	/// The animation object.
	/// </summary>
	Animation animator;
	
	/// <summary>
	/// The names of the clips to randomly loop.
	/// </summary>
	string[] clips = {"Idle1", "Idle2", "Idle3"};
	
	// Use this for initialization
	void Start () {
		animator = GetComponent(typeof(Animation)) as Animation;
	}
	
	// Update is called once per frame
	void Update () {
		if(!animator.isPlaying)
		{
			animator.Play(clips[Random.Range(0, clips.Length)]);	
		}
	}
}
