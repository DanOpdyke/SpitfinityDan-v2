/*
 * Filename: Boots.cs
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
/// Simulates a Boot Armor piece.
/// </summary>
public class Boots : Armor, Item {
	
	/// <summary>
	/// Initializes a new instance of the <see cref="Boots"/> class.
	/// </summary>
	public Boots() {
		this.Name = "Boots";
		this.texturePath = "BootTextures/texture";
		this.numTextures = 8;
	}
	
	public void Equip(){
		
	}
		
	public int getItemRarity(){
		return this.itemRarity;
	}
	
	public Item getCopy(){
		return new Boots();
	}
	
	public void randomize(int level){
		this.randomizeArmor(level);	
	}

}
