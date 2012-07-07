/*
 * Filename: VayneInfoScript.cs
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
/// The Vayne Info script provides simple information pertaining to the Vayne character class,
/// as well as button navigation for starting a game.
/// </summary>
public class VayneInfoScript : MonoBehaviour {
	public Texture aTexture;  		  //Vayne's Info Screen
	public Texture buttonTexture; 	  //"Continue"
	public Texture buttonTexture2;    //"Continue" when hovered
	public Texture continueTexture;   //"Continue" when not hovered
	public Texture backTexture;       //"Back" when not hovered
	public Texture backTexture2;      //"Back" when hovered
	public Texture backButtonTexture; //"Back"
		
	
	private float[] buttonCoords = {0.474f, 0.523f, 0.025f, 0.058f};
	private float[] backCoords = {0.417f, 0.449f, 0.025f, 0.058f};
	
	Light light;
	
	// Use this for initialization
	void Start () {
		light = GetComponent(typeof(Light)) as Light;
	}
	
	
	void OnGUI() {
		
		
		//GUI.DrawTexture(new Rect((Screen.width / 2) - 60, 210, 120, 32), aTexture, ScaleMode.StretchToFill, true, 10.0F);
		//GUI.DrawTexture(new Rect((Screen.width / 2) - 379, 250, 758, 665),creditsTexture, ScaleMode.StretchToFill, true, 10.0F);
	    GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), aTexture, ScaleMode.StretchToFill, true, 10.0F);
		GUI.DrawTexture(new Rect((Screen.width / 2) - (buttonTexture.width / 2), Screen.height - 60, buttonTexture.width, buttonTexture.height), buttonTexture, ScaleMode.StretchToFill, true, 10.0F);
		GUI.DrawTexture(new Rect((Screen.width / 2) - (buttonTexture.width / 2) - (backButtonTexture.width / 2), Screen.height - 60, backButtonTexture.width, backButtonTexture.height), backButtonTexture, ScaleMode.StretchToFill, true, 10.0F);

		
	}
	
	// Update is called once per frame
	void Update () {
		
		int x = (int) Input.mousePosition.x;
		int y = (int) Input.mousePosition.y;
		
		bool overButton = (x > (buttonCoords[0] * Screen.width) && x < (buttonCoords[1] * Screen.width) && y > (buttonCoords[2] * Screen.height) && y < (buttonCoords[3] * Screen.height));
		bool overBack = (x > (backCoords[0] * Screen.width) && x < (backCoords[1] * Screen.width) && y > (backCoords[2] * Screen.height) && y < (backCoords[3] * Screen.height));

		
		Debug.Log("Position! " + Input.mousePosition.x + ", " + Input.mousePosition.y);
	
		if(overButton){
			buttonTexture = buttonTexture2;
		}
		else
			buttonTexture = continueTexture;
		if(overBack){
			backButtonTexture = backTexture2;
		}
		else
			backButtonTexture = backTexture;
		
		
		if(Input.GetButtonDown("Fire1")){ //Left click
			//Credits Screen
			if(overButton)
				Application.LoadLevel("Instructions");
			if(overBack)
				Application.LoadLevel("CharacterSelection");
			
		}		
	}
}

