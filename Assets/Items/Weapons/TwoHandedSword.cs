/*
 * Filename: TwoHandedSword.cs
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
/// Simulates a Two-Handed Sword Weapon.
/// </summary>
public class TwoHandedSword : Weapon, Item {
	
	/// <summary>
	/// Initializes a new instance of the <see cref="TwoHandedSword"/> class.
	/// </summary>
	public TwoHandedSword(){
		numTextures = 6;
		Name = "Two-Handed Sword";
		texturePath = "2HSwordTextures/texture";
		flavorText = null;
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
		return new TwoHandedSword();
	}
	
	public void randomize(int level){
		this.randomizeWeapon(level);	
	}
	
	public override void makeLegendary(){
		string[] legendarySwords = {"Trinity Force", "Infinity Edge", "Youmuu's Ghostblade,", "Guinsoo's Rageblade", "Phreak's \"IPlayThisChampionAsAJungler\" Sword"};
		
		numTextures = 5;
		
		int index = Random.Range (0,5);
		Name = legendarySwords[index];
		texturePath = "LegendarySwordTextures/texture";
		
		if(index == 0){
			Strength = 20;
			Dexterity = 20;
			Intelligence = 20;
			Vitality = 20;
			WeaponDamage = 40f;
			WeaponSpeed = 2.2f;
			texturePath = "LegendarySwordTextures/TrinityForce";
			Texture = Resources.Load(texturePath) as Texture2D;
			flavorText = "\"Balance in everything allows for \n Massive Damage.\""; 
		}
		else if(index == 1){
			Strength = 40;
			Dexterity = 10;
			Intelligence = 0;
			Vitality = 10;
			WeaponDamage = 70f;
			WeaponSpeed = 2.0f;
			texturePath = "LegendarySwordTextures/InfinityEdge";
			Texture = Resources.Load(texturePath) as Texture2D;
			flavorText = "\"When the sun is at its peak, the pool \n blends in with the shimmering heat, \n looking as if its waters run off into infinity.\"";
		}
		else if(index == 2){
			Strength = 30;
			Dexterity = 20;
			Intelligence = 0;
			Vitality = 20;
			WeaponDamage = 50f;
			WeaponSpeed = 2.6f;
			texturePath = "LegendarySwordTextures/YoumuusGhostblade";
			Texture = Resources.Load(texturePath) as Texture2D;
			flavorText = "\"It is said that this blade may \n be purely imagined by its \n wielder, as it cannot be seen by anyone else.\"";
		}
		else if(index == 3){
			Strength = 35;
			Dexterity = 0;
			Intelligence = 50;
			Vitality = 10;
			WeaponDamage = 40f;
			WeaponSpeed = 2.4f;
			texturePath = "LegendarySwordTextures/GuinsoosRageblade";
			Texture = Resources.Load (texturePath) as Texture2D;
			flavorText = "\"Crumble, before my wrath\n ...and nerfs to your favorite champ.\"";
		}
		else{
			Strength = 50;
			Dexterity = 50;
			Intelligence = 50;
			Vitality = 50;
			WeaponDamage = 70f;
			WeaponSpeed = 2.6f;
			texturePath = "LegendarySwordTextures/Phreak";
			Texture = Resources.Load (texturePath) as Texture2D;
			flavorText = "\"Get On My Level \n ...and make more Teemo skins.\"";
		}
	}
}
