/*
 * Filename: MiasmaDebuff.cs
 * 
 * Author:
 * 		Programming: Daniel Opdyke
 * 
 * Last Modified: 6/22/2012
 * 
 * NOTE: All Models, Original Character Concepts, and Icons are property of Riot Games.
 * */
using UnityEngine;
using System.Collections;

/// <summary>
/// MiasmaDebuff represents the debuff that is applied when hit by Cassiopeia's Miasma ability.
/// Upon being hit, the Player has their movement speed reduced by 30%, and is set to a poisoned
/// state, allowing Cassiopeia to use her Twin Fang ability. After a set duration of time, the Miasma
/// debuff expires. If the Player reenters or remains within the Miasma poison area, the debuff duration
/// is refreshed.
/// </summary>
public class MiasmaDebuff : MonoBehaviour, Debuff {
	
	/// <summary>
	/// The time at which the Miasma debuff will expire. Refreshed by retriggering the poison.
	/// </summary>
	private float expirationTime;
	
	/// <summary>
	/// The total duration of a single application of the Miasma poison.
	/// </summary>
	private float duration = 8;
	
	/// <summary>
	/// The amount of speed reduction a Player suffers while Miasma is active.
	/// </summary>/
	private float slowAmount = 0.30f;
	
	/// <summary>
	/// The original speed of the Player before Miasma is applied. Used to reset the Player
	/// speed after the debuff has expired.
	/// </summary>
	private float originalSpeed;
	
	/// <summary>
	/// Reference to the Player being hit by the poison.
	/// </summary>
	private PlayerScript player;
	
	/// <summary>
	/// The debuff texture to be displayed above the Player's health bar.
	/// </summary>
	private Texture2D texture;
		
	/// <summary>
	/// Initializes a new instance of the <see cref="MiasmaDebuff"/> class.
	/// </summary>
	/// <param name='player'>
	/// The Player hit by the Miasma poison.
	/// </param>
	public MiasmaDebuff(PlayerScript player){
		this.player = player;
		expirationTime = Time.time + duration;
		texture = Resources.Load("Debuffs/Miasma") as Texture2D;
	}
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	#region Debuff implementation
	/// <summary>
	/// Determines if Miasma has expired, and requires deletion.
	/// </summary>
	/// <returns>
	/// If Miasma has expired.
	/// </returns>
	public bool hasExpired ()
	{
		return Time.time > expirationTime;
	}
	
	/// <summary>
	/// Applies the debuff to the damage currently being taken by the Player. Miasma does not affect
	/// incoming damage.
	/// </summary>
	/// <returns>
	/// The newly calculated damage after the debuff is applied. 
	/// </returns>
	/// <param name='damage'>
	/// The damage before the debuff is applied.
	/// </param>
	public float applyDebuff (float damage)
	{
		return damage;
	}

	/// <summary>
	/// Applies a stack of the debuff, if possible. Debuff stacks increase the effect of the debuff.
	/// </summary>
	/// <param name='numAdditionalStacks'>
	/// Number additional stacks to apply.
	/// </param>
	public void applyStack (int numAdditionalStacks)
	{
		;
	}

	/// <summary>
	/// Gets the debuff texture.
	/// </summary>
	/// <returns>
	/// The debuff texture.
	/// </returns>
	public Texture2D getTexture ()
	{
		return texture;
	}
	
	/// <summary>
	/// Called when the Miasma debuff is first applied to the character, decreasing movement speed by
	/// a set percentage.
	/// </summary>
	/// <param name='player'>
	/// The Player hit by Miasma.
	/// </param>
	public void apply (PlayerScript player)
	{
		originalSpeed = player.MovementSpeed;
		player.MovementSpeed = player.MovementSpeed * (1 - slowAmount);
		player.setPoisoned(true);
	}
	
	/// <summary>
	/// Called when the Miasma buff has expired, restoring the original Player movement speed.
	/// </summary>
	/// <param name='player'>
	/// The Player currently afflicted by Miasma.
	/// </param>
	public void expire (PlayerScript player)
	{
		player.MovementSpeed = originalSpeed;
		player.setPoisoned(false);
	}
	
	/// <summary>
	/// Determines if this debuff can be stacked for increased effect.
	/// </summary>
	public bool stackable ()
	{
		return false;
	}
	
	/// <summary>
	/// Determines if this debuff can have its duration reset.
	/// </summary>
	public bool prolongable ()
	{
		return true;
	}
	
	/// <summary>
	/// Refresh the duration of this debuff.
	/// </summary>
	public void refresh ()
	{
		expirationTime = Time.time + duration;
	}
	
	/// <summary>
	/// Returns the tooltip description of this debuff.
	/// </summary>
	public string description ()
	{
		return "Miasma \n Slows the target by " + (slowAmount * 10) + "%";
	}
	
	/// <summary>
	/// Returns the name of this debuff, used in the debuff hashtable.
	/// </summary>
	public string name ()
	{
		return "Miasma";
	}
	
	public void update()
	{
		;	
	}
	#endregion
}
