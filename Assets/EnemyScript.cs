/*
 * Filename: EnemyScript.cs
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
/// The Enemy Script abstract class specifies the general behavior of all minions. As many of the functions of
/// the minions are shared, we decided to use an abstract class instead of an interface.
/// </summary>
public abstract class EnemyScript : MonoBehaviour {
	
	/// <summary>
	/// The health texture of the minion.
	/// </summary>
	public Texture2D healthTexture;
	
	/// <summary>
	/// The current health of the minion.
	/// </summary>
	protected float currentHealth;
	
	/// <summary>
	/// The maximum possible health of the minion.
	/// </summary>
	protected float maxHealth;
	
	/// <summary>
	/// The original scale of x axis for the minion. In future iterations, we may introduce
	/// spells which modify various scales of the minions.
	/// </summary>
	protected float originalXScale;
	
	/// <summary>
	/// The attack range of the minion.
	/// </summary>
	protected float range;
	
	/// <summary>
	/// The weapon damage of the minion.
	/// </summary>
	protected float weaponDamage;
	
	/// <summary>
	/// The weapon speed of the minion.
	/// </summary>
	protected float weaponSpeed;
	
	/// <summary>
	/// The time at which the minion may next attack.
	/// </summary>
	protected float nextAttack = 0;
	
	/// <summary>
	/// The movement speed of the minion, modifiable by buffs and debuffs.
	/// </summary>
	protected float movespeed = 0.1f;
	
	/// <summary>
	/// The time at which the minion should be destroyed. This allows the death animation to
	/// finish.
	/// </summary>
	protected float deathTimer;
	
	/// <summary>
	/// The time at which the minion will no longer be snared.
	/// </summary>
	protected float snareTimer;
	
	/// <summary>
	/// The time at which the minon will no longer be stunnned.
	/// </summary>
	protected float stunTime;
	
	/// <summary>
	/// The position which the minion will move towards, if not attacking.
	/// </summary>
	protected Vector3 dest;
	
	/// <summary>
	/// The original position of the minion when spawned.
	/// </summary>
	protected Vector3 originalPosition;
	
	/// <summary>
	/// Determines if the minion is currently fleeing due to being low on health.
	/// </summary>
	protected bool fleeing;
	
	/// <summary>
	/// The time at which the minion will determine if it will continue fleeing, or begin
	/// pursueing the Player.
	/// </summary>
	protected float fleeTime;
	
	/// <summary>
	/// The health orb object which may be dropped when the minion is killed.
	/// </summary>
	public GameObject healthOrb;
	
	/// <summary>
	/// The item loot object which will always be dropped upon minion death.
	/// </summary>
	public GameObject itemLoot;
	
	/// <summary>
	/// A Hashtable mapping Debuff names to Debuff objects.
	/// </summary>
	protected Hashtable debuffs;
	
	/// <summary>
	/// Determines if the minion is currently alive.
	/// </summary>
	protected bool alive;
	
	/// <summary>
	/// The animation object of the minion.
	/// </summary>
	protected Animation animator;
	
	/// <summary>
	/// The current Player object.
	/// </summary>
	public PlayerScript player;
	
	// Use this for initialization
	void Start () {
		currentHealth = 100;
		maxHealth = 100;
		animator = gameObject.GetComponent(typeof(Animation)) as Animation;
		alive = true;
		debuffs = new Hashtable();
		player = GameObject.FindGameObjectWithTag("Player").GetComponent(typeof(PlayerScript)) as PlayerScript;
		originalPosition = gameObject.transform.position;
	}
	
	/// <summary>
	/// Paints the minion's health above its model.
	/// </summary>
	void OnGUI(){
		if(getHealthPercent() > 0){ //If not dead
			Vector3 healthBarPosition = Camera.main.WorldToScreenPoint(gameObject.transform.position);
			GUI.Box(new Rect(healthBarPosition.x - 23, Screen.height - healthBarPosition.y - 70, 50 * getHealthPercent(), 10), healthTexture);
		}
	}
	
	// Update is called once per frame
	void Update () {
		if(Time.time > deathTimer && !alive)
			Destroy(gameObject);
		
		if(!alive)
			return;
		
		if(!player.isAlive()){
			return; //TODO make this more interesting after the player dies
		}

		string[] keys = new string[debuffs.Keys.Count];
		debuffs.Keys.CopyTo(keys, 0);
		
		//Check for expired debuffs
		foreach(string obj in keys){
			Debuff debuff = (Debuff) debuffs[obj];
			if(debuff.hasExpired()){
				debuff.expire(player);
				debuffs.Remove(debuff.name());
			}
		}
		
		updateDebuffs();
		
		if(Time.time > fleeTime)
			fleeing = false;
		
		if(getHealthPercent() < 0.20f && !fleeing){
			//80% chance to flee at low health
			Random.seed = (int)Time.time;
			if(Random.value <= 0.8f){
				fleeing = true;
				fleeTime = Time.time + 2;
				dest = new Vector3(100.0f * Random.value, gameObject.transform.position.y, 100.0f * Random.value);
			}
		}
		
		float distance = (player.getGameObject().transform.position - gameObject.transform.position).magnitude;
		
		//If in attack range
		if(distance <= range && !fleeing){
			if(animator.IsPlaying("Run"))
				animator.Stop();
			
			if(!animator.isPlaying)
				animator.Play("Idle");
			
			if(Time.time > nextAttack){
				player.damage(weaponDamage);
				nextAttack = Time.time + weaponSpeed;
				animator.Stop();
				animator.Play("Attack1");
			}
		}
		
		//If not in attack range
		else{
			if(snareTimer > Time.time)
				return;
			if(!animator.IsPlaying("Run")){
				animator.Stop();
				animator.Play("Run");
			}
			if(!fleeing)
				dest = player.getGameObject().transform.position;
			
			//TODO Implement pathfinding algorithm to avoid running through objects
			int directionX = dest.x > gameObject.transform.position.x ? 1 : -1;
			
			float deltaX = dest.x - gameObject.transform.position.x;
			if(Mathf.Abs(deltaX) > movespeed)
				deltaX = movespeed;
			
			int directionZ = dest.z > gameObject.transform.position.z ? 1 : -1;
			
			float deltaZ = dest.z - gameObject.transform.position.z;
			if(Mathf.Abs(deltaZ) > movespeed)
				deltaZ = movespeed;
			
			Vector3 newPos = new Vector3(gameObject.transform.position.x + (deltaX * directionX), gameObject.transform.position.y, gameObject.transform.position.z + (deltaZ * directionZ));
			
			gameObject.transform.position = newPos;
		}
		
		dest.y = gameObject.transform.position.y;
		gameObject.transform.LookAt(dest);
		
	}
	
	/// <summary>
	/// Stuns the minion for the specified period of time.
	/// </summary>
	/// <param name='time'>
	/// The duration of the stun.
	/// </param>
	public void stun(float time){
		stunTime = Time.time + time;
	}
	
	/// <summary>
	/// Gets the current health percentage of the minon.
	/// </summary>
	/// <returns>
	/// Health percentage of the minion.
	/// </returns>
	public float getHealthPercent(){
		return currentHealth / maxHealth;	
	}
	
	/// <summary>
	/// Updates the minion debuffs.
	/// </summary>
	protected void updateDebuffs(){
		string[] keys = new string[debuffs.Keys.Count];
		debuffs.Keys.CopyTo(keys, 0);
		
		//Check for expired debuffs
		foreach(string obj in keys){
			Debuff debuff = (Debuff) debuffs[obj];
			debuff.update();
		}
	}
	
	
	/// <summary>
	/// Applies the specified amount of damage to the minion, handling the event
	/// that the minion dies.
	/// </summary>
	/// <param name='amount'>
	/// Amount of damage to apply to the minion.
	/// </param>
	public void damage(float amount){
		if(!alive)
			return;
		
		string[] keys = new string[debuffs.Keys.Count];
		debuffs.Keys.CopyTo(keys, 0);
		
		//Check for expired debuffs
		foreach(string obj in keys){
			Debuff debuff = (Debuff) debuffs[obj];
			amount = debuff.applyDebuff(amount);
		}
		
		currentHealth -= amount;
		if(currentHealth <= 0){
			animator.Stop();
			animator.Play("Death");
			currentHealth = 0;
			if(player.getCurrentEnemy() == this)
				player.setCurrentEnemy(null);
			alive = false;
			DropLoot();
			
			deathTimer = Time.time + 3;
			
			if(gameObject.GetComponent(typeof(SphereCollider)))
				(gameObject.GetComponent(typeof(SphereCollider)) as SphereCollider).enabled = false;
		}
	}
	
	/// <summary>
	/// Gets the destination location of the minion.
	/// </summary>
	/// <returns>
	/// The destination that the minion is heading.
	/// </returns>
	public Vector3 getDest(){
		return dest;
	}
	
	/// <summary>
	/// Randomly generates and drops loot near the minion's model.
	/// </summary>
	protected void DropLoot(){
		
		//Drop health orbs
		if(Random.value >= 0.5f)
		{
			Vector3 itemPosition = transform.position;
			
			float deltaX = Random.value * 4;
			if(Random.value > .5)
				deltaX *= -1;
			float deltaZ = Random.value * 4;
			if(Random.value > .5)
				deltaZ *= -1;
			
			itemPosition.x += deltaX;
			itemPosition.z += deltaZ;
			itemPosition.y = 10;
			
			Instantiate(healthOrb, itemPosition, Quaternion.identity);
		}
		
		//Drop item loot
		
		GameObject loot = (GameObject) Instantiate(itemLoot, transform.position, Quaternion.identity);
		ItemLootScript lootScript = loot.GetComponent(typeof(ItemLootScript)) as ItemLootScript;
		lootScript.setLevel(player.Level);
		lootScript.randomizeItem();
		
	}
	
	/// <summary>
	/// Applies a specified Debuff to the minion.
	/// </summary>
	/// <param name='debuff'>
	/// The Debuff to be applied.
	/// </param>
	/// <param name='stacking'>
	/// If the Debuff is stackable.
	/// NOTE: This is no longer neccessary, as the Debuff tracks if it is stackable.
	/// </param>
	public void applyDebuff(Debuff debuff, bool stacking){
		
		if(debuffs.ContainsKey(debuff.name())) {
			Debuff currentDebuff = (Debuff) debuffs[debuff.name()];
			if(debuff.prolongable()){
				currentDebuff.refresh();
			}
			else if(debuff.stackable())
				currentDebuff.applyStack(1);
		}
		else
			debuffs.Add(debuff.name(), debuff);
		
		
	}
	
	/// <summary>
	/// Snares the minion for the specified duration.
	/// </summary>
	/// <param name='duration'>
	/// Duration of the snare.
	/// </param>
	public void snare(float duration){
		snareTimer = Time.time + duration;	
	}
	
	/// <summary>
	/// Gets the minion's debuffs, used to allow the Player to draw the debuffs below
	/// the minion's health bar.
	/// </summary>
	/// <returns>
	/// The minion's debuffs
	/// </returns>
	public Hashtable getDebuffs(){
		return debuffs;	
	}
}