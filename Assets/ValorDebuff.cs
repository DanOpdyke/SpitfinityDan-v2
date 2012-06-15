using UnityEngine;
using System.Collections;

public class ValorDebuff : MonoBehaviour, Debuff {
	private float expirationTime;
	private float duration = 10;
	private int numStacks;
	private int maxStacks = 3;
	
	public Texture2D texture;
	
	
	// Use this for initialization
	public void Start () {
		numStacks = 0;
		expirationTime = Time.time + duration;
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
		Debug.Log("Adjusted damage using number of stacks: " + numStacks);
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
	
	public void setTexture(Texture2D texture){
		this.texture = texture;
	}
}
