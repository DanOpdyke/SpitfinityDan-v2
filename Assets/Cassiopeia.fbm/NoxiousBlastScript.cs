using UnityEngine;
using System.Collections;

public class NoxiousBlastScript : MonoBehaviour {
	private float nextTick = Time.time + 4;
	private float lifeTime;
	
	// Use this for initialization
	void Start () {
		//Expire after 7 seconds
		lifeTime = Time.time + 7;
		
	}
	
	// Update is called once per frame
	void Update () {
		if(Time.time > lifeTime)
			Destroy(gameObject);
	}
	
	public void trigger(PlayerScript player){
		//TODO This should apply a debuff that should damage the player, and automatically wear off after a set duration.
		if (Time.time > nextTick){
			//player.damage(4);	
			player.setPoisoned(true);
			nextTick = Time.time + 1;
		}
	}
}
