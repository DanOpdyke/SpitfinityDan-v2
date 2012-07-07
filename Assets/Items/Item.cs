/*
 * Filename: Item.cs
 * 
 * Author:
 * 		Programming: Daniel Opdyke
 * 
 * Last Modified: 7/6/2012
 * 
 * NOTE: All Models, Original Character Concepts, and Icons are property of Riot Games.
 * */
using UnityEngine;
using System.Collections;

/// <summary>
/// The Item interface specifies required methods for Items which may be placed in a Player's inventory.
/// </summary>
public interface Item {
	
	/// <summary>
	/// Gets the Item's texture.
	/// </summary>
	/// <returns>
	/// The texture.
	/// </returns>
	Texture2D getTexture();
	
	/// <summary>
	/// Simulates equipping the specified Item on the Player, if applicable.
	/// </summary>
	/// <param name='player'>
	/// Player to equip the item on.
	/// </param>
	void Equip(PlayerScript player);
	
	/// <summary>
	/// Gets the string representation of the Item's stats.
	/// </summary>
	/// <returns>
	/// The Item's stats
	/// </returns>
	string getStats();
	
	/// <summary>
	/// Gets the Item's rarity.
	/// </summary>
	/// <returns>
	/// The Item's rarity.
	/// </returns>
	int getItemRarity();
	
	/// <summary>
	/// Gets a shallow copy of the Item.
	/// </summary>
	/// <returns>
	/// Shallow copy of the Item.
	/// </returns>
	Item getCopy();
	
	/// <summary>
	/// Randomize the Item using the specified level.
	/// </summary>
	/// <param name='level'>
	/// Level of the newly randomized Item.
	/// </param>
	void randomize(int level);
	
	/// <summary>
	/// Returns the string neccessary to save this Item.
	/// </summary>
	string Save();
}
