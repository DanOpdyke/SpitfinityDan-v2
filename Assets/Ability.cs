/*
 * Filename: Ability.cs
 * 
 * Author:
 * 		Programming: Daniel Opdyke
 * 
 * Last Modified: 6/22/2012
 * 
 * */
using UnityEngine;
using System.Collections;

/// <summary>
/// The Ability interface specifies required methods for any Player spells. Currently, PlayerScripts are
/// responsible for understanding and executing their own spells, so the Ability interface is relatively
/// unused. However, each spell is still required to implement the interface, to ensure future scalability.
/// </summary>
public class Ability : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	// Executes the ability
	void Execute(){
	}
}
