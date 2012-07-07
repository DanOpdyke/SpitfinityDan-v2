/*
 * Filename: Weapon.cs
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
/// The Weapon abstract class provides basic funcationality for equippable stat increasing items.
/// Items are randomly generated upon minion death.
/// </summary>
public abstract class Weapon : MonoBehaviour, Item {
	
	/// <summary>
	/// The texture of the Weapon peice.
	/// </summary>
	protected Texture2D texture;
	
	/// <summary>
	/// The file file of the Weapon texture;
	/// </summary>
	protected string texturePath;
	
	/// <summary>
	/// The rarity of the Weapon, as determined upon randomization.
	/// </summary>
	protected int itemRarity;
	
	/// <summary>
	/// The number of possible textures a piece of Weapon can have.
	/// </summary>
	protected int numTextures;
	
	/// <summary>
	/// The flavor text of the Weapon, showed on the bottom of the tooltip.
	/// Currently, only legendary Weapons have flavor text.
	/// </summary>
	protected string flavorText;
	
	/// <summary>
	/// String array containing possible prefixes to Weapon names.
	/// </summary>
	protected string[] prefixes = { "Rusty", "Dull", "Iron", "Steel", "Engineered", "Heavy"};
	
	/// <summary>
	/// String array containing possible prefixes to Weapon names.
	/// </summary>
	protected string[] suffixes = {"Power", "Quickness", "Wisdom", "Fortitude"};
	
	/*
	 * Legendary Names (Currently Implemented):
	 * 2H Sword: Infinity Edge, Trinity Force, Youmuu's Ghostblade, Guinsoo's Rageblade
	 * Phreak's "IPlayThisChampionAsAJungler" Sword
	 * Bows: Last Whisper, Ionic Spark, Hextech Gunblade
	 * 
	 * Fake Legendaries (Not Currently Implemented):
	 * Finity Edge
	 * Guinsoo's Hugblade
	 * Ionic Sparkle
	 * */
	
	#region Weapon Stats
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
	
	private float weaponDamage;
	
	private float weaponSpeed;

	public string Name {
		get {
			return this.name;
		}
		set {
			name = value;
		}
	}

	public float WeaponDamage {
		get {
			return this.weaponDamage;
		}
		set {
			weaponDamage = value;
		}
	}

	public float WeaponSpeed {
		get {
			return this.weaponSpeed;
		}
		set {
			weaponSpeed = value;
		}
	}
	
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

	private string name;
	#endregion
	
	/// <summary>
	/// Gets the string representation of the Weapon's stats, used for displaying in tooltips.
	/// </summary>
	/// <returns>
	/// String representation of Weapon stats.
	/// </returns>
	public string getStats(){
		
		string toReturn = name + "\n";
		if(strength > 0)
			toReturn += "Strength: " + strength + "\n";
		if(dexterity > 0)
			toReturn += "Dexterity: " + dexterity + "\n";
		if(intelligence > 0)
			toReturn += "Intelligence: " + intelligence + "\n";
		if(vitality > 0)
			toReturn += "Vitality: " + vitality + "\n";
		
		toReturn += "Weapon Damage: " + weaponDamage.ToString("0.00") + "\n" +
			"Weapon Speed: " + weaponSpeed.ToString("0.00") + "\n";
		
		if(flavorText != null)
			toReturn += flavorText + "\n";
		
		return toReturn;
			
	}
	
	/// <summary>
	/// Modifies the Player's stats based on the weapon's stats. Originally, this was done in the Player class,
	/// but allowing Weapons to apply their own stats increased the possible effects a Weapon could have.
	/// </summary>
	/// <param name='player'>
	/// The Player who receives the stat increases.
	/// </param>
	public void Equip(PlayerScript player){
		player.Strength += this.Strength;
		player.Dexterity += this.Dexterity;
		player.Intelligence += this.Intelligence;
		player.Vitality += this.Vitality;
		player.WeaponDamage += this.WeaponDamage;
		
		//This is a temporary fix - If the Player was previously attacking without an equipped weapon, make their new attack speed equal
		//to this weapon's attack speed. In future iterations, we must make this a more accurate check, to avoid issues with 1.5 speed weapons.
		if(player.WeaponSpeed == 1.5f)
			player.WeaponSpeed = this.weaponSpeed;
		else
			player.WeaponSpeed += this.WeaponSpeed;
	}

	/// <summary>
	/// Gets the item rarity, which corresponds to the relative value of the Weapon.
	/// </summary>
	/// <returns>
	/// The item rarity.
	/// </returns>
	public int getItemRarity(){
		return itemRarity;
	}
	
	/// <summary>
	/// Gets a copy of the Weapon. Implemented by sub-classes.
	/// </summary>
	/// <returns>
	/// A shallow copy of the Weapon.
	/// </returns>
	public Item getCopy(){
		return null;
	}
	
	/// <summary>
	/// Randomize the Weapon using specified level.
	/// </summary>
	/// <param name='level'>
	/// Level of the newly randomized Weapon.
	/// </param>
	public void randomize(int level){
		this.randomizeWeapon(level);	
	}
	
	/// <summary>
	/// Randomizes the Weapon using the specified level.
	/// </summary>
	/// <param name='level'>
	/// Level of the newly randomized Weapon.
	/// </param>
	/// <param name='forcedRartiy'>
	/// Optional parameter to specify an Weapon's rarity, instead of
	/// randomizing it.
	/// </param>
	public void randomizeWeapon(int level, int forcedRarity = -1){
		float rarityPercent = Random.value;
		
		if(forcedRarity > -1)
			itemRarity = forcedRarity;
		else{
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
				
			if(itemRarity == 5){
				makeLegendary();
				return;
			}
		}
		
		
		float effectiveWeaponLevel = Random.value * level * Mathf.Pow(2, itemRarity);
		
		//Allow each item to have up to three stats on it
		for(int i = 0; i < 3; i++){
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
		
		float weaponSpeedRange = 0.9f;
		weaponSpeed = Random.value * weaponSpeedRange + 1.8f;
		
		float weaponDamageRange = 20f;
		weaponDamage = Random.value * weaponDamageRange + (effectiveWeaponLevel * 7);
		
		Name = prefixes[itemRarity] + " " + Name + " of " + suffixes[highestStatIndex];
		this.texturePath += Random.Range(0, numTextures);
		texture = Resources.Load(texturePath) as Texture2D;
	}
	
	/// <summary>
	/// Makes this Weapon legendary, converting it to a predefined Item.
	/// </summary>
	public abstract void makeLegendary();
	
	/// <summary>
	/// Increases the specified stat by a specified amount for the Weapon.
	/// </summary>
	/// <param name='statNum'>
	/// The index to the state to be increased.
	/// </param>
	/// <param name='amount'>
	/// The amount to increase the specified stat.
	/// </param>
	private void increaseStat(int statNum, int amount){
		switch(statNum){
		case 0:
			this.strength += amount;
			goto default;
		case 1:
			this.dexterity += amount;
			goto default;
		case 2:
			this.intelligence += amount;
			goto default;
		case 3:
			this.vitality += amount;
			goto default;
		default:
			break;
		}
	}
	
	/// <summary>
	/// Gets the Weapon's texture.
	/// </summary>
	/// <returns>
	/// The Weapon's texture.
	/// </returns>
	public Texture2D getTexture(){
		return texture;
	}
	
	//<summary>
	//Saves item in standard format.
	//Type, Name, Rarity, Str, Dex, Int, Vit, Dmg, Speed, TexturePath
	//</summary>
	public string Save(){
		string toReturn = "Weapon, " + this.GetType().ToString();
		string Conn = ",";
		toReturn += Conn + this.name;
		toReturn += Conn + this.itemRarity;
		toReturn += Conn + this.strength;
		toReturn += Conn + this.dexterity;
		toReturn += Conn + this.intelligence;
		toReturn += Conn + this.vitality;
		toReturn += Conn + this.weaponDamage;
		toReturn += Conn + this.weaponSpeed;
		toReturn += Conn + this.texturePath;
		toReturn += Conn + this.flavorText;
		return toReturn;
	}
	
	
	//<summary>
	//Loads item assuming string in standard format.
	//Type, Name, Rarity, Str, Dex, Int, Vit, Dmg, Speed, TexturePath
	//</summary>
	public static Weapon Load(string savedData){
		string[] data = savedData.Split(',');
		Weapon weapon = (Weapon) System.Activator.CreateInstance(null, data[1]).Unwrap();
		weapon.Name = data[2];
		weapon.ItemRarity = int.Parse(data[3]);
		weapon.Strength = int.Parse(data[4]);
		weapon.Dexterity = int.Parse(data[5]);
		weapon.Intelligence = int.Parse(data[6]);
		weapon.Vitality = int.Parse(data[7]);
		weapon.WeaponDamage = float.Parse(data[8]);
		weapon.WeaponSpeed = float.Parse(data[9]);
		weapon.texturePath = data[10];
		weapon.Texture = (Texture2D) Resources.Load(data[10]);
		weapon.flavorText = data[11];
		return weapon;
	}
}
