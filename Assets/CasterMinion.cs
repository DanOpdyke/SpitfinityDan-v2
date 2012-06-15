using UnityEngine;
using System.Collections;

public class CasterMinion : MinionScript {
	private int numMaxMissles = 10;
	private int index = 0;
	
	public GameObject wizmis;
	
	// Use this for initialization
	void Start () {
		currentHealth = 70;
		maxHealth = 70;
		animator = gameObject.GetComponent(typeof(Animation)) as Animation;
		alive = true;
		range = 70;
		debuffs = new ArrayList();
	}
	
	void OnGUI(){
		if(getHealthPercent() > 0){ //If not dead
			Vector3 healthBarPosition = Camera.main.WorldToScreenPoint(gameObject.transform.position);
			GUI.Box(new Rect(healthBarPosition.x - 23, Screen.height - healthBarPosition.y - 70, 50 * getHealthPercent(), 10), healthTexture);
		}
		
	}
	
	// Update is called once per frame
	void Update () {
	if(!alive)
			return;
		
		if(!player.isAlive()){
			return; //TODO make this more interesting after the player dies
		}
		
		for(int i = 0; i < debuffs.Count; i++)
			if(((Debuff)debuffs[i]).hasExpired()){
				debuffs.Remove(i--);
				Debug.Log("Debuff expired");
			}
		
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
		
		float distance = (player.transform.position - gameObject.transform.position).magnitude;
		
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
					spawnLocation.y = 12.0f;
					
					GameObject temp = (GameObject) Instantiate(wizmis, spawnLocation, gameObject.transform.rotation);
					ProjectileScript script = temp.GetComponent(typeof(ProjectileScript)) as ProjectileScript;
					script.setDest(player.transform.position);
					script.setPlayer(player);
					
					/*Missle m = new Missle();
					Vector3 healthBarPosition = Camera.main.WorldToScreenPoint(gameObject.transform.position);
					m.setX(healthBarPosition.x);
					m.setY(Screen.height - healthBarPosition.y);
					activeMissles[index++] = m;*/
				}
				
				nextAttack = Time.time + weaponSpeed;
				animator.Stop();
				animator.Play("Attack1");
			}
		}
		
		//If not in attack range
		else{
			if(!animator.IsPlaying("Run")){
				animator.Stop();
				animator.Play("Run");
			}
			if(!fleeing)
				dest = player.transform.position;
			
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
		
		
		gameObject.transform.LookAt(dest);
		
	}
	
}
