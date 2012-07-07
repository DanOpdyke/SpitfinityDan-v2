/*
 * Filename: CharacterSelectionLight.cs
 * 
 * Author:
 * 		Programming: David Spitler, Daniel Opdyke
 * 
 * Last Modified: 7/6/2012
 * 
 * NOTE: All Models, Original Character Concepts, and Icons are property of Riot Games.
 * */
using UnityEngine;
using System.Collections;

/// <summary>
/// The Character Selection Light script provides a light over the currently selected Player in the
/// character selection screen, indicating the Player's choice.
/// </summary>
public class CharacterSelectionLight : MonoBehaviour {
	
	/// <summary>
	/// The percentage coordinates of the Garen model. When the mouse is within these coordinates,
	/// a light will appear over Garen.
	/// </summary>
	private float[] garenCoords = {0.365f, 0.469f, 0.242f, 0.56f};
	
	/// <summary>
	/// The percentage coordinates of the Vayne model. When the mouse is within these coordinates,
	/// a light will appear over Vayne.
	/// </summary>
	private float[] vayneCoords = {0.516f, 0.5875f, 0.229f, 0.549f};
	
	/// <summary>
	/// The percentage coordinates of the continue button. When the mouse is within these coordinates,
	/// the continue button texture will change.
	/// </summary>
	private float[] creditsCoords = {0.465f, 0.535f, 0.025f, 0.058f};
	
	/// <summary>
	/// The bitmask which, when applied to the light object, will only illuminate the Garen model.
	/// </summary>
	private int garenMask = 8;
	
	/// <summary>
	/// The bitmask which, when applied to the light object, will only illuminate the Vayne model.
	/// </summary>
	private int vayneMask = 9;
	
	/// <summary>
	/// The light object, which illuminates the Player's current character choice.
	/// </summary>
	Light light;
	
	// Use this for initialization
	void Start () {
		light = GetComponent(typeof(Light)) as Light;
	}
	
	// Update is called once per frame
	void Update () {
		
		int x = (int) Input.mousePosition.x;
		int y = (int) Input.mousePosition.y;
		
		bool overGaren = (x > (garenCoords[0] * Screen.width) && x < (garenCoords[1] * Screen.width) && y > (garenCoords[2] * Screen.height) && y < (garenCoords[3] * Screen.height));
		bool overVayne = (x > (vayneCoords[0] * Screen.width) && x < (vayneCoords[1] * Screen.width) && y > (vayneCoords[2] * Screen.height) && y < (vayneCoords[3] * Screen.height));
		
		bool overCredits = (x > (creditsCoords[0] * Screen.width) && x < (creditsCoords[1] * Screen.width) && y > (creditsCoords[2] * Screen.height) && y < (creditsCoords[3] * Screen.height));
		
		
		
		if(Input.GetButtonDown("Fire1")){ //Left click
			//Garen
			if(overGaren){
				PlayerPrefs.SetString("IsSaveGame", "false"); //New game - Do not load previous
				PlayerPrefs.SetString("Character", "Garen");
				Application.LoadLevel("GarenInfo");
			}			
			//Vayne
			else if(overVayne){
				PlayerPrefs.SetString("IsSaveGame", "false");
				PlayerPrefs.SetString("Character", "Vayne");
				Application.LoadLevel("VayneInfo");
			}
			else if(overCredits){
				Application.LoadLevel("Credits");
			}
		}
		else{
			if (overGaren)
				light.cullingMask = (1 << garenMask);	
			else if (overVayne)
				light.cullingMask = (1 << vayneMask);
			else
				light.cullingMask = 0;
		}
		
	}
	
}