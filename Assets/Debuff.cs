/*
 * Filename: Debuff.cs
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
/// The Debuff interface specifies required methods for any Buff or Debuff that results
/// from a spell. Originally, this interface was designed only for Debuffs, but as we began
/// to further develop the game, we found that our existing Debuff architecture was also efficient
/// for Player buffs. In very near future iterations, we will be renaming this interface to include
/// both Buffs and Debuffs, and possibly be adding sub-abstract classes that indicate if a status
/// effect is a Buff or Debuff.
/// </summary>
public interface Debuff {
	
	/// <summary>
	/// Determines if the Debuff has expiored.
	/// </summary>
	/// <returns>
	/// If the Debuff is expired and should be deleted.
	/// </returns>
	bool hasExpired();
	
	/// <summary>
	/// Applies this debuffs affect upon the specified damage being done.
	/// Example: Garen's Might of Demacia Debuff increases damage by 10%.
	/// </summary>
	/// <returns>
	/// The newly calculated damage to be applied.
	/// </returns>
	/// <param name='damage'>
	/// The damage before this Debuff has been applied.
	/// </param>
	float applyDebuff(float damage);
	
	/// <summary>
	/// Applies the specified number of stacks to this Debuff.
	/// </summary>
	/// <param name='numAdditionalStacks'>
	/// Number additional stacks to apply.
	/// </param>
	void applyStack(int numAdditionalStacks);
	
	/// <summary>
	/// Gets the texture of this Debuff, to be used in the GUI draw methods.
	/// </summary>
	/// <returns>
	/// The Debuff's texture.
	/// </returns>
	Texture2D getTexture();
	
	/// <summary>
	/// Called when the Debuff is first applied to the Player. In future iterations, this needs to be
	/// expanded to be applied to either Players or Minions.
	/// </summary>
	/// <param name='player'>
	/// The player to receive the Debuff's initial effects.
	/// </param>
	void apply(PlayerScript player);
	
	/// <summary>
	/// Called when the Debuff has expired, negating whatever effects were initially applied to the Player.
	/// </summary>
	/// <param name='player'>
	/// The Player who should no longer be afflicted by the Debuff.
	/// </param>
	void expire(PlayerScript player);
	
	/// <summary>
	/// Determines if the Debuff can be stacked.
	/// </summary>
	bool stackable();
	
	/// <summary>
	/// Determines if the Debuff can be refreshed.
	/// </summary>
	bool prolongable();
	
	/// <summary>
	/// Refresh this duration of this Debuff.
	/// </summary>
	void refresh();
	
	/// <summary>
	/// The string descriptions of this Debuff.
	/// </summary>
	string description();
	
	/// <summary>
	/// The string name of this Debuff.
	/// </summary>
	string name();
	
	/// <summary>
	/// Updates the Debuff during the game loop.
	/// </summary>
	void update();
}
