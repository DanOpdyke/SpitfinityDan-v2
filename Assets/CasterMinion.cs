/*
 * Filename: CasterMinion.cs
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
/// The Caster Minion script simluates the basic behavior of a ranged minion. If the Player is outside of the minion's
/// attack range, it will attempt to move towards the Player. In future iterations, minions will be given pathfinding
/// scripts, as currently they will get stuck at walls between themself and the Player. When within attack range, the caster
/// minion will fire a dodgeable missile towards the Player. 
/// 
/// Currently, there is a fairly large amount of redundent code between the melee and caster minion's update functions. In future
/// iterations, the redundant code will be moved into the EnemyScript super class, and each subclass will begin their update functions
/// by called "Super.Update()".
/// </summary>
public class CasterMinion : EnemyScript {
	
	/// <summary>
	/// The maximum number of missles this minion may have active at a single time. Currently, the game is not large enough
	/// in scale for this to matter, but in future iterations, this will help to avoid overwhelming framerate issues.
	/// </summary>
	private int numMaxMissles = 10;
	
	/// <summary>
	/// The index of the next free missle slots. In future iterations, missles will be pulled from a pool instead of instantiated,
	/// to reduce unneccesary overhead.
	/// </summary>
	private int index = 0;
	
	/// <summary>
	/// The missles projectile to be instantiated when the caster minion attacks.
	/// </summary>
	public GameObject wizmis;
	
	// Use this for initialization
	void Start () {
		alive = true;
		range = 70;
		
		//We generally like to avoid try catch blockes. However, in Unity, it is required that a "dummy" instance of every object be 
		//present at the beginning of the game, or more mobs cannot be spawned. Since this "dummy" instance is present before the player
		//is instantied, it will be unable to find the player, and this initialization will fail. To remedy this situation, we surround
		//this assignment in a try-catch, then look to update the assignment in the "update" function. Future iterations should look
		//to address this Unity flaw.
		try{
		player = GameObject.FindGameObjectWithTag("Player").GetComponent(typeof(PlayerScript)) as PlayerScript;
		} catch{
			;	
		}
		animator = gameObject.GetComponent(typeof(Animation)) as Animation;
		debuffs = new Hashtable();
		maxHealth = 100 * Mathf.Pow(2, player.Level);
		currentHealth = maxHealth;
		weaponDamage = 5 * Mathf.Pow(2, player.Level);
		weaponSpeed = 1.5f;
	}
	
	/// <summary>
	/// Draws the minion's health bar above their position.
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
		
		if(!alive || Time.time < stunTime)
			return;
		
		if(player == null)
			player = GameObject.FindGameObjectWithTag("Player").GetComponent(typeof(PlayerScript)) as PlayerScript;
		
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
				//player.damage(weaponDamage);
				
				if(index <= 9){
					Vector3 spawnLocation = gameObject.transform.position;
					spawnLocation.y = player.getGameObject().transform.position.y;
					
					GameObject temp = (GameObject) Instantiate(wizmis, spawnLocation, gameObject.transform.rotation);
					PhysicsProjectileScript script = temp.GetComponent(typeof(PhysicsProjectileScript)) as PhysicsProjectileScript;
					script.setSpeed(20);
					script.setDamage(weaponDamage);
					script.setTimeToLive(8);
					script.setDestination(player.getGameObject().transform.position);
				}
				
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
			
			(gameObject.GetComponent(typeof(CharacterController)) as CharacterController).Move((dest - gameObject.transform.position).normalized * Time.smoothDeltaTime * 10);
		}
		
		dest.y = gameObject.transform.position.y;
		gameObject.transform.LookAt(dest);
		
	}
	
}
