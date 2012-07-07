/*
 * Filename: CreditScreenScript.cs
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
/// The Credit Screen script displays basic credits information pertaining to the creation
/// of this game, as well as buttons for navigation.
/// </summary>
public class CreditScreenScript : MonoBehaviour {
	public Texture aTexture;  		//"Credits"
	public Texture buttonTexture; 	//"Back"
	public Texture buttonTexture2;  //"Back" when hovered
	public Texture backTexture;		//"Back" when not hovered
	public Texture logoTexture; 	//Logo
	public Texture creditsTexture;  //Actual Credits
	
	
	private float[] creditsCoords = {0.474f, 0.523f, 0.025f, 0.058f};
	
	Light light;
	
	// Use this for initialization
	void Start () {
		light = GetComponent(typeof(Light)) as Light;
	}
	
	
	void OnGUI() {
		
		GUI.DrawTexture(new Rect((Screen.width / 2) - 205, 20,410,188), logoTexture, ScaleMode.StretchToFill, true, 10.0F);
		GUI.DrawTexture(new Rect((Screen.width / 2) - 60, 210, 120, 32), aTexture, ScaleMode.StretchToFill, true, 10.0F);
		GUI.DrawTexture(new Rect((Screen.width / 2) - 379, 250, 758, 665),creditsTexture, ScaleMode.StretchToFill, true, 10.0F);
	    GUI.DrawTexture(new Rect((Screen.width / 2) - 42, Screen.height - 60, 84, 33), buttonTexture, ScaleMode.StretchToFill, true, 10.0F);
		
	}
	
	// Update is called once per frame
	void Update () {
		
		int x = (int) Input.mousePosition.x;
		int y = (int) Input.mousePosition.y;
		
		bool overCredits = (x > (creditsCoords[0] * Screen.width) && x < (creditsCoords[1] * Screen.width) && y > (creditsCoords[2] * Screen.height) && y < (creditsCoords[3] * Screen.height));

		
		Debug.Log("Position! " + Input.mousePosition.x + ", " + Input.mousePosition.y);
	
		if(overCredits){
			buttonTexture = buttonTexture2;
		}
		else
			buttonTexture = backTexture;
		
		if(Input.GetButtonDown("Fire1")){ //Left click
			//Credits Screen
			if(overCredits){
				Application.LoadLevel("CharacterSelection");
			}
		}
		else{
			
		}
		
	}
	
}
