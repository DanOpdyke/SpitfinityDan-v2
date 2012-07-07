/*
 * Filename: LoadScript.cs
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
using System.IO;

/// <summary>
/// The Load script provides basical functionality of loading saved games. Each game is specified using the character class type,
/// time and date of last save, and level of the character. When a saved file is selected, the Player is placed in their last saved position,
/// with appropriate Items in their inventory and equipment arrays.
/// </summary>
public class LoadScript : MonoBehaviour {
	
	/// <summary>
	/// The x offset of the curren Button
	/// </summary>
	private float xOffset = 0;
	
	/// <summary>
	/// The y offset of the current Button
	/// </summary>
	private float yOffset = 0;
	public Texture buttonTexture; 	//"Back"
	public Texture buttonTexture2;  //"Back" when hovered
	public Texture backTexture;		//"Back" when not hovered
	
	/// <summary>
	/// Array containing save file names
	/// </summary>
	string[] saveFileNames;
	
	/// <summary>
	/// The percentage coordinates of the credits button.
	/// </summary>
	private float[] creditsCoords = {0.474f, 0.523f, 0.025f, 0.058f};

	// Use this for initialization
	void Start () {	
		if(!Directory.Exists("saves"))
			Directory.CreateDirectory("saves");
		saveFileNames = Directory.GetFiles("saves");
		Debug.Log("Total number of files: " + saveFileNames.Length);
	}
	
	// Update is called once per frame
	void Update () {
		int x = (int) Input.mousePosition.x;
		int y = (int) Input.mousePosition.y;

		bool overCredits = (x > (creditsCoords[0] * Screen.width) && x < (creditsCoords[1] * Screen.width) && y > (creditsCoords[2] * Screen.height) && y < (creditsCoords[3] * Screen.height));

	
		if(overCredits){
			buttonTexture = buttonTexture2;
		}
		else
			buttonTexture = backTexture;
		
		if(Input.GetButtonDown("Fire1")){ //Left click
			if(overCredits){
				Application.LoadLevel("CharacterSelection");
			}
		}
	}
	
	/// <summary>
	/// Draws the load screen buttons, and handles click events.
	/// </summary>
	void OnGUI(){
		xOffset = 0;
		yOffset = 0;
		
		float windowLength = Screen.width - (Screen.width / 5);
		
		float windowHeight = Screen.height / 2;
		
		int numRows = (int) Mathf.Sqrt(saveFileNames.Length);
		
		int numButtonsPerRow = Mathf.CeilToInt(saveFileNames.Length / (float)numRows);
		
		float buttonWidth = windowLength / numButtonsPerRow;
		
		float buttonHeight = windowHeight / numRows;
		
		GUI.BeginGroup(new Rect(Screen.width / 10, Screen.height / 4, windowLength, windowHeight));
		
		int counter = 0;
		
		foreach(string fileName in saveFileNames){
			System.IO.StreamReader reader = new System.IO.StreamReader(fileName);
			string totalData = reader.ReadToEnd();
			string buttonText = "";
			if(fileName.Contains("Garen"))
				buttonText = "Garen";
			else if(fileName.Contains("Vayne"))
				buttonText = "Vayne";
			else
				buttonText = "Error: Save file has invalid format.";
			buttonText += " \n " + getTaggedLine("<DateStamp>", totalData);
			buttonText += " \n Level: " + getTaggedLine("<Level>", totalData);
			if(GUI.Button(new Rect(xOffset, yOffset, buttonWidth, buttonHeight), buttonText)){
				PlayerPrefs.SetString("IsSaveGame", "true");
				PlayerPrefs.SetString("SaveFileName", fileName);
				if(fileName.Contains("Garen"))
					PlayerPrefs.SetString("Character", "Garen");
				else if(fileName.Contains("Vayne"))
					PlayerPrefs.SetString("Character", "Vayne");
				
				Application.LoadLevel("Main");
			}
			counter++;
			if(counter == numButtonsPerRow){
				counter = 0;
				yOffset += buttonHeight;
				xOffset = 0;
			}
			else
				xOffset += buttonWidth;
		}	
		
		GUI.EndGroup();
		
		GUI.DrawTexture(new Rect((Screen.width / 2) - (buttonTexture.width / 2), Screen.height - 60, buttonTexture.width, buttonTexture.height), buttonTexture, ScaleMode.StretchToFill, true, 10.0F);
	}
	
	/// <summary>
	/// Gets the string between two tagged areas. Primarily used in parsing data from a load file.
	/// </summary>
	/// <returns>
	/// The string between the two tagged areas.
	/// </returns>
	/// <param name='tag'>
	/// The tag to look for. IE: "<Position>".
	/// </param>
	/// <param name='data'>
	/// The string to be parsed.
	/// </param>
	/// <example>
	/// Given the tag "<Position>" and the string "<Position>Example String</Position>", this function
	/// would return "Example String".
	/// </example>
	private string getTaggedLine(string tag, string data){
		return data.Substring(data.IndexOf(tag) + tag.Length, data.IndexOf(tag.Replace("<", "</")) - data.IndexOf(tag) - tag.Length);
	}
}
