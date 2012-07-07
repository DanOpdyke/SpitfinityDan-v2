/*
 * Filename: EnemyHealthBarRotation.cs
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
/// The Enemy Health Bar Rotation script is an alternative method for displaying the enemy's health bar
/// above their model. The script, when attached to a plane indicating the enemy's health, ensures that
/// the place is always facing the camera. Although this functionality has been relocated to the EnemyScript,
/// we decided to keep this script for future reference.
/// </summary>
public class EnemyHealthBarRotation : MonoBehaviour {
	public GameObject camera;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		transform.LookAt(camera.transform);
	}
}
