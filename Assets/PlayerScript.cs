/*
 * Filename: PlayerScript.cs
 * 
 * Author:
 * 		Programming: Daniel Opdyke
 * 
 * Last Modified: 7/6/2012
 * 
 * */
using UnityEngine;
using System.Collections;

/// <summary>
/// The PlayerScript interface specifies required methods for character classes. In future iterations,
/// this interface will be changed to an abstract class, to remove a large amount of redundant code.
/// </summary>
public interface PlayerScript {
	
	#region Player Stats
	int Strength
	{
		get;
		set;
	}
	
	int Dexterity
	{
		get;
		set;
	}
	
	int Intelligence
	{
		get;
		set;
	}
	
	int Vitality
	{
		get;
		set;
	}
	
	float WeaponDamage
	{
		get;
		set;
	}
	
	float WeaponSpeed
	{
		get;
		set;
	}
	
	float MovementSpeed
	{
		get;
		set;
	}
	
	int Level
	{
		get;
		set;
	}
	#endregion
	
	/// <summary>
	/// Begins a randomly chosen idle animation.
	/// </summary>
	void playIdleSequence();
	
	/// <summary>
	/// Determines if the Player's animator is currently playing the run animation.
	/// </summary>
	/// <returns>
	/// If the run animation is playing.
	/// </returns>
	bool isRunAnimation();
	
	/// <summary>
	/// If the Player's animator is currently playing.
	/// </summary>
	/// <returns>
	/// If an animation is playing.
	/// </returns>
	bool noAnimation();
	
	/// <summary>
	/// Determines if the Player is currently alive.
	/// </summary>
	/// <returns>
	/// If the Player is alive.
	/// </returns>
	bool isAlive();
	
	/// <summary>
	/// Stops the Player's animator.
	/// </summary>
	void stopAnimation();
	
	/// <summary>
	/// Gets the current health percent of the Player.
	/// </summary>
	/// <returns>
	/// Player's health percentage.
	/// </returns>
	float getHealthPercent();
	
	/// <summary>
	/// Sets the Player's current enemy.
	/// </summary>
	/// <param name='enemy'>
	/// New Enemy.
	/// </param>
	void setCurrentEnemy(EnemyScript enemy);
	
	/// <summary>
	/// Sets the idling state of the Player, used for determining when idling animations
	/// should begin.
	/// </summary>
	/// <param name='idle'>
	/// If the player should now be idling.
	/// </param>
	void setIdling(bool idle);
	
	/// <summary>
	/// Gets the Player's current enemy.
	/// </summary>
	/// <returns>
	/// The current Player's enemy.
	/// </returns>
	EnemyScript getCurrentEnemy();
	
	/// <summary>
	/// Gets the Player's weapon damage, with primary stat effects already
	/// included.
	/// </summary>
	/// <returns>
	/// The Player's current weapon damage.
	/// </returns>
	float getWeaponDamage();
	
	/// <summary>
	/// Gets the Player's weapon speed, with additional buff and debuff effects
	/// already applied.
	/// </summary>
	/// <returns>
	/// The Player's current weapon speed.
	/// </returns>
	float getWeaponSpeed();
	
	/// <summary>
	/// Plays the specified animation, if exists.
	/// </summary>
	/// <param name='animationName'>
	/// The name of the animation to play.
	/// </param>
	void playAnimation(string animationName);
	
	/// <summary>
	/// Sets Player's running state, used to determine what actions are possible, and what
	/// animations should be played.
	/// </summary>
	/// <param name='active'>
	/// The Player's new running state.
	/// </param>
	void setRunning(bool active);
	
	/// <summary>
	/// Gets the Player's attack range.
	/// </summary>
	/// <returns>
	/// Player's attack range.
	/// </returns>
	float getRange();
	
	/// <summary>
	/// Awards the Player additional health, up to a maximum health value.
	/// </summary>
	/// <param name='amount'>
	/// Amount of additional Player health.
	/// </param>
	void awardHealth(float amount);
	
	/// <summary>
	/// Damage the Player by specified amount, before mitigation is taken into consideration.
	/// </summary>
	/// <param name='amount'>
	/// Amount of pre-mitigation damage to the Player.
	/// </param>
	void damage(float amount);
	
	/// <summary>
	/// Performs a simple auto-attack on the specified player. The calling procedure is responsible
	/// to ensure that the auto-attack is only called when off cooldown.
	/// </summary>
	/// <returns>
	/// The new cooldown of the auto-attack.
	/// </returns>
	/// <param name='enemy'>
	/// The enemy to attack.
	/// </param>
	float autoAttack(EnemyScript enemy);
	
	/// <summary>
	/// Gets the GameObject to which the Player is attached.
	/// </summary>
	/// <returns>
	/// Player's GameObject.
	/// </returns>
	GameObject getGameObject();
	
	/// <summary>
	/// Determines if a specific button click is within the bounds of the equipment or
	/// inventory windows. 
	/// </summary>
	/// <returns>
	/// If the mouse position is within a GUI window.
	/// </returns>
	/// <param name='mousePosition'>
	/// Current position of the mouse.
	/// </param>
	bool guiInteraction(Vector3 mousePosition);
	
	/// <summary>
	/// Places the specified item into the Player's inventory, if it is not already full.
	/// The caller is responsible for handling the case of a full inventory before awarding
	/// an item.
	/// </summary>
	/// <param name='item'>
	/// Item.
	/// </param>
	void awardItem(Item item);
	
	/// <summary>
	/// Specifies if the Player is currently in a stunned state. Stunned players are unable to
	/// move, attack, or loot. A Player may be stunned by enemy spells, or due to having recently
	/// performed an attack.
	/// </summary>
	bool stunned();
	
	/// <summary>
	/// Stun the Player for the specified time.
	/// </summary>
	/// <param name='time'>
	/// Time for Player to remain stunned.
	/// </param>
	void Stun(float time);
	
	/// <summary>
	/// Sets the Player's poisoned state.
	/// </summary>
	/// <param name='poisoned'>
	/// Player's new poisoned state.
	/// </param>
	void setPoisoned(bool poisoned);
	
	/// <summary>
	/// Determines if the Player is currently poisoned.
	/// </summary>
	/// <returns>
	/// Player's poisoned state.
	/// </returns>
	bool isPoisoned();
	
	/// <summary>
	/// Applies the given debuff. If the debuff is already on the Player, and the debuff is allowed to be refreshed, its
	/// duration is reset.
	/// </summary>
	/// <param name='debuff'>
	/// The debuff to apply.
	/// </param>
	void applyDebuff(Debuff debuff);
	
	/// <summary>
	/// Save the inventory, equipment, and state data of the Player.
	/// </summary>
	void Save();
	
	/// <summary>
	/// Sets the game state to "won".
	/// </summary>
	/// <param name='win'>
	/// If the game has been won.
	/// </param>
	void setWin(bool win);
}
