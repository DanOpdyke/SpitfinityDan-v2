using UnityEngine;
using System.Collections;

public class CharacterSelectionLight : MonoBehaviour {
	private float[] garenCoords = {0.365f, 0.469f, 0.242f, 0.56f};	
	private float[] vayneCoords = {0.516f, 0.5875f, 0.229f, 0.549f};
	private float[] creditsCoords = {0.465f, 0.535f, 0.025f, 0.058f};
	
	private int garenMask = 8;
	
	private int vayneMask = 9;
	
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
				PlayerPrefs.SetString("Character", "Garen");
				Application.LoadLevel("Main");
			}
			
			//Vayne
			else if(overVayne){
				PlayerPrefs.SetString("Character", "Vayne");
				Application.LoadLevel("Main");
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