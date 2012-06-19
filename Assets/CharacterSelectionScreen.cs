using UnityEngine;
using System.Collections;

public class CharacterSelectionScreen : MonoBehaviour {
	public GameObject Garen;
	public GameObject Vayne;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnGUI() {
		GUI.Window(0, new Rect(0, 0, Screen.width, Screen.height), drawScreen, "");
	}
	
	void drawScreen(int windowID){
		GUIStyle labelStyle = new GUIStyle();
		labelStyle.alignment = TextAnchor.UpperCenter;
		labelStyle.fontSize = 50;
		
		float labelWidth = Screen.width / 2.5f;
		GUI.Label(new Rect((Screen.width / 2) - (labelWidth / 2), 30, Screen.width / 2.5f, 150), "Art of Anguish", labelStyle);
		
		labelStyle.fontSize = 36;
		GUI.Label(new Rect((Screen.width / 2) - (labelWidth / 2), 400, Screen.width / 2.5f, 150), "Choose Your Path:", labelStyle);
		
		
		float buttonLength = Screen.width / 6;
		if(GUI.Button(new Rect((Screen.width / 2) - buttonLength - 20, 600, buttonLength, buttonLength / 3), "Garen")){
			PlayerPrefs.SetString("Character", "Garen");
			Application.LoadLevel("Main");
		}
		
		if(GUI.Button(new Rect((Screen.width / 2) + 20, 600, buttonLength, buttonLength / 3), "Vayne")){
			PlayerPrefs.SetString("Character", "Vayne");
			Application.LoadLevel("Main");
		}
			
	}
}
