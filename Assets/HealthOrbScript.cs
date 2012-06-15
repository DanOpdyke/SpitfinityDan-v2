using UnityEngine;
using System.Collections;

public class HealthOrbScript : MonoBehaviour, PickUp {
	private float healAmount = 20;
	
	// Use this for initialization
	public void Start () {
	
	}
	
	// Update is called once per frame
	public void Update () {
	
	}
	
	public void trigger(GarenScript player){
		player.awardHealth(healAmount);
		Destroy(gameObject);
	}
}
