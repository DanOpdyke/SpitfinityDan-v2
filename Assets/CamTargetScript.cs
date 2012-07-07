/*
 * Filename: CamTargetScript.cs
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
/// CamTargetScripts is used to track the movement of the Player with the camera, while maintaining
/// a fixed rotation. All current standard Unity3D camera scripts do not support a fixed rotation for
/// the camera, but rather rotate with the Player. CamTargetScript is also responsible for instantiating
/// the Player at game launch, using preferences set by the Character Selection screen.
/// </summary>
public class CamTargetScript : MonoBehaviour {
	
	/// <summary>
	/// Reference to the current Player object. This Player object will be constantly followed by
	/// the camera.
	/// </summary>
	public GameObject player;
	
	/// <summary>
	/// Reference to a generic Garen object. Provided as an option to instantiate at the beginning
	/// of a game. 
	/// </summary>
	public GameObject Garen;
	
		
	/// <summary>
	/// Reference to a generic Vayne object. Provided as an option to instantiate at the beginning
	/// of a game. 
	/// </summary>
	public GameObject Vayne;

	// Use this for initialization
	void Start () {
		string characterType = PlayerPrefs.GetString("Character");
		
		if(characterType.Equals("Garen"))
			player = (GameObject) Instantiate(Garen);
		else if(characterType.Equals("Vayne"))
			player = (GameObject) Instantiate(Vayne);
		
		
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = player.transform.position;
	}
}
