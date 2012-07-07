/*
 * Filename: Chest.cs
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
/// Simulates a Chest Armor piece.
/// </summary>
public class Chest : Armor, Item {
	
	public void Equip(){
		
	}
	
	/// <summary>
	/// Initializes a new instance of the <see cref="Chest"/> class.
	/// </summary>
	public Chest() {
		this.Name = "Chest";
		this.texturePath = "ChestTextures/texture";
		this.numTextures = 10;
	}
		
	public int getItemRarity(){
		return this.itemRarity;
	}
	
	public Item getCopy(){
		return new Chest();
	}
	
	public void randomize(int level){
		this.randomizeArmor(level);	
	}
}
