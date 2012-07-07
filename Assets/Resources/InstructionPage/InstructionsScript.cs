using UnityEngine;
using System.Collections;

public class InstructionsScript : MonoBehaviour {
	public Texture aTexture;  		//"Instruction Screen"
	public Texture buttonTexture; 	//"Continue"
	public Texture buttonTexture2;  //"Continue" when hovered
	public Texture continueTexture;	//"Continue" when not hovered
		
	
	private float[] buttonCoords = {0.474f, 0.523f, 0.025f, 0.058f};
	
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
		
	}
	
	// Update is called once per frame
	void Update () {
		
		int x = (int) Input.mousePosition.x;
		int y = (int) Input.mousePosition.y;
		
		bool overButton = (x > (buttonCoords[0] * Screen.width) && x < (buttonCoords[1] * Screen.width) && y > (buttonCoords[2] * Screen.height) && y < (buttonCoords[3] * Screen.height));

		
		Debug.Log("Position! " + Input.mousePosition.x + ", " + Input.mousePosition.y);
	
		if(overButton){
			buttonTexture = buttonTexture2;
		}
		else
			buttonTexture = continueTexture;
		
		if(Input.GetButtonDown("Fire1")){ //Left click
			//Credits Screen
			if(overButton){
				Application.LoadLevel("Main");
			}
		}
		else{
			
		}
		
	}
	
}
