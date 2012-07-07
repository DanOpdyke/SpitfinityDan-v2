/*
 * Filename: CassiopeiaBossScript.cs
 * 
 * Author:
 * 		Programming: Daniel Opdyke, David Spitler
 * 		Character Design: David Spitler
 * 
 * Last Modified: 6/22/2012
 * 
 * NOTE: All Models, Original Character Concepts, and Icons are property of Riot Games.
 * */
using UnityEngine;
using System.Collections;

/// <summary>
/// CassiopeiaBossScript represents a modified version of the League of Legends Cassiopeia character.
/// When a Player is within range, Cassiopeia will begin laying poison zones on the ground, and will
/// perform dodgeable auto-attacks when other spells are on cooldown. If the Player is hit by a poison,
/// Cassiopeia will begin using Twin Fang to "punish" the mistake. To successfully defeat Cassiopeia, it
/// is highly advised that the Player avoid the poison zones.
/// </summary>
public class CassiopeiaBossScript : EnemyScript {
	
	/// <summary>
	/// The current phase of Cassiopeia, as determined by current health. As Cassiopeia changes phases,
	/// she begins to utilize different spells.
	/// NOTE: Although she is currently unable to heal, if such an ability was added, she would not
	/// currently transition to a previous phase.
	/// </summary>
	private int phase;
	
	/// <summary>
	/// Object representing the Miasma spell, used to provide a visual representation of where the Miasma
	/// spell has been cast, as well as to calculate collisions.
	/// </summary>
	public GameObject miasma_object;
	
	/// <summary>
	/// Object representing the Noxious Blast spell, used to provide a visual representation of where the Noxious
	/// Blast spell has been cast, as well as to calculate collisions.
	/// </summary>
	public GameObject noxious_blast_object;
	
	/// <summary>
	/// Object representing the Twin Fang spell, used to provide a visual representation of where the Twin
	/// Fan spell is currently located, as well as when it will hit the Player.
	/// NOTE: Twin Fang is a "locked on" spell, and will always hit the Player when cast.
	/// </summary>
	public GameObject twin_fang_object;
	
	/// <summary>
	/// Object representing the auto-attack of Cassiopeia, used to provide visual respresentation of where
	/// the auto-attack model is currently located, and if it will hit a Player.
	/// NOTE: Cassiopeia's auto-attack is dodgeable, and moves in a fixed direction after being cast.
	/// </summary>
	public GameObject autoAttack;
	
	
	/// <summary>
	/// The time at which Cassiopeia will be able to use another auto-attack.
	/// </summary>
	private float nextAttack;
	
	/// <summary>
	/// The range at which Cassiopeia will begin attacking the Player.
	/// </summary>
	private float aggroRange = 200;
	
	/// <summary>
	/// Determines if the Player should have its "gameWon" variable set. We want this set only one
	/// time in update per life. Originally, this functionality was placed within the "damage" function,
	/// but we ran into inheritance issues. In future iterations, this variable should be removed.
	/// </summary>
	private bool staleWin;
	
	#region Ability Objects
	/// <summary>
	/// Reference to Twin Fang ability.
	/// </summary>
	private Twin_Fang twin_fang;
	
	/// <summary>
	/// Reference to Noxious Blast ability.
	/// </summary>
	private Noxious_Blast noxious_blast;
	
	/// <summary>
	/// Reference to Miasma ability.
	/// </summary>
	private Miasma miasma;
	#endregion

	// Use this for initialization
	void Start () {
		
		alive = true;
		debuffs = new Hashtable();
		phase = 1;
		animator = gameObject.GetComponent(typeof(Animation)) as Animation;
		//We generally like to avoid try catch blockes. However, in Unity, it is required that a "dummy" instance of every object be 
		//present at the beginning of the game, or more mobs cannot be spawned. Since this "dummy" instance is present before the player
		//is instantied, it will be unable to find the player, and this initialization will fail. To remedy this situation, we surround
		//this assignment in a try-catch, then look to update the assignment in the "update" function. Future iterations should look
		//to address this Unity flaw.
		try{
			player = GameObject.FindGameObjectWithTag("Player").GetComponent(typeof(PlayerScript)) as PlayerScript;
			twin_fang = new Twin_Fang(player, twin_fang_object);
			noxious_blast = new Noxious_Blast(player, noxious_blast_object);
			miasma = new Miasma(player, miasma_object);
		} catch{
			;	
		}
		
		maxHealth = 750 * Mathf.Pow(2, player.Level);
		weaponDamage = 15 * Mathf.Pow(2, player.Level);
		currentHealth = maxHealth;
		
		
	}
	
	// Update is called once per frame
	void Update () {
				
		if(player == null){
			player = GameObject.FindGameObjectWithTag("Player").GetComponent(typeof(PlayerScript)) as PlayerScript;
			twin_fang = new Twin_Fang(player, twin_fang_object);
			noxious_blast = new Noxious_Blast(player, noxious_blast_object);
			miasma = new Miasma(player, miasma_object);
		}
		
		if(!alive && !staleWin){
			player.setWin(true);
			staleWin = true;
			return;
		}
		
		if(Time.time > deathTimer && !alive)
			Destroy(gameObject);
		
		if(!player.isAlive())
			return;
		
		Vector3 lookat = player.getGameObject().transform.position;
		lookat.y = gameObject.transform.position.y;
		gameObject.transform.LookAt(lookat);
			
		
		//If the Player is too far away to be aggroed, do not cast spells.
		if(Vector3.Distance(player.getGameObject().transform.position, gameObject.transform.position) > aggroRange)
			return;
		
		//Phase 1
		if(phase == 1){
			//If the player is poisoned, use twin fangs
			if(player.isPoisoned() && twin_fang.offCooldown() && !animator.isPlaying){
				animator.Play("Spell2");
				twin_fang.setInitialPosition(gameObject.transform.position);
				twin_fang.Execute();
				return;
			}
			
			//If off cooldown, attempt to use Noxious Blast to poison player
			if(noxious_blast.offCooldown() && !animator.isPlaying){
				animator.Play("Spell1");
				noxious_blast.Execute();
				return;
			}
			//Otherwise, autoattack
			if(nextAttack < Time.time){
				GameObject attack = (GameObject) Instantiate(autoAttack, gameObject.transform.position, Quaternion.identity);
				PhysicsProjectileScript projectile = attack.GetComponent(typeof(PhysicsProjectileScript)) as PhysicsProjectileScript;
				projectile.setTimeToLive(10);
				projectile.setDamage(weaponDamage);
				projectile.setSpeed(25);
				projectile.setDestination(player.getGameObject().transform.position);
				nextAttack = Time.time + 3;
			}
			
			if((currentHealth / maxHealth) < .66){
				//Phase Transition
				transitionPhase();
			}
			
		}
		
		else if (phase == 2){
			//If the player is poisoned, use twin fangs
			if(player.isPoisoned() && twin_fang.offCooldown()){
				animator.Play("Spell2");
				twin_fang.setInitialPosition(gameObject.transform.position);
				twin_fang.Execute();
				return;
			}
			
			//If off cooldown, attempt to use Miasma to poison player
			if(miasma.offCooldown()){
				animator.Play("Spell2");
				miasma.Execute();
				return;
			}
			
			//Otherwise, autoattack
			if(nextAttack < Time.time){
				GameObject attack = (GameObject) Instantiate(autoAttack, gameObject.transform.position, Quaternion.identity);
				PhysicsProjectileScript projectile = attack.GetComponent(typeof(PhysicsProjectileScript)) as PhysicsProjectileScript;
				projectile.setTimeToLive(10);
				projectile.setDamage(weaponDamage * 1.3f);
				projectile.setSpeed(35);
				projectile.setDestination(player.getGameObject().transform.position);
				nextAttack = Time.time + 3;
			}
			
			if((currentHealth / maxHealth) < .33){
				//Phase Transition
				transitionPhase();
			}
			
		}
		
		else{
			//If the player is poisoned, use twin fangs
			if(player.isPoisoned() && twin_fang.offCooldown()){
				animator.Play("Spell2");
				twin_fang.setInitialPosition(gameObject.transform.position);
				twin_fang.Execute();
				return;
			}
			
			//If off cooldown, attempt to use Noxious Blast or Miasma to poison player
			if(noxious_blast.offCooldown()){
				animator.Play("Spell1");
				noxious_blast.Execute();
				return;
			}
			
			if(miasma.offCooldown()){
				animator.Play("Spell2");
				miasma.Execute();
				return;
			}
			
			//Otherwise, autoattack
			if(nextAttack < Time.time){
				GameObject attack = (GameObject) Instantiate(autoAttack, gameObject.transform.position, Quaternion.identity);
				PhysicsProjectileScript projectile = attack.GetComponent(typeof(PhysicsProjectileScript)) as PhysicsProjectileScript;
				projectile.setTimeToLive(10);
				projectile.setDamage(weaponDamage * 1.5f);
				projectile.setSpeed(35);
				projectile.setDestination(player.getGameObject().transform.position);
				nextAttack = Time.time + 3;
			}
		}
	}
	
	
	/// <summary>
	/// Transitions between Cassiopeia's phases. During the transition, Cassiopia plays the animation for
	/// Petrifying Gaze, and the Player is stunned for 1.5 seconds.
	/// NOTE: The caller is responsible for using the transition phase function only at appropriate times
	/// (IE: Only when another phase exists).
	/// </summary>
	private void transitionPhase(){
		phase++;
		animator.Stop();
		animator.Play("Spell4");
		player.Stun(1.5f);
	}
	
	/// <summary>
	/// The Twin Fang ability is Cassiopeia's primary nuke, used only when the Player is poisoned. Once 
	/// cast, Twin Fang will always hit the Player, but can have its damaged delayed if the Player attempts to
	/// "kite" the spell while attacking.
	/// </summary>
	class Twin_Fang : Ability {
		private float damage;
		private float nextAttack;
		private PlayerScript player;
		private GameObject twin_fang_object;
		private Vector3 initialPosition;
		
		public Twin_Fang(PlayerScript player, GameObject twin_fang_object){
			this.player = player;
			this.twin_fang_object = twin_fang_object;
			damage = 20 * Mathf.Pow(2, player.Level);
		}
		
		public void setInitialPosition(Vector3 init){
			initialPosition = init;	
		}
		
		public void Execute(){
			//TODO Implement Particle, and delay damage until particle hits player
			//player.damage(damage);
			GameObject twinfang = (GameObject) Instantiate(twin_fang_object);
			twinfang.transform.position = initialPosition;
			SeekingProjectileScript projectile = twinfang.GetComponent(typeof(SeekingProjectileScript)) as SeekingProjectileScript;
			projectile.setSpeed(1);
			projectile.setDamage(damage);
			projectile.setTarget(player.getGameObject());
			nextAttack = Time.time + 3;
		}
		
		public void setScript(PlayerScript player){
			this.player = player;
		}
		
		public bool offCooldown(){
			return Time.time > nextAttack;
		}
	}
	
	/// <summary>
	/// The Noxious Blast ability is Cassiopeia's primary poison. After existing on the ground for a short period
	/// of time, Noxious Blast will apply a damaging debuff to any Player which enters its radius, as well as
	/// set the Player as poisoned, allowing use of the Twin Fang ability.
	/// </summary>
	class Noxious_Blast : Ability {
		private float nextAttack;
		private PlayerScript player;
		private float coolDown = 5;
		private GameObject noxious_object;
		
		public Noxious_Blast(PlayerScript player, GameObject noxious_object){
			this.player = player;
			this.noxious_object = noxious_object;
		}
		
		public void Execute(){
			//TODO Instantiate Noxious_Blast object
			((GameObject) Instantiate(noxious_object)).transform.position = player.getGameObject().transform.position;
			//, player.getGameObject().transform.position, Quaternion.identity
			nextAttack = Time.time + coolDown;
		}
		
		
		public bool offCooldown(){
			return Time.time > nextAttack;
		}
	}
	
	/// <summary>
	/// The Miasma ability is Cassiopeia's utility poison. After existing on the ground for a short period 
	/// of time, Miasma will slow any Player which enters its radius, as well as set the Player as poisoned,
	/// allowing the use of the Twin Fang ability. Every time that Miasma is triggered by the Player, it grows
	/// in size.
	/// </summary>
	class Miasma : Ability {
		private float nextAttack;
		private PlayerScript player;
		private float coolDown = 5;
		private GameObject miasma_object;
		
		public Miasma(PlayerScript player, GameObject miasma_object){
			this.player = player;
			this.miasma_object = miasma_object;
		}
		
		public void Execute(){
			//TODO Instantiate Miasma object
			((GameObject) Instantiate(miasma_object)).transform.position = player.getGameObject().transform.position;
			nextAttack = Time.time + coolDown;
		}
		
		public bool offCooldown(){
			return Time.time > nextAttack;
		}
	}
}
