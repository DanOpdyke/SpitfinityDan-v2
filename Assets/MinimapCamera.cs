/*
 * Filename: MinimapCamera.cs
 * 
 * Author:
 * 		Programming: Daniel Opdyke, David Spitler
 * 
 * Last Modified: 7/6/2012
 * 
 * NOTE: All Models, Original Character Concepts, and Icons are property of Riot Games.
 * */
using UnityEngine;
using System.Collections;

/// <summary>
/// The Minimap camera scripts controls the secondary camera seen in the upper left corner of
/// the screen, ensuring that the Player is always centered.
/// </summary>
public class MinimapCamera : MonoBehaviour {
	GameObject player;
	float aspectRatio;
	Camera camera;
	
	// Use this for initialization
	void Start () {
		camera = gameObject.GetComponent(typeof(Camera)) as Camera;
	}
	
	// Update is called once per frame
	void Update () {
		aspectRatio = Screen.height / (float) Screen.width;
		float height = 0.22f;
		float width = height * aspectRatio;
		
		camera.rect = new Rect(1 - width, 1 - height, width, height);
		
		if(player == null)
			player = GameObject.FindGameObjectWithTag("TopLevelObject") as GameObject;
		Vector3 newPosition = player.transform.position;
		newPosition.y = gameObject.transform.position.y;
		gameObject.transform.position = newPosition;
	}
}
