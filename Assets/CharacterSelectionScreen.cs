/*
 * Filename: CharacterSelectionScreen.cs
 * 
 * Author:
 * 		Programming: David Spitler, Daniel Opdyke
 * 		Design: David Spitler
 * 
 * Last Modified: 7/6/2012
 * 
 * NOTE: All Models, Original Character Concepts, and Icons are property of Riot Games.
 * */
using UnityEngine;
using System.Collections;

/// <summary>
/// The Character Selection Screen provides the basic functionality of loading saved games
/// and viewing the game credits to the Player.
/// </summary>
public class CharacterSelectionScreen : MonoBehaviour {
	public Texture aTexture;		 //Logo & "Choose Path"
	public Texture buttonTexture;    //"Credits" (redundant but needed for start of scene)
	public Texture creditsTexture;	 //"Credits"
	public Texture buttonTexture2;   //"Credits" filled with white
	public Texture loadTexture;      //"Load Saved Game" 
	public Texture loadTexture2;     //"Load Saved Game" filled with white
	public Texture loadButtonTexture;//"Load Saved Game" (redundant but needed for start of scene)
	
	
	private float[] creditsCoords = {0.474f, 0.523f, 0.025f, 0.058f};
	private float[] loadCoords = {0.4323f, 0.5729f, 0.0934f, 0.1286f};
	
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {		
		int x = (int) Input.mousePosition.x;
		int y = (int) Input.mousePosition.y;
		
		bool overCredits = (x > (creditsCoords[0] * Screen.width) && x < (creditsCoords[1] * Screen.width) && y > (creditsCoords[2] * Screen.height) && y < (creditsCoords[3] * Screen.height));
		bool overLoad = (x > (loadCoords[0] * Screen.width) && x < (loadCoords[1] * Screen.width) && y > (loadCoords[2] * Screen.height) && y < (loadCoords[3] * Screen.height));
	
		if(overCredits)
			buttonTexture = buttonTexture2;
		else
			buttonTexture = creditsTexture;
		if(overLoad)
			loadButtonTexture = loadTexture2;
		else
			loadButtonTexture = loadTexture;
			
		
		if(Input.GetButtonDown("Fire1")){ //Left click
			//Credits Screen
			if(overCredits){
				Application.LoadLevel("Credits");
			}
			else if(overLoad){
				Application.LoadLevel("LoadScreen");
			}
		}
	}
	
	/// <summary>
	/// Paints the general starting screen textures.
	/// </summary>
	void OnGUI() {
		
		GUI.DrawTexture(new Rect((Screen.width / 2) - 251, 20,503,334), aTexture, ScaleMode.StretchToFill, true, 10.0F);
	    GUI.DrawTexture(new Rect((Screen.width / 2) - 60, Screen.height - 60, 120, 32), buttonTexture, ScaleMode.StretchToFill, true, 10.0F);
		GUI.DrawTexture(new Rect((Screen.width / 2) - 152, Screen.height - 120, 304, 33), loadButtonTexture, ScaleMode.StretchToFill, true, 10.0F);
		
	}
	
	
	
}
