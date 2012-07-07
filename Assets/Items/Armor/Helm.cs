/*
 * Filename: Helm.cs
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
public class Helm : Armor, Item {
	
	/// <summary>
	/// Initializes a new instance of the <see cref="Helm"/> class.
	/// </summary>
	public Helm() {
		this.Name = "Helm";
		this.texturePath = "HelmTextures/texture";
		this.numTextures = 4;
	}

	public void Equip(){
		
	}
	
	public int getItemRarity(){
		return this.itemRarity;
	}
	
	public Item getCopy(){
		return new Helm();
	}
	
	public void randomize(int level){
		this.randomizeArmor(level);	
	}
}
