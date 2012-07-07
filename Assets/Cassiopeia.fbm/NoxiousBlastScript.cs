/*
 * Filename: NoxiousBlastScript.cs
 * 
 * Author:
 * 		Programming: Daniel Opdyke
 * 
 * Last Modified: 6/22/2012
 * 
 * NOTE: All Models, Original Character Concepts, and Icons are property of Riot Games.
 * */
using UnityEngine;
using System.Collections;

/// <summary>
/// The Noxious Blast script mimics the behavior of Cassiopeia's Noxious Blast spell, which, when triggered,
/// applies a period damage debuff to the Player. In the current iteration, the damage of Noxious Blast's poison
/// does not scale with Cassiopeia's level, due to the difficulty of Cassiopeia in general.
/// </summary>
public class NoxiousBlastScript : MonoBehaviour {
	
	/// <summary>
	/// The time at which the poison will next be refreshed, if applicable.
	/// </summary>
	private float nextTick = Time.time + 4;
	
	/// <summary>
	/// The time at which the Noxious Blast area should be destroyed.
	/// </summary>
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
	
	/// <summary>
	/// Indicates that the Player has triggered this script, causing the poison to be applied.
	/// This poison can only be applied every 4 seconds.
	/// </summary>
	/// <param name='player'>
	///	The Player who has trigged the poison.
	/// </param>
	public void trigger(PlayerScript player){
		//TODO This should apply a debuff that should damage the player, and automatically wear off after a set duration.
		if (Time.time > nextTick){
			NoxiousBlastDebuff debuff = new NoxiousBlastDebuff();
			debuff.refresh();
			player.applyDebuff(debuff);
			nextTick = Time.time + 1;
		}
	}
}
