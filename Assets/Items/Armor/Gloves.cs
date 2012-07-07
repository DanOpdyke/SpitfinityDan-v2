/*
 * Filename: Glove.cs
 * 
 * Author:
 * 		Programming: Daniel Opdyke, David Spitler
 * 
 * Last Modified: 7/6/2012
 * 
 * NOTE: All Models, Original Character Concepts, and Icons are property of Riot Games.
 * */
using UnityEngine;
using System.Collections;

/// <summary>
/// Simulates a Glove Armor piece.
/// </summary>
public class Gloves : Armor, Item {
	
	public void Equip(){
		;
	}
	
	public Gloves() {
		this.Name = "Gloves";
		this.texturePath = "GlovesTextures/texture";
		this.numTextures = 4;
	}
	
	public int getItemRarity(){
		return this.itemRarity;
	}
	
	public Item getCopy(){
		return new Gloves();
	}
	
	public void randomize(int level){
		this.randomizeArmor(level);	
	}
}
