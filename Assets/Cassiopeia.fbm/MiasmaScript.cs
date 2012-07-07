/*
 * Filename: MiasmaScript.cs
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
/// The Miasma script mimics the behavior of Cassiopeia's Miasma spell, which, when triggered,
/// applies a slowing poison to the Player, and grows in size. In the current iteration, the size
/// increase is relatively small, due to the difficulty of Cassiopeia in general.
/// </summary>
public class MiasmaScript : MonoBehaviour {
	
	/// <summary>
	/// The time at which the poison will next be refreshed, if applicable.
	/// </summary>
	private float nextTick = Time.time + 4;
	
	/// <summary>
	/// The time at which the Miasma area should be destroyed.
	/// </summary>
	private float lifeTime;
	
	/// <summary>
	/// The current scale of the Miasma zone, modified when a Player triggers the zone.
	/// </summary>
	private float currentScale = 1;
	
	// Use this for initialization
	void Start () {
		lifeTime = Time.time + 10;
	}
	
	// Update is called once per frame
	void Update () {
		if(Time.time > lifeTime)
			Destroy(gameObject);
	}
	
	/// <summary>
	/// Indicates that the Player has triggered this script, causing the poison to be applied.
	/// Maisma will also increase in size by a scale of .2 on each "trigger", and can only be trigged
	/// every 4 seconds.
	/// </summary>
	/// <param name='player'>
	///	The Player who has trigged the poison.
	/// </param>
	public void trigger(PlayerScript player){
		if (Time.time > nextTick){
			MiasmaDebuff debuff = new MiasmaDebuff(player);
			player.applyDebuff(debuff);
			currentScale += .2f;
			
			gameObject.transform.localScale = Vector3.one * currentScale;
			nextTick = Time.time + 1;
		}
	}
}
