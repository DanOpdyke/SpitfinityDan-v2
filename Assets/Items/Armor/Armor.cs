/*
 * Filename: Armor.cs
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
/// The Armor abstract class provides basic funcationality for equippable stat increasing items.
/// Items are randomly generated upon minion death.
/// </summary>
public abstract class Armor : MonoBehaviour, Item {
	
	#region Armor Stats
	private int strength;
	
	public int Strength {
		get
		{	
			return this.strength;
		}
		set
		{
			strength = value;
		}
	}
	
	private int dexterity;

	public int Dexterity {
		get {
			return this.dexterity;
		}
		set {
			dexterity = value;
		}
	}	
	
	private int intelligence;
	
	private int vitality;

	public int Intelligence {
		get {
			return this.intelligence;
		}
		set {
			intelligence = value;
		}
	}

	public int Vitality {
		get {
			return this.vitality;
		}
		set {
			vitality = value;
		}
	}	

	public string Name {
		get {
			return this.name;
		}
		set {
			name = value;
		}
	}

	private string name;

	public int ItemRarity {
		get {
			return this.itemRarity;
		}
		set {
			itemRarity = value;
		}
	}

	public Texture2D Texture {
		get {
			return this.texture;
		}
		set {
			texture = value;
		}
	}
	#endregion
	
	/// <summary>
	/// The texture of the Armor peice.
	/// </summary>
	protected Texture2D texture;
	
	/// <summary>
	/// The file file of the Armor texture;
	/// </summary>
	protected string texturePath;
	
	/// <summary>
	/// The rarity of the Armor, as determined upon randomization.
	/// </summary>
	protected int itemRarity;
	
	/// <summary>
	/// The number of possible textures a piece of Armor can have.
	/// </summary>
	protected int numTextures;
	
	/// <summary>
	/// String array containing possible prefixes to Armor names.
	/// </summary>
	protected string[] prefixes = { "Torn", "Worn", "Woven", "Sturdy", "Reinforced", "Heavy"};
	
	/// <summary>
	/// String array containing possible prefixes to Armor names.
	/// </summary>
	protected string[] suffixes = {"Power", "Quickness", "Wisdom", "Fortitude"};
	
	
	/*
	 * Legendaries Ideas (Not currently Implemented):
	 * Warmogs, Sunfire Cape, Randuin's Omen, Rabadon's Deathcap
	 * 
	 * Fake Legendaries Ideas (Not currently Implemented):
	 * Madred's BeardShaver
	 * */
	
	/// <summary>
	/// Gets the Armor's texture.
	/// </summary>
	/// <returns>
	/// The Armor's texture.
	/// </returns>
	public Texture2D getTexture(){
		return texture;
	}
	
	/// <summary>
	/// Simulates equipping the Armor piece on the Player by appropriately modifying the
	/// Player's stats.
	/// </summary>
	/// <param name='player'>
	/// Player to receive the stat increases.
	/// </param>
	public void Equip(PlayerScript player){
		player.Strength += this.Strength;
		player.Dexterity += this.Dexterity;
		player.Intelligence += this.Intelligence;
		player.Vitality += this.Vitality;
	}
	
	/// <summary>
	/// Gets the Armor's rarity.
	/// </summary>
	/// <returns>
	/// The Armor's rarity.
	/// </returns>
	public int getItemRarity(){
		return itemRarity;
	}
	
	/// <summary>
	/// Gets the string representation of the Armor's stats.
	/// </summary>
	/// <returns>
	/// Armor's stat representation.
	/// </returns>
	public string getStats(){
		string toReturn = Name + "\n";
		if(Strength > 0)
			toReturn += "Strength: " + Strength + "\n";
		if(Dexterity > 0)
			toReturn += "Dexterity: " + Dexterity + "\n";
		if(Intelligence > 0)
			toReturn += "Intelligence: " + Intelligence + "\n";
		if(Vitality > 0)
			toReturn += "Vitality: " + Vitality + "\n";
		
		return toReturn;
	}
	
	/// <summary>
	/// Gets a copy of the Armor piece. Implemented by sub-classes.
	/// </summary>
	/// <returns>
	/// A shallow copy of the Armor piece.
	/// </returns>
	public Item getCopy(){
		return null;
	}
	
	/// <summary>
	/// Randomize the Armor piece using specified level.
	/// </summary>
	/// <param name='level'>
	/// Level of the newly randomized Armor piece.
	/// </param>
	public void randomize(int level){
		this.randomizeArmor(level);	
	}
	
	/// <summary>
	/// Randomizes the Armor peices using the specified level.
	/// </summary>
	/// <param name='level'>
	/// Level of the newly randomized Armor piece.
	/// </param>
	/// <param name='forcedRartiy'>
	/// Optional parameter to specify an Armor's rarity, instead of
	/// randomizing it.
	/// </param>
	public void randomizeArmor(int level, int forcedRartiy = -1){
		float rarityPercent = Random.value;
		if(forcedRartiy > -1)
			itemRarity = forcedRartiy;
		else{
		//Fortitute, Quickness, Wisdom, Power
			if(rarityPercent <= 0.3f)
				itemRarity = 0;
			else if(rarityPercent <= 0.6f)
				itemRarity = 1;
			else if(rarityPercent <= 0.80f)
				itemRarity = 2;
			else if(rarityPercent <= 0.90f)
				itemRarity = 3;
			else if(rarityPercent <= 0.96f)
				itemRarity = 4;
			else
				itemRarity = 5;
		}
		
		float effectiveWeaponLevel = Random.value * level * Mathf.Pow(2, itemRarity);
		
		//Allow each item to have up to three stats on it
		for(int i = 0; i < 3; i++)
		{
			int amountToIncrease = (int) Random.Range(1+effectiveWeaponLevel, 10 + effectiveWeaponLevel);
			increaseStat(Random.Range(0, 4), amountToIncrease);
		}
		int highestStatIndex = 0;
		int highestStat = strength;
		if(dexterity > highestStat){
			highestStat = dexterity;
			highestStatIndex = 1;
		}
		if(intelligence > highestStat){
			highestStat = intelligence;
			highestStatIndex = 2;
		}
		if(vitality > highestStat){
			highestStat = dexterity;
			highestStatIndex = 3;
		}
		
		Name = prefixes[itemRarity] + " " + Name + " of " + suffixes[highestStatIndex];
		texturePath += Random.Range(0, numTextures);
		texture = Resources.Load(texturePath) as Texture2D;
		
		
	}
	
	/// <summary>
	/// Increases the specified stat by a specified amount for the Armor piece.
	/// </summary>
	/// <param name='statNum'>
	/// The index to the state to be increased.
	/// </param>
	/// <param name='amount'>
	/// The amount to increase the specified stat.
	/// </param>
	protected void increaseStat(int statNum, int amount){
		switch(statNum){
		case 0:
			this.Strength += amount;
			goto default;
		case 1:
			this.Dexterity += amount;
			goto default;
		case 2:
			this.Intelligence += amount;
			goto default;
		case 3:
			this.Vitality += amount;
			goto default;
		default:
			break;
		}
	}
	
	//<summary>
	//Saves item in standard format.
	//Type, Name, Rarity, Str, Dex, Int, Vit, TexturePath
	//</summary>
	public string Save(){
		string toReturn = "Armor, " + this.GetType().ToString();
		string Conn = ",";
		toReturn += Conn + this.name;
		toReturn += Conn + this.itemRarity;
		toReturn += Conn + this.strength;
		toReturn += Conn + this.dexterity;
		toReturn += Conn + this.intelligence;
		toReturn += Conn + this.vitality;
		toReturn += Conn + this.texturePath;
		return toReturn;
	}
	
	//<summary>
	//Loads item assuming string in standard format.
	//Type, Name, Rarity, Str, Dex, Int, Vit, TexturePath
	//</summary>
	public static Armor Load(string savedData){
		string[] data = savedData.Split(',');
		Armor armor = (Armor) System.Activator.CreateInstance(null, data[1]).Unwrap();
		armor.Name = data[2];
		armor.ItemRarity = int.Parse(data[3]);
		armor.Strength = int.Parse(data[4]);
		armor.Dexterity = int.Parse(data[5]);
		armor.Intelligence = int.Parse(data[6]);
		armor.Vitality = int.Parse(data[7]);
		armor.texturePath = data[8];
		armor.Texture = (Texture2D) Resources.Load(data[8]);
		return armor;
	}
}
