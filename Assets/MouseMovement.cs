/*
 * Filename: MouseMovement.cs
 * 
 * Author:
 * 		Programming: Daniel Opdyke, David Spitler
 * 
 * Last Modified: 7/6/2012
 * 
 * */

using UnityEngine;
using System.Collections;

/// <summary>
/// MouseMovement is a shared script between all Playerscripts. During updates, the MouseMovement script
/// looks for changes in the mouse state, and does the corresponding Player movement, attacks, or looting, as
/// well as transitions the idle and running animations.
/// NOTE: The MouseMovement class only determines auto-attacks, and is not responsible to perform any other Player
/// spells.
/// </summary>
public class MouseMovement : MonoBehaviour {
	
	/// <summary>
	/// The movement speed of the Player, modified by buffs and debuffs.
	/// </summary>
	private float PLAYER_MOVEMENT_SPEED = 0.5F;
	
	/// <summary>
	/// The last position in space that the Player was directed to move towards.
	/// During the update loop, if this position is not equal to the Player's 
	/// position, the Player will move towards it.
	/// </summary>
	private Vector3 dest;
	
	/// <summary>
	/// The direction of movement needed to reach the destination position.
	/// </summary>
	private Vector3 direction;
	
	/// <summary>
	/// The CharacterMotor object used to alter the position of the Player.
	/// </summary>
	private CharacterMotor motor;
	
	/// <summary>
	/// The RaycastHit, which describes which objects are under the mouse at the time of left-click.
	/// Can collider with any object that has a collider, including loot, enemies, and terrain.
	/// </summary>
	private RaycastHit hit;
	
	/// <summary>
	/// The time until the next Player auto-attack is possible. Determined by the Player class.
	/// </summary>
	private float nextAttack = 0;
	
	/// <summary>
	/// Reference to the current Player.
	/// </summary>
	private PlayerScript player;
	
	/// <summary>
	/// Determines if the Player is currently moving. This ensures that when a Player is displaced,
	/// it does not automatically begin moving towards its previous destination.
	/// </summary>
	private bool moving = false;
	
	/// <summary>
	/// Determines if the Player is currently moving towards a target to attack. If true, a Player
	/// will automatically being auto-attacks when within range.
	/// </summary>
	private bool movingToAttack = false;
	
	/// <summary>
	/// Determines if a Player is currently auto-attacking. If true, the Player's auto-attack will
	/// be used as soon as it is off cooldown, assuming it has a target.
	/// </summary>
	private bool attacking = false;
	
	/// <summary>
	/// Determines if the Player is currently idle. If the Player is idle, they will begin playing
	/// randomly chosen idle animations.
	/// </summary>
	private bool idling;
	
	/// <summary>
	/// The time at which idle animations will begin Playing. Used to give a brief delay between an 
	/// attack, and the following idle animation.
	/// </summary>
	private float idleTime;
	
	/// <summary>
	/// Sets the current state of the Player moving. This helps to avoid issues with Loading characters,
	/// and them desiring to move back to their previous destination. In future iterations, this function
	/// should be removed to make a clear seperation of concerns.
	/// </summary>
	/// <param name='moving'>
	/// If the Player should move towards its destination.
	/// </param>
	public void setMoving(bool moving){
		this.moving = moving;	
	}
	
	/// <summary>
	/// The range at which loot can be picked up.
	/// </summary>
	private float lootRange = 20;
	
	// Use this for initialization
	void Start () {
		dest = transform.position;
		motor = GetComponent(typeof(CharacterMotor)) as CharacterMotor;
		player = GameObject.FindGameObjectWithTag("Player").GetComponent(typeof(PlayerScript)) as PlayerScript;
	}
	
	// Update is called once per frame
	void Update () {
		
		
		if (Input.GetButtonUp("Fire1")) //Released left click
			attacking = false;
		
		if(!player.isAlive() || player.stunned())
			return;
		
		
		if((!moving) && player.isRunAnimation()){
			player.playAnimation("Idle1");
			//return;
		}
		
		//Update movement speed to take current player buffs and debuffs into account
		PLAYER_MOVEMENT_SPEED = player.MovementSpeed * 0.5f;
		
		/*
		 * Conditions that must be met to start idling:
		 * 1) Not yet idling
		 * 2) At current destination (not running)
		 * 3) Not attacking (holding down left click)
		 * 4) Not in another animation (spells, etc)
		 * */
		if(transform.position - dest == Vector3.zero && !attacking && !idling && (player.isRunAnimation() || player.noAnimation())){
			player.stopAnimation();
			idling = true;
			moving = false;
			player.setRunning(false);
			idleTime = Time.time + 5;
		}
	
		
		else if(idling && Time.time > idleTime && player.noAnimation())
			player.playIdleSequence();
		

		//Check for mouse click and update destination
		if(Input.GetButtonDown("Fire1") && !player.guiInteraction(Input.mousePosition)){
			idling = false;
			player.setIdling(false);
			
			movingToAttack = false;
			
			player.setRunning(true);
			
			Ray ray = Camera.allCameras[0].ScreenPointToRay(Input.mousePosition);
			
			int layermask = (1 << 10) | (1 << 12);
			layermask = ~layermask;
			
			if(Physics.Raycast(ray, out hit, 10000, layermask)){
				
				ItemLootScript loot = hit.collider.gameObject.GetComponent(typeof(ItemLootScript)) as ItemLootScript;
				if(loot && (Vector3.Distance(player.getGameObject().transform.position, loot.gameObject.transform.position) < lootRange)) {
					player.awardItem(loot.getItem());
					Destroy(loot.gameObject);
					return;
				}
				
				EnemyScript enemy = hit.collider.gameObject.GetComponent(typeof(EnemyScript)) as EnemyScript;
				if(enemy){ //If target is an enemy
					attacking = true;
					player.setCurrentEnemy(enemy);
					if(Time.time > nextAttack && ((enemy.gameObject.transform.position - player.getGameObject().transform.position).magnitude <= player.getRange())){
						nextAttack = player.autoAttack(enemy);
						Vector3 enemyLookAt = enemy.gameObject.transform.position;
						enemyLookAt.y = player.getGameObject().transform.position.y;
						player.getGameObject().transform.LookAt(enemyLookAt);
						dest = player.getGameObject().transform.position;
					}
					//TODO See if we can make this all just one conditional
					//If cooldown is complete, but too far away to attack
						else if(Time.time > nextAttack){
						dest = hit.point;
						dest.y =  player.getGameObject().transform.position.y;
					    direction = hit.point - transform.position;
						direction.y = player.getGameObject().transform.position.y;
						if(direction.magnitude > 1)
							direction = direction.normalized;
						Vector3 toLookAt = dest;
						toLookAt.y = player.getGameObject().transform.position.y;
						GameObject.FindGameObjectWithTag("Player").transform.LookAt(toLookAt);
						movingToAttack = true;
						moving = true;
					}
					
					
				}
				else{ //If not enemy
				    dest = hit.point;
					dest.y = player.getGameObject().transform.position.y;
				    direction = hit.point - transform.position;
					direction.y = player.getGameObject().transform.position.y;
					if(direction.magnitude > 1)
						direction = direction.normalized;
					
					Vector3 toLookAt = dest;
					toLookAt.y = player.getGameObject().transform.position.y;
					GameObject.FindGameObjectWithTag("Player").transform.LookAt(toLookAt);
					moving = true;
				}
			}
		}
		
		else if (Input.GetButtonUp("Fire1")) //Released left click
			attacking = false;
		
		
		//Moving towards target to attack, and gets within range.
		if(attacking && player.getCurrentEnemy() != null && Time.time > nextAttack && 
				(player.getCurrentEnemy().gameObject.transform.position - player.getGameObject().transform.position).magnitude <= player.getRange()){
			EnemyScript enemy = player.getCurrentEnemy();
			dest = transform.position; //Stop moving
			moving = false;
			dest.y = player.getGameObject().transform.position.y;
			nextAttack = player.autoAttack(enemy);
			Vector3 enemyLookAt = enemy.gameObject.transform.position;
			enemyLookAt.y = player.getGameObject().transform.position.y;
			transform.LookAt(enemyLookAt);
			
			movingToAttack = false;
			//nextAttack = Time.time + player.getWeaponSpeed();
		}
		
		if(Vector3.Distance(dest, gameObject.transform.position) > .001f && moving){
			float distance = Vector3.Distance(gameObject.transform.position, dest);
			if(distance < 1f){ //Reach destination
				dest.y = transform.position.y;
				transform.position = dest;
				player.setRunning(false);
				moving = false;
				return;
			}
			CharacterController controller = gameObject.GetComponent(typeof(CharacterController)) as CharacterController;
			dest.y = gameObject.transform.position.y;
			controller.Move((dest - gameObject.transform.position).normalized * PLAYER_MOVEMENT_SPEED * (Time.smoothDeltaTime * 100));
		}
		
	}
	
}
