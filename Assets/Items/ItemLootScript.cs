/*
 * Filename: ItemLootScript.cs
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
/// The Item Loot script simulates the dropping, randomizing, and retrieving of loot upon minion death.
/// NOTE: Currently, legendary loot is predetermined, and will NOT scale with the Player's level.
/// This should be fixed in future iterations.
/// </summary>
public class ItemLootScript : MonoBehaviour {
	
	/// <summary>
	/// The Item that will be contained within this loot oject.
	/// </summary>
	private Item item;
	
	/// <summary>
	/// The level of the Item, used to determine the magnitude of the stats.
	/// </summary>
	private int level = 1;
	
	/// <summary>
	/// The color of the loot object's smoke, as determined by the Item rarity.
	/// </summary>
	private Color[] itemRarityColors = {
		Color.gray, Color.white, Color.green, new Color(0.01f, 0.80f, 0.85f, 1), Color.yellow, Color.red	
	};
	
	/// <summary>
	/// Array containing possible Items to instantiate when randomizing.
	/// </summary>
	private static Item[] items = {
		new Bow(),
		new TwoHandedSword(),
		new Helm(),
		new Chest(),
		new Boots(),
		new Gloves()
	};
	
	/// <summary>
	/// Sets the level of this Item.
	/// </summary>
	/// <param name='level'>
	/// New Item level.
	/// </param>
	public void setLevel(int level){
		this.level = level;
	}

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	/// <summary>
	/// Sets the Item contained within this loot object.
	/// </summary>
	/// <param name='item'>
	/// Loot object's new Item.
	/// </param>
	public void setItem(Item item){
		this.item = item;
		ParticleSystem particleSystem = gameObject.GetComponent(typeof(ParticleSystem)) as ParticleSystem;
		particleSystem.startColor = itemRarityColors[item.getItemRarity()];
	}
	
	/// <summary>
	/// Gets the Item contained within this loot object
	/// </summary>
	/// <returns>
	/// Loot objects Item
	/// </returns>
	public Item getItem(){
		return this.item;
	}
	
	/// <summary>
	/// Randomizes the Item contained within this Loot object
	/// </summary>
	public void randomizeItem(){
		this.item = items[Random.Range(0, items.Length)].getCopy();
		this.item.randomize(level);
		
		ParticleSystem particleSystem = gameObject.GetComponent(typeof(ParticleSystem)) as ParticleSystem;
		particleSystem.startColor = itemRarityColors[item.getItemRarity()];
	}
}
