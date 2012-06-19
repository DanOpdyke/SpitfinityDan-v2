using UnityEngine;
using System.Collections;

public class CamTargetScript : MonoBehaviour {
	public GameObject player;
	public GameObject Garen;
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
