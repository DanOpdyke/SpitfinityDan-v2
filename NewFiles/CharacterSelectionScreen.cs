using UnityEngine;
using System.Collections;

public class CharacterSelectionScreen : MonoBehaviour {
	public GameObject Garen;
	public GameObject Vayne;
	public Texture aTexture;
	public Texture buttonTexture;
	public Texture creditsTexture;
	public Texture buttonTexture2;
	
	
	private float[] creditsCoords = {0.474f, 0.523f, 0.025f, 0.058f};
	
	// Use this for initialization
	void Start () {
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
			buttonTexture = creditsTexture;
		
		if(Input.GetButtonDown("Fire1")){ //Left click
			//Credits Screen
			if(overCredits){
				Application.LoadLevel("Credits");
			}
		}
	}
	
	void OnGUI() {
		
		GUI.DrawTexture(new Rect((Screen.width / 2) - 257, 20,514,352), aTexture, ScaleMode.StretchToFill, true, 10.0F);
	    GUI.DrawTexture(new Rect((Screen.width / 2) - 60, Screen.height - 60, 120, 32),buttonTexture, ScaleMode.StretchToFill, true, 10.0F);
		
	}
	
	
	
}
