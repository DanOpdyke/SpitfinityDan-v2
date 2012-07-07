/*
 * Filename: Bow.cs
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
/// Simulates a Bow Weapon.
/// </summary>
public class Bow : Weapon, Item {
	
	/// <summary>
	/// Initializes a new instance of the <see cref="Bow"/> class.
	/// </summary>
	public Bow(){
		numTextures = 7;
		texturePath = "BowTextures/texture";
		Name = "Crossbow";
	}
	
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void Equip(){
		
	}
	
	public int getItemRarity(){
		return this.itemRarity;
	}
	
	public Item getCopy(){
		return new Bow();
	}
	
	public void randomize(int level){
		this.randomizeWeapon(level);	
	}
	
	public override void makeLegendary(){
		string[] legendarySwords = {"Last Whisper", "Ionic Spark", "Hextech Gunblade"};
		
		numTextures = 3;
		
		int index = Random.Range (0,3);
		Name = legendarySwords[index];
		texturePath = "LegendaryBowTextures/texture";
		
		if(index == 0){
			Strength = 40;
			Dexterity = 60;
			Intelligence = 0;
			Vitality = 10;
			WeaponDamage = 60f;
			WeaponSpeed = 2.2f;
			texturePath = "LegendaryBowTextures/LastWhisper";
			Texture = Resources.Load(texturePath) as Texture2D;
			flavorText = "\"You may say your last \n words, with a whisper.\""; 
		}
		else if(index == 1){
			Strength = 20;
			Dexterity = 40;
			Intelligence = 0;
			Vitality = 50;
			WeaponDamage = 40f;
			WeaponSpeed = 2.6f;
			texturePath = "LegendaryBowTextures/IonicSpark";
			Texture = Resources.Load(texturePath) as Texture2D;
			flavorText = "\"Let's just pretend this \n does chain lightning.\"";
		}
		else{
			Strength = 20;
			Dexterity = 50;
			Intelligence = 50;
			Vitality = 10;
			WeaponDamage = 70f;
			WeaponSpeed = 2.2f;
			texturePath = "LegendaryBowTextures/HextechGunblade";
			Texture = Resources.Load (texturePath) as Texture2D;
			flavorText = "\"Useful for stabbing AND shooting... \nbut mostly shooting.\"";
		}
	}

}
