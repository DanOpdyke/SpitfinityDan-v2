using UnityEngine;
using System.Collections;

public class GarenScript : MonoBehaviour, PlayerScript {
	
	//Spells
	private Decisive_Strike dStrike;
	private Courage courage;
	private Judgement judgement;
	private Demacian_Justice dJustice;
	private Valor valor;
	
	//Spell textures
	public Texture2D dStrikeTexture;
	public Texture2D courageTexture;
	public Texture2D judgementTexture;
	public Texture2D dJusticeTexture;
	public Texture2D valorTexture;
	public Texture2D mightOfDemaciaTexture;
	public Texture2D valorDebuffTexture;
	
	
	//Health textures
	public Texture2D enemyHealthTexture;
	public Texture2D playerHealthTexture;
	
	public Texture2D tooltipTexture;
	
	private bool alive;
	public Texture2D healthTexture;
	
	//Status
	bool poisoned;
	
	
	private float nextAttack = 0;
	private float currentHealth;
	private float maxHealth = 100;
	
	private bool courageActive;
	private Quaternion originalRotation;
	private bool spinning;
	private Animation a;
	private bool running;

	private float range = 30;
	private bool idling;
	private float pickUpRange = 10;
	
	
	//private float WeaponDamage = 20;
	//private float WeaponSpeed = 1.5f;
	
	private int IconWidth;
	private int IconHeight;
	
	private int debuffWidth = 30;
	private int debuffHeight = 30;
	
	#region Stats
	private int strength;
	
	public int Strength {
		get
		{	
			return this.strength;
		}
		set
		{
			strength = value;
		}
	}
	
	private int dexterity;

	public int Dexterity {
		get {
			return this.dexterity;
		}
		set {
			dexterity = value;
		}
	}	
	
	private int intelligence;
	
	private int vitality;

	public int Intelligence {
		get {
			return this.intelligence;
		}
		set {
			intelligence = value;
		}
	}

	public int Vitality {
		get {
			return this.vitality;
		}
		set {
			vitality = value;
		}
	}	
	
	private float weaponDamage;
	
	private float weaponSpeed;

	public string Name {
		get {
			return this.name;
		}
		set {
			name = value;
		}
	}

	public float WeaponDamage {
		get {
			return this.weaponDamage;
		}
		set {
			weaponDamage = value;
		}
	}

	public float WeaponSpeed {
		get {
			return this.weaponSpeed;
		}
		set {
			weaponSpeed = value;
		}
	}

	private string name;
	#endregion
	
	#region Tooltips
	private string dStrikeTooltip = "Decisive Strike \n Garen becomes invulnerable to \n slows/snares for 1 second, in \n addition his next melee attack will \n deal increased damage in a short \n cone in front of him. ";
	private string courageTooltip = "Courage \n  Garen shields himself, decreasing \n all damage taken by 30% for \n 3 seconds. ";
	private string judgementTooltip = "Judgment \n Garen rapidly spings his sword \n around his body for 3 seconds, \n dealing 180% weapon damage as \n AoE over the duration. ";
	private string demacianJusticeTooltip = "Demacian Justice \n Garen brings down Demacian Justice \n on his opponents, dealing 300% \n weapon damage spread across 5 closest \n enemies (within melee range x 2) \n and stunning for 1.5 seconds.";
	private string mightOfDemaciaTooltip = "Might of Demacia \n Garen attacks targetted enemy for \n weapon damage. Subsequent attacks against \n this target deal 10% increased \n damage (stacks 3 times, max 30% \n increased damage for target attacked 3+ \n consecutive times)";
	private string valorTooltip = "Valor \n Garen cleaves up to 3 \n targets in front of him \n for 50% weapon damage each.";
	#endregion
	
	private EnemyScript currentEnemy;
	
	#region Inventory
	private Item[] inventory;
	private int inventorySize = 30;
	private bool inventoryOpen = false;
	private Rect inventoryRect;
	#endregion
	
	#region Equipment
	private Item[] equipment;
	private bool characterSheetOpen = false;
	private Rect equipmentRect;
	#endregion
	
	private string toolTipString;
	private Color toolTipColor = Color.white;
	private float toolTipExpireTime;
	private float toolTipDelay = 0.3f;
	
	private float stunTime;
	
	void Awake() {
		DontDestroyOnLoad(this);
	}
	
	void OnGUI() {
		
		if(Time.time > toolTipExpireTime){
			toolTipString = null;
			toolTipColor = Color.white;
		}
		
		#region Inventory
		if(inventoryOpen)
			inventoryRect = GUI.Window(0, inventoryRect, inventoryDraw, "Inventory");
		#endregion
		
		if(characterSheetOpen)
			equipmentRect = GUI.Window(1, equipmentRect, equipmentDraw, "Equipment");
	
		#region Spells
		GUI.BeginGroup(new Rect(Screen.width / 2 - (2 * IconWidth), Screen.height - 70, IconWidth * 4, IconHeight * 4));
		
		GUI.Button(new Rect(0, 0, IconWidth, IconHeight), new GUIContent(dStrikeTexture, dStrikeTooltip));
		GUI.Button(new Rect(IconWidth, 0, IconWidth, IconHeight), new GUIContent(courageTexture, courageTooltip));
		GUI.Button(new Rect(IconWidth * 2, 0, IconWidth, IconHeight), new GUIContent(judgementTexture, judgementTooltip));
		GUI.Button(new Rect(IconWidth * 3, 0, IconWidth, IconHeight), new GUIContent(dJusticeTexture, demacianJusticeTooltip));

		GUI.EndGroup();

		
		
		GUI.BeginGroup(new Rect(Screen.width - (2 * IconWidth) - 30, Screen.height - 70, IconWidth * 2, IconHeight * 2));
		GUI.Button(new Rect(0, 0, IconWidth, IconHeight), new GUIContent(mightOfDemaciaTexture, mightOfDemaciaTooltip));
		GUI.Button(new Rect(IconWidth, 0, IconWidth, IconHeight), new GUIContent(valorTexture, valorTooltip));
		GUI.EndGroup();
		
		if(!string.IsNullOrEmpty(GUI.tooltip)){
			toolTipString = GUI.tooltip;
			toolTipColor = Color.white;
			toolTipExpireTime = Time.time + toolTipDelay;
		}
		
		//TODO Cooldown animation for Might of Demacia and Valor? (Weapon Speed)
		
		#endregion
		
		#region Cooldowns
		//Creates mask over buttons based on cooldown.
		
		GUI.BeginGroup(new Rect(Screen.width / 2 - (2 * IconWidth), Screen.height - 70, IconWidth * 4, IconHeight * 4));
		
		GUI.Button(new Rect(0, 0, IconWidth, IconHeight * dStrike.getCooldown()), "");
		GUI.Button(new Rect(IconWidth, 0, IconWidth, IconHeight * courage.getCooldown()), "");
		GUI.Button(new Rect(IconWidth * 2, 0, IconWidth, IconHeight * judgement.getCooldown()), "");
		GUI.Button(new Rect(IconWidth * 3, 0, IconWidth, IconHeight * dJustice.getCooldown()), "");
		
		GUI.EndGroup();
		
		#endregion
		
		#region Health Bars
		//Current Enemy Healthbar
		int buttonLength = 300;
		if(currentEnemy){
		
			GUI.Box(new Rect((Screen.width / 2) - (buttonLength / 2), 40, buttonLength, 30), "");
			
			GUI.BeginGroup(new Rect((Screen.width / 2) - (buttonLength / 2), 40, buttonLength * currentEnemy.getHealthPercent(), 30));
			GUI.Box(new Rect(0, 0, buttonLength,30) , enemyHealthTexture);
			GUI.EndGroup();
			
			GUI.BeginGroup(new Rect((Screen.width / 2) - (buttonLength / 2), 80, buttonLength, 30));
			ArrayList debuffs = currentEnemy.getDebuffs();
			int offsetX = 0;
			foreach(Object debuff in debuffs){
				GUI.Box(new Rect(offsetX, 0, debuffWidth, debuffHeight), ((Debuff)debuff).getTexture());
				offsetX += debuffWidth;
			}
			GUI.EndGroup();
			
		}
		
		//Player Healthbar
			GUI.Box(new Rect((Screen.width / 2) - (buttonLength / 2), Screen.height - 120, buttonLength, 30), "");
			
			GUI.BeginGroup(new Rect((Screen.width / 2) - (buttonLength / 2), Screen.height - 120, buttonLength * getHealthPercent(), 30));
			GUI.Box(new Rect(0, 0, buttonLength,30) , playerHealthTexture);
			GUI.EndGroup();
		#endregion
		
		
		if(!string.IsNullOrEmpty(toolTipString)){
			GUI.Window(3, new Rect (Screen.width / 2 - 450, Screen.height - 130, 300, 125), tooltipWindow, ""); 
		}
		
	}
	
	void inventoryDraw(int windowID){
		int x = 0;
		int y = 20;
		int numButtonsInRow = 6;
		for(int i = 1; i <= inventory.Length; i++){
			Item item = inventory[i-1];
			if(item == null)
				GUI.Button(new Rect(x, y, 50, 50), "");
			else{
				if(GUI.Button(new Rect(x, y, 50, 50), new GUIContent(item.getTexture(), ""+(i-1)))){
					inventory[i-1] = equipItem(item);
				}
			}
			
			if(i % numButtonsInRow == 0 && i != 0){
				y += 50;
				x = 0;
			}
			else
				x += 50;
		}
		
		if(!string.IsNullOrEmpty(GUI.tooltip)){
			int index = int.Parse(GUI.tooltip);
			Item item = inventory[index];
			toolTipString = item.getStats();
			Color[] itemRarityColors = {
			Color.gray, Color.white, Color.green, Color.blue, Color.yellow, Color.red	
			};
			toolTipColor = itemRarityColors[item.getItemRarity()];
			toolTipExpireTime = Time.time + toolTipDelay;
		}
		
		GUI.DragWindow();
	}
	
	void tooltipWindow(int windowID){
		GUIStyle tooltipStyle = new GUIStyle();
		tooltipStyle.fontSize = 14;
		tooltipStyle.normal.textColor = toolTipColor;
		tooltipStyle.alignment = TextAnchor.MiddleCenter;
		GUI.Label(new Rect(0, 0, 300, 125), toolTipString, tooltipStyle);
	}
	
	void equipmentDraw(int windowID){
		
		//Helm
		GUI.Button(new Rect(175, 20, 50, 50), new GUIContent(equipment[0]!=null?equipment[0].getTexture():null, equipment[0]!=null?"0":null));
		
		//Weapon
		GUI.Button(new Rect(120, 120, 50, 50), new GUIContent(equipment[1]!=null?equipment[1].getTexture():null, equipment[1]!=null?"1":null));
		
		//Chest
		GUI.Button(new Rect(175, 120, 50, 50), new GUIContent(equipment[2]!=null?equipment[2].getTexture():null, equipment[2]!=null?"2":null));
		
		//Gloves
		GUI.Button(new Rect(230, 120, 50, 50), new GUIContent(equipment[3]!=null?equipment[3].getTexture():null, equipment[3]!=null?"3":null)); 
		
		//Boots
		GUI.Button(new Rect(175, 220, 50, 50), new GUIContent(equipment[4]!=null?equipment[4].getTexture():null, equipment[4]!=null?"4":null)); 
		
		//Player Stats
		GUIStyle statStyle = new GUIStyle();
		statStyle.fontSize = 11;
		statStyle.alignment = TextAnchor.MiddleCenter;
		GUI.Label(new Rect(5, 20, 100, 300), getStats(), statStyle);
		//Set tooltip window if applicable
		if(!string.IsNullOrEmpty(GUI.tooltip)){
			int Index = int.Parse(GUI.tooltip);
			Item item = equipment[Index];
			toolTipString = item.getStats();
			Color[] itemRarityColors = {
			Color.gray, Color.white, Color.green, Color.blue, Color.yellow, Color.red	
			};
			toolTipColor = itemRarityColors[item.getItemRarity()];
			toolTipExpireTime = Time.time + toolTipDelay;
		}
		
		GUI.DragWindow();
	}
	
	public string getStats(){
		return "Strength: " + strength + "\n" +
			"Dexterity: " + dexterity + "\n" +
			"Intelligence: " + intelligence + "\n" +
			"Vitality: " + vitality + "\n" +
			"Damage: " + weaponDamage.ToString("0.00") + "\n" +
			"Attack Speed: " + weaponSpeed.ToString("0.00") + "\n";
	}
	
	// Use this for initialization
	void Start () {
		
		currentHealth = maxHealth;
		
		//Initialize Abilities - TODO Should this be done statically?
		dStrike = new Decisive_Strike();
		dStrike.setScript(this);
		courage = new Courage();
		courage.setScript(this);
		judgement = new Judgement();
		judgement.setScript(this);
		dJustice = new Demacian_Justice();
		dJustice.setScript(this);
		valor = new Valor();
		valor.setScript(this);
		
		IconWidth = judgementTexture.width;
		IconHeight = judgementTexture.height;
		
		//Initially not running
		running = false;
		
		idling = true;
		
		a = gameObject.GetComponent(typeof(Animation)) as Animation;
		
		originalRotation = transform.localRotation;
		spinning = false;
		
		alive = true;
		
		inventory = new Item[inventorySize];
		
		//TODO This is just for testing. Delete this before submitted!
		for(int i = 0; i < 5; i++){
		TwoHandedSword ths = new TwoHandedSword();
		ths.randomizeWeapon(1);
		inventory[i] = ths;
		}
		
		Helm helm = new Helm();
		helm.randomizeArmor(1);
		inventory[5] = helm;
		
		Chest chest = new Chest();
		chest.randomizeArmor(1);
		inventory[6] = chest;
		
		Gloves gloves = new Gloves();
		gloves.randomizeArmor(1);
		inventory[7] = gloves;
		
		Boots boots = new Boots();
		boots.randomizeArmor(1);
		inventory[8] = boots;
					
		equipment = new Item[5]; //TODO decide on size
		
		equipmentRect = new Rect(50, 50, 300, 300);
		inventoryRect = new Rect(50, 50, 300, Mathf.Ceil(inventorySize / 6) * 50 + 20);
		
		recalculateStats();
	}
	
	// Update is called once per frame
	void Update () {
		
		if(!alive)
			return;
		
		//Open Inventory
		if(Input.GetKeyDown(KeyCode.I))
			inventoryOpen = !inventoryOpen;
		
		//Open Character Sheet
		if(Input.GetKeyDown(KeyCode.C))
			characterSheetOpen = !characterSheetOpen;
		
		//Check for spells being cast
		if(Input.GetKeyDown(KeyCode.Alpha1)){
			dStrike.Execute();
		}
		
		else if(Input.GetKeyDown(KeyCode.Alpha2)){
			courage.Execute();
		}
		
		else if(Input.GetKeyDown(KeyCode.Alpha3)){
			judgement.Execute();
		}
		
		else if(Input.GetKeyDown(KeyCode.Alpha4)){
			dJustice.Execute();
		}
		
		else if(Input.GetButtonDown("Fire2")){ //Right click
			valor.Execute();
		}
		
		if(running && !a.IsPlaying("Run") && !a.IsPlaying("Spell3")) {
			a.Stop();
			a.Play("Run");
			idling = false;
		}
		else if (!a.isPlaying)
			a.Play("Idle1");
		
		//Update all spells
		courage.Update();
		judgement.Update();
		
		//Pick up any items within range
		Collider[] items = Physics.OverlapSphere(gameObject.transform.position, pickUpRange);
		foreach(Collider itemCollider in items){
			
			PickUp item	= itemCollider.gameObject.GetComponent(typeof(HealthOrbScript)) as PickUp;
			if(item != null)
				item.trigger(this);
			
			NoxiousBlastScript noxiousBlast = itemCollider.gameObject.GetComponent(typeof(NoxiousBlastScript)) as NoxiousBlastScript;
			if(noxiousBlast)
				noxiousBlast.trigger(this);
			
			MiasmaScript miasma = itemCollider.gameObject.GetComponent(typeof(MiasmaScript)) as MiasmaScript;
			if(miasma)
				miasma.trigger(this);
		}
	
	}
	
	public void awardItem(Item item){
		for(int i = 0; i < inventory.Length; i++)
			if(inventory[i] == null){
			inventory[i] = item;
			return;
		}
	}
	
	public bool stunned(){
		return Time.time < stunTime;	
	}
	
	private Item equipItem(Item item){
		System.Type[] types = new System.Type[5];
		types[0] = typeof(Helm);
		types[1] = typeof(TwoHandedSword);
		types[2] = typeof(Chest);
		types[3] = typeof(Gloves);
		types[4] = typeof(Boots);
		
		int index = System.Array.IndexOf(types, item.GetType());
		
		if(index != -1){
			Item toReturn = equipment[index];
			equipment[index] = item;
			recalculateStats();
			return toReturn;
		}
		
		return item;
	}
	
	public bool guiInteraction(Vector3 mousePosition){
		//Y position is inversed
		mousePosition.y = Screen.height - mousePosition.y;
		
		if(inventoryOpen)
			if(inventoryRect.Contains(mousePosition))
				return true;
			
		if(characterSheetOpen)
			if(equipmentRect.Contains(mousePosition))
				return true;
		
		return false;
	}
	
	private void recalculateStats(){
		//Restore base stats
		strength = 10;
		dexterity = 7;
		intelligence = 7;
		vitality = 10;
		weaponDamage = 5;
		weaponSpeed = 1.5f;
		maxHealth = 100;
		
		//Add each item
		foreach(Item item in equipment){
			if(item == null)
				continue;
			if(item.GetType().GetInterface("Weapon") != null){
				Weapon weapon = (Weapon) item;
				this.strength += weapon.Strength;
				this.intelligence += weapon.Intelligence;
				this.dexterity += weapon.Dexterity;
				this.vitality += weapon.Vitality;
				this.weaponDamage += weapon.WeaponDamage;
				this.weaponSpeed = weapon.WeaponSpeed;
			}
			else if(item.GetType().GetInterface("Armor") != null){
				Armor armor = (Armor) item;
				this.strength += armor.Strength;
				this.intelligence += armor.Intelligence;
				this.dexterity += armor.Dexterity;
				this.vitality += armor.Vitality;
			}
		}
		
		//Class specific enhancements based on stats
		this.weaponDamage += this.strength;
		this.maxHealth = vitality * 10;
	}
	
	public void playIdleSequence(){
		if(!a.isPlaying){
			int num = ((int) (Random.value * 4)) + 1;
			a.Play("Idle"+num);
		}
		
		idling = true;
	}
	
	public bool isRunAnimation(){
		return a.IsPlaying("Run");	
	}
	
	public bool noAnimation(){
		return !a.isPlaying;	
	}
	
	public bool isAlive(){
		return alive;
	}
	
	public void stopAnimation(){
		a.Stop();	
	}
	
	public float getHealthPercent(){
		return currentHealth / maxHealth;
	}
	
	public void setCurrentEnemy(EnemyScript enemy){
		this.currentEnemy = enemy;	
	}
	
	public void setIdling(bool idle){
		idling = idle;
	}

	public EnemyScript getCurrentEnemy(){
		return currentEnemy;	
	}
	
	public float getWeaponDamage(){
		return WeaponDamage;	
	}
	

	public float getWeaponSpeed(){
		return WeaponSpeed;
	}
	

	public void playAnimation(string animationName){
		a.Stop();
		a.Play(animationName);
	}
	
	public void setRunning(bool active){
		running = active;
	}
	
	public void setSpinning(bool active){
		string animationName = active?"Spell3":"Idle2";
		spinning = active;
			a.Stop();
			a.Play(animationName);
	}
	
	public bool isSpinning(){
		return spinning;	
	}
	
	public void activateCourage(bool active){
			courageActive = active;
	}
	
	public float getRange(){
		return range;
	}

	public void awardHealth(float amount){
		//Calculate additional healing based on passive or skills
		currentHealth += amount;
		if(currentHealth > maxHealth)
			currentHealth = maxHealth;
	}

	public void damage(float amount){
		//Compute damage reductions
		if(courageActive)
			amount *= 0.7f;
		
		//Apply damage
		currentHealth -= amount;
		if(currentHealth < 0){
			currentHealth = 0;
			alive = false;
			a.Stop();
			a.Play("Death");
		}
		
		//Check for death
	}
	
	public void setPoisoned(bool poisoned){
		this.poisoned = poisoned;
		Debug.Log("Player has been poisoned!");
	}
	
	public bool isPoisoned(){
		return poisoned;
	}
	
	public GameObject getGameObject(){
		return gameObject;	
	}
	
	/*
	 * Returns the time of the next auto-attack
	 * */
	public float autoAttack(EnemyScript enemy){
			a.Play("Attack" + (((int)(Random.value * 3)) + 1));
			enemy.damage(WeaponDamage);
			ValorDebuff debuff = new ValorDebuff();
			debuff.setTexture(valorDebuffTexture);
			debuff.refresh();
			enemy.applyDebuff(debuff, true);
			stunTime = Time.time + 1f; //Delay after attack
			return Time.time + WeaponSpeed;
	}
	
	
	class Decisive_Strike : Ability {
		
		private GarenScript player;
		private double totalDamage;
		private float nextAttack = 0;
		private float coolDown = 5;
		private string animationName = "Spell1";
		
		
		public void Execute(){
			
			totalDamage = player.getWeaponDamage() * 1.6f;
			
			//Check if the ability is on cooldown
			if(nextAttack > Time.time)
				return;
			
			player.playAnimation(animationName);
			/*
			 * To find enemies in a cone, we first use Physics.OverlapSphere to find all enemies close to our position.
			 * We then iterate through each enemy, and use the dot product of the direction an forward vectors to determine
			 * if the enemy is in the cone.
			 * */
			Vector3 position = player.gameObject.transform.position;
			Collider[] enemies = Physics.OverlapSphere(position, 30);
			foreach(Collider enemyCol in enemies){
				EnemyScript enemy = enemyCol.gameObject.GetComponent(typeof(EnemyScript)) as EnemyScript;
				if(enemy){
					Vector3 direction = Vector3.Normalize(enemy.gameObject.transform.position - player.gameObject.transform.position);
					float dot = Vector3.Dot(direction, player.gameObject.transform.forward);
					if(dot > 0.707f)
						enemy.damage((float)totalDamage);
				}
			}
			
			nextAttack = Time.time + coolDown;
		}
		
		public void setScript(GarenScript script){
			player = script;		
		}
		
		public float getCooldown(){
			if(Time.time > nextAttack)
				return 0;
			else
				return (float)((nextAttack - Time.time) / coolDown);
		}
		
	}
	
	class Courage : Ability {
		private GarenScript player;
		private bool active = false;
		private float cooldown = 5;
		private float nextAttack = 0;
		private string animationName = "Spell2";
	
		
		public void Update(){
			if(nextAttack < Time.time && active){
				active = false;
				player.activateCourage(false);
				Debug.Log("Courage finished!");
			}
		}
		
		public void Execute(){
			
			//Check if on cooldown
			if(Time.time < nextAttack)
				return;
			
			player.activateCourage(true);
			nextAttack = Time.time + cooldown;
			player.playAnimation(animationName);
			active = true;
			Debug.Log("Courage active!");
		}
		
		public void setScript(GarenScript script){
			player = script;
		}
		
		public float getCooldown(){
			if(Time.time > nextAttack)
				return 0;
			else
				return (float)((nextAttack - Time.time) / cooldown);
		}
	}
	
	/*
	 * To simulate damage over time, judgement uses a variable called "numTicks". While numTicks > 0, every
	 * second nearby enemies are damage for totalDamage / numTicks damage (calculated when numTicks is at its original value),
	 * and numTicks is decremented by one. This allows for even distribution of damage over time, and ensures that enemies are 
	 * able to "run away" from the damage zone.
	 * */
	class Judgement : Ability {
		private GarenScript player;
		private float nextTickTime = 0;
		private int radius = 30;
		private int numTicks = 0;
		private double totalDamage;
		private double damagePerTick = 0;
		private double nextAttack = 0;
		private double coolDown = 10;
		public void Execute(){
			totalDamage = 1.8f * player.getWeaponDamage();
			
			//If this ability is on cooldown, don't do anything.
			//TODO Play "On Cooldown" sound?
			if(nextAttack > Time.time)
				return;
			
			numTicks = 3;
			damagePerTick = totalDamage / (double)numTicks; //TODO Calculate damage using equipment and skills
			nextAttack = Time.time + coolDown; //Put ability on cooldown
			
			//SPIN TO WIN!!!!! (animation)
			player.setSpinning(true);
		}
		
		public void setScript(GarenScript script){
			player = script;
		}
		
		public void Update(){
			
			//If there are still ticks of damage to be applied, and it's time for another tick.
			if(numTicks > 0 && Time.time > nextTickTime){
				Vector3 position = player.gameObject.transform.position;
				Collider[] targets = Physics.OverlapSphere(position, radius);
				foreach(Collider t in targets){
					EnemyScript enemy = t.gameObject.GetComponent(typeof(EnemyScript)) as EnemyScript;
					if(enemy){
						enemy.damage((float)damagePerTick);
					}
				}
				
				nextTickTime = Time.time + 1;
				numTicks--;
			}
			
			//If the player is spinning, but no damage can be applied, stop spinning.
			else if(numTicks <= 0 && player.isSpinning()){
				player.setSpinning(false);
			}
		}
		
		public float getCooldown(){
			if(Time.time > nextAttack)
				return 0;
			else
				return (float)((nextAttack - Time.time) / coolDown);
		}
	}
	
	class Demacian_Justice : Ability {
		private GarenScript player;
		private float cooldown = 120;
		private float nextAttack = 0;
		private int numPossibleTargets = 5;
		private int radius = 60;
		private string animationName = "Spell4";
		
		public void Execute(){
			
			//If on cooldown, return
			if(Time.time < nextAttack)
				return;
			
			player.playAnimation(animationName);
			
			nextAttack = Time.time + cooldown;
			Vector3 position = player.gameObject.transform.position;
			EnemyScript[] enemies = new EnemyScript[numPossibleTargets];
			int index = 0;
			Collider[] targets = Physics.OverlapSphere(position, radius);
			foreach(Collider t in targets){
				EnemyScript enemy = t.gameObject.GetComponent(typeof(EnemyScript)) as EnemyScript;
				if(enemy){
				enemies[index++] = enemy;
				if(index > numPossibleTargets - 1) //If the maximum number of targets has been reached, stop adding new ones.
					break;
					}
				}
			
			//If there are no enemies to hit, return
			if(index == 0) 
				return;
			
			float damagePerTarget = (player.WeaponDamage * 3.0f) / index;
			foreach(EnemyScript enemy in enemies){
				if(enemy){
					enemy.damage(damagePerTarget);
					enemy.stun(1.5f);
				}
			}
		}
		
		public void setScript(GarenScript script){
			player = script;
		}
		
		public float getCooldown(){
			if(Time.time > nextAttack)
				return 0;
			else
				return (float)((nextAttack - Time.time) / cooldown);
		}
	}
	
	class Valor : Ability {
		private GarenScript player;
		private float nextAttack = 0;
		private int numPossibleTargets = 3;
		private int radius = 30;
		private string animationName = "Attack2";
		
		public void Execute(){
			
			//If on cooldown, return
			if(Time.time < nextAttack)
				return;
			
			player.playAnimation(animationName);
			
			nextAttack = Time.time + player.WeaponSpeed;
			Vector3 position = player.gameObject.transform.position;
			EnemyScript[] enemies = new EnemyScript[numPossibleTargets];
			int index = 0;
			Collider[] targets = Physics.OverlapSphere(position, radius);
			foreach(Collider t in targets){
				EnemyScript enemy = t.gameObject.GetComponent(typeof(EnemyScript)) as EnemyScript;
				if(enemy){
					Vector3 direction = Vector3.Normalize(enemy.gameObject.transform.position - player.gameObject.transform.position);
					float dot = Vector3.Dot(direction, player.gameObject.transform.forward);
					if(dot > 0.707f){
						enemies[index++] = enemy;
						if(index > numPossibleTargets - 1) //If the maximum number of targets has been reached, stop adding new ones.
						break;
					}
				}
			}
			
			//If there are no enemies to hit, return
			if(index == 0) 
				return;
			
			float damagePerTarget = player.WeaponDamage * 0.5f;
			foreach(EnemyScript enemy in enemies){
				if(enemy)
					enemy.damage(damagePerTarget);
			}
		}
		
		public void setScript(GarenScript script){
			player = script;
		}
		
		public float getCooldown(){
			if(Time.time > nextAttack)
				return 0;
			else
				return (float)((nextAttack - Time.time) / player.getWeaponSpeed());
		}
	}
}
