/*
 * Filename: NoxiousBlastDebuff.cs
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
/// NoxiousBlastDebuff represents the debuff that is applied when hit by Cassiopeia's Noxious Blast ability.
/// Upon being hit, the Player takes period damage, and is set to a poisoned
/// state, allowing Cassiopeia to use her Twin Fang ability. After a set duration of time, the Noxious Blast
/// debuff expires. If the Player reenters or remains within the Noxious Blast poison area, the debuff duration
/// is refreshed.
/// </summary>
public class NoxiousBlastDebuff : MonoBehaviour, Debuff {
	
	/// <summary>
	/// The time at which the NoxiousBlastDebuff should expire.
	/// </summary>
	private float expirationTime;
	
	/// <summary>
	/// The total duration of the NoxiousBlastDebuff, which may be refreshed.
	/// </summary>
	private float duration = 8;
	
	/// <summary>
	/// The damage of the NoxiousBlastDebuff. Currently, this does not scale with Cassiopeia's level, as this
	/// was found to significantly increase the difficulty of the fight.
	/// </summary>
	private float damage = 5;
	
	/// <summary>
	/// The time at which the poison may tick again, if applicable.
	/// </summary>
	private float nextTickTime;
	
	/// <summary>
	/// The delay time between ticks of the poison.
	/// </summary>
	private float timeBetweenTicks = 2;
	
	/// <summary>
	/// The Player who is receiving the NoxiousBlastDebuff
	/// </summary>
	private PlayerScript player;
	
	/// <summary>
	/// The texture of the NoxiousBlastDebuff, shown above the Player's healthbar.
	/// </summary>
	private Texture2D texture;
	
	/// <summary>
	/// Initializes a new instance of the <see cref="NoxiousBlastDebuff"/> class,
	/// setting its expiration time and texture.
	/// </summary>
	public NoxiousBlastDebuff() {
		expirationTime = Time.time + duration;
		texture = Resources.Load("Debuffs/NoxiousBlast") as Texture2D;
	}

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	}

	#region Debuff implementation
	public bool hasExpired ()
	{
		return Time.time > expirationTime;
	}

	public float applyDebuff (float damage)
	{
		return damage;
	}

	public void applyStack (int numAdditionalStacks)
	{
		; //Noxious Blast does not stack.
	}

	public Texture2D getTexture ()
	{
		return texture;
	}

	public void apply (PlayerScript player)
	{
		player.setPoisoned(true);
	}

	public void expire (PlayerScript player)
	{
		player.setPoisoned(false);
	}

	public bool stackable ()
	{
		return false;
	}

	public bool prolongable ()
	{
		return true;
	}
	
	public void refresh() {
		expirationTime = Time.time + duration;
	}
	
	public string description() {
		return "Noxious Blast \n Suffer " + damage + " damage every " + timeBetweenTicks + " seconds. ";	
	}
	
	public string name() {
		return "Noxious Blast";	
	}
	
	public void update(){
		if(Time.time > nextTickTime){
			nextTickTime = Time.time + timeBetweenTicks;
			player.damage(damage);
		}
	}
	#endregion
}
