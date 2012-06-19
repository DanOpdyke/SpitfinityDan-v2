using UnityEngine;
using System.Collections;

public class Boots : MonoBehaviour, Item, Armor {

	private Texture2D texture;
	private int numTextures = 8;
	private int itemRarity;
	
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
	#endregion

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void Equip(){
		
	}
	
	public void randomizeArmor(int level){
		float rarityPercent = Random.value;
		
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
		
		float effectiveWeaponLevel = Random.value * 2 * level * itemRarity;
		
		//Allow each item to have up to three stats on it
		for(int i = 0; i < 3; i++)
		{
			int amountToIncrease = (int) Random.Range(1+effectiveWeaponLevel, 10 + effectiveWeaponLevel);
			increaseStat(Random.Range(0, 4), amountToIncrease);
		}
		
		name = "Generic Boots";
		texture = Resources.Load("BootTextures/texture" + Random.Range(0, numTextures)) as Texture2D;
		
	}
	
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
		
		return toReturn;
	}
	
	public Texture2D getTexture(){
		return texture;
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
