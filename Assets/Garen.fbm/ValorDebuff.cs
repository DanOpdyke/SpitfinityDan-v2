/*
 * Filename: ValorDebuff.cs
 * 
 * Author:
 * 		Programming: Daniel Opdyke
 * 
 * Last Modified: 7/6/2012
 * 
 * NOTE: All Models, Original Character Concepts, and Icons are property of Riot Games.
 * */
using UnityEngine;
using System.Collections;

/// <summary>
/// The Valor debuff script mimics the behavior of Garen's Might of Demacia, increasing the damage
/// an enemy takes from any source. In an embarrassing mistake, this debuff script was misnamed, and the
/// incorrect name was used within multiple scripts. In a very, very near future iteration, we will need
/// to rename this script, and correct the mistake in every other applicable script.
/// </summary>
public class ValorDebuff : MonoBehaviour, Debuff {
	private float expirationTime;
	private float duration = 10;
	private int numStacks;
	private int maxStacks = 3;
	
	public Texture2D texture;
	
	public ValorDebuff(Texture2D texture){
		expirationTime = Time.time + duration;
		this.texture = texture;
		numStacks = 1;
	}
	
	// Use this for initialization
	public void Start () {
		
	}
	
	public void refresh(){
		expirationTime = Time.time + duration;
	}
	
	// Update is called once per frame
	public void Update () {
	
	}
	
	public bool hasExpired(){
		return Time.time >= expirationTime;
	}
	
	//Applies an additonal 10% damage for each stack, up to a maximum of three stacks
	public float applyDebuff(float damage){
		return damage * (1.0f + ((float)numStacks * 0.1f));
	}
	
	// Applies additional stacks of the debuff
	public void applyStack(int numAdditionalStacks){
		expirationTime = Time.time + duration;
		numStacks += numAdditionalStacks;
		if(numStacks > maxStacks)
			numStacks = maxStacks;
	}
	
	public Texture2D getTexture(){
		return texture;
	}
	
	public void apply(PlayerScript player){
		;	
	}
	
	public void expire(PlayerScript player){
		;
	}
	
	public bool stackable(){
		return true;
	}
	
	public bool prolongable(){
		return false;
	}
	
	public string description(){
		return "Increases damage taken by " + (numStacks * 10) + "% \n";	
	}
	
	public string name(){
		return "Valor";
	}
	
	public void update(){
		;	
	}
}
