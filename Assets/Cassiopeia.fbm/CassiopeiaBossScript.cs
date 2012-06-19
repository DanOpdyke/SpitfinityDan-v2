using UnityEngine;
using System.Collections;

public class CassiopeiaBossScript : EnemyScript {
	private int phase;
	public GameObject miasma_object;
	public GameObject noxious_blast_object;
	private GameObject Petrifiying_Gaze;
	
	private PlayerScript player;
	
	#region Ability Objects
	private Twin_Fang twin_fang;
	private Noxious_Blast noxious_blast;
	private Miasma miasma;
	#endregion

	// Use this for initialization
	void Start () {
		currentHealth = 400;
		maxHealth = 400;
		alive = true;
		debuffs = new ArrayList();
		
		player = GameObject.FindGameObjectWithTag("Player").GetComponent(typeof(PlayerScript)) as PlayerScript;
		
		phase = 1;
		
		twin_fang = new Twin_Fang(player);
		
		noxious_blast = new Noxious_Blast(player, noxious_blast_object);
		
		miasma = new Miasma(player, miasma_object);
	}
	
	// Update is called once per frame
	void Update () {
		
		if(player == null)
			player = GameObject.FindGameObjectWithTag("Player").GetComponent(typeof(PlayerScript)) as PlayerScript;
		
		//Phase 1
		if(phase == 1){
			//If the player is poisoned, use twin fangs
			if(player.isPoisoned() && twin_fang.offCooldown()){
				twin_fang.Execute();
				return;
			}
			
			//If off cooldown, attempt to use Noxious Blast to poison player
			if(noxious_blast.offCooldown()){
				noxious_blast.Execute();	
			}
			//Otherwise, autoattack
			
		}
		
		else if (phase == 2){
			//If the player is poisoned, use twin fangs
			if(player.isPoisoned() && twin_fang.offCooldown()){
				twin_fang.Execute();
				return;
			}
			
			//If off cooldown, attempt to use Miasma to poison player
			if(miasma.offCooldown()){
				miasma.Execute();	
			}
			
			//Otherwise, autoattack
			
		}
		
		else{
			//If the player is poisoned, use twin fangs
			
			
			//If off cooldown, attempt to use Noxious Blast or Miasma to poison player
			
			
			//Otherwise, autoattack
			
		}
	}
	
	
	private void transitionPhase(){
		;	
	}
	
	class Twin_Fang : Ability {
		private float damage = 40;
		private float nextAttack;
		private PlayerScript player;
		
		public Twin_Fang(PlayerScript player){
			this.player = player;
		}
		
		public void Execute(){
			//TODO Implement Particle, and delay damgae until particle hits player
			//player.damage(damage);
			nextAttack = Time.time + 1;
		}
		
		public void setScript(PlayerScript player){
			this.player = player;
		}
		
		public bool offCooldown(){
			return Time.time > nextAttack;
		}
	}
	
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
			Instantiate(noxious_object, player.getGameObject().transform.position, Quaternion.identity);
			nextAttack = Time.time + coolDown;
		}
		
		
		public bool offCooldown(){
			return Time.time > nextAttack;
		}
	}
	
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
			Instantiate(miasma_object, player.getGameObject().transform.position, Quaternion.identity);
			nextAttack = Time.time + coolDown;
		}
		
		public bool offCooldown(){
			return Time.time > nextAttack;
		}
	}
}
