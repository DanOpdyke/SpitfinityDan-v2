/*
 * Filename: VayneScript.cs
 * 
 * Author:
 * 		Programming: David Spitler, Daniel Opdyke
 * 		Character Design: David Spitler
 * 
 * Last Modified: 6/28/2012
 * 		By: David Spitler
 * 
 * 
 * NOTE: All Models, Original Character Concepts, and Icons are property of Riot Games.
 * */
using UnityEngine;
using System.Collections;

/// <summary>
/// VayneScript representing the Vayne class. Vayne is a ranged champion from League of Legends,
/// who utilizes mobility and single target damage in combat. Vayne has been equipped with modified
/// variations of her League of Legends abilities, as well as two new, unique abilities.
/// </summary>
public class VayneScript : MonoBehaviour, PlayerScript {
	
	#region Spell Objects
	/// <summary>
	/// The Tumble ability object used to determine cooldown and execute ability.
	/// </summary>
	private Tumble tumble;
	
	/// <summary>
	/// The Silver Bolts ability object used to determine cooldown and execute ability.
	/// </summary>
	private Silver_Bolts silverBolts;
	
	/// <summary>
	/// The Condemn ability object used to determine cooldown and execute ability.
	/// </summary>
	private Condemn condemn;
	
	/// <summary>
	/// The Final Hour ability object used to determine cooldown and execute ability.
	/// </summary>
	private Final_Hour finalHour;
	
	/// <summary>
	/// The Shadow Bolt ability object used to determine cooldown and execute ability.
	/// </summary>
	private ShadowBolt shadowBolt;
	
	/// <summary>
	/// The Auto-Attack ability object used to determine cooldown and execute ability.
	/// </summary>
	public GameObject boltObject;
	
	#endregion
	
	#region Spell Textures
	/// <summary>
	/// The Tumble ability texure.
	/// </summary>
	public Texture2D tumbleTexture;
	
	/// <summary>
	/// The Shadow Bolts ability texure.
	/// </summary>
	public Texture2D sBoltsTexture;
	
	/// <summary>
	/// The Condemn ability texure.
	/// </summary>
	public Texture2D condemnTexture;
	
	/// <summary>
	/// The Final Hour ability texure.
	/// </summary>
	public Texture2D fHourTexture;
	
	/// <summary>
	/// The Swift Death ability texure.
	/// </summary>
	public Texture2D swiftDeathTexture;
	
	/// <summary>
	/// The Shadow Bolt ability texure.
	/// </summary>
	public Texture2D shadowBoltTexture;
	
	/// <summary>
	/// The overlay of a mouse with the left button highlighted, indicating that the
	/// skill may be activated by pressing the left mouse button.
	/// </summary>
	private Texture2D leftClickOverlay;
	
	/// <summary>
	/// The overlay of a mouse with the right button highlighted, indicating that the
	/// skill may be activated by pressing the right mouse button.
	/// </summary>
	private Texture2D rightClickOverlay;
	
	#endregion
	
	#region Health Bar and Mana Bar Textures
	
	/// <summary>
	/// The texture to use for Enemy health bars, shown above each enemy.
	/// </summary>
	public Texture2D enemyHealthTexture;
	
	/// <summary>
	/// The texture to use for the Player's health bar, shown in the center
	/// of the screen. This texture should never be the same as the Enemy health bar
	/// texture.
	/// </summary>
	public Texture2D playerHealthTexture;
	
	/// <summary>
	/// The texture to use for the Player's mana bar, shown in the center
	/// of the screen. This texture should never be the same as the Enemy mana bar
	/// texture.
	/// </summary>
	private Texture2D playerManaTexture;
	
	#endregion
	
	/// <summary>
	/// Player's current living status. When set to false, the Player is unable to perform actions,
	/// and the game is terminated.
	/// </summary>
	private bool alive;
	
	/// <summary>
	/// The texture for the current selected enemy health bar, shown in the top center of the
	/// screen. Generally the same texture as the "enemyHealthTexture".
	/// </summary>
	/// <see cref="enemyHealthTexture"/>
	public Texture2D healthTexture;
	
	/// <summary>
	/// Boolean indicating if the Player is currently poisoned. Used primarily to determine if certain
	/// enemy abilities are amplified. The actual damage of the poison should be applied using a debuff.
	/// </summary>
	private bool poisoned;
	
	/// <summary>
	/// Hashtable matching debuff names to debuff objects.
	/// </summary>
	private Hashtable debuffs = new Hashtable();
	
	/// <summary>
	/// The time at which the Player may next attack.
	/// </summary>
	private float nextAttack = 0;
	
	/// <summary>
	/// The current health of the Player. When current health reaches 0, the Player is dead.
	/// </summary>
	private float currentHealth;
	
	/// <summary>
	/// The maximum health of the Player, calculated using the Vitality statistic.
	/// </summary>
	private float maxHealth = 100;
	
	/// <summary>
	/// The current mana of the Player.
	/// </summary>
	private float currentMana;
	
	/// <summary>
	/// The maximum mana of the Player.
	/// </summary>
	private float maxMana = 100;
	
	/// <summary>
	/// The rate at which a Player will regenerate mana in and out of combat.
	/// </summary>
	private float manaRegenRate = 1f;
	
	/// <summary>
	/// The rate at which a Player will regenerate health when out of combat. In future
	/// version of the game, buffs and debuffs will be added to modify this value.
	/// </summary>
	private float healthRegenRate = 1f;
	
	/// <summary>
	/// The last time the Player either attacked, or took damage. Used to determine
	/// when health should begin regenerating.
	/// </summary>
	private float lastCombatTime;
	
	/// <summary>
	/// The amount of time to wait before health may start regenerating after combat.
	/// </summary>
	private float healthRegenDelay = 5f;
	
	/// <summary>
	/// Determines if the Final Hour Buff is active.
	/// </summary>
	private bool finalHourActive;
	
	/// <summary>
	/// The original rotation of the Player.
	/// </summary>
	private Quaternion originalRotation;
	
	/// <summary>
	/// Determines if the Tumble Buff is currently active.
	/// </summary>
	private bool tumbled;
	
	/// <summary>
	/// The Player's animation object.
	/// </summary>
	private Animation a;
	
	/// <summary>
	/// Boolean indicating if the Player is currently running. Running determines which animations
	/// should be played, and should only be set if the Player is currently moving.
	/// </summary>
	private bool running;
	
	/// <summary>
	/// The Player's attack range.
	/// </summary>
	private float range = 80;
	
	/// <summary>
	/// Boolean indicating if the Player is currently idling. If true, the Player will begin to play
	/// idle animations after a few seconds.
	/// </summary>
	private bool idling;
	
	/// <summary>
	/// The minimal range requirement for the Player to automatically pick items up off the ground.
	/// </summary>
	private float pickUpRange = 20;
	
	/// <summary>
	/// The text of the current tooltip. This text is stored as a work around to the Unity built-in
	/// tooltip system, in order to display tooltips outside of the bounds of a GUI.Window.
	/// </summary>
	private string toolTipString;
	
	/// <summary>
	/// The color of the tooltip text. Modified by the rarity of an Item.
	/// </summary>
	private Color toolTipColor = Color.white;
	
	/// <summary>
	/// The time at which the tooltip should cease being displayed.
	/// </summary>
	private float toolTipExpireTime;
	
	/// <summary>
	/// The amount of delay time that a tooltip should appear after the mouse is no longer highlighted
	/// over an Item or ability. Primarily implemented to resolve issues with tooltips being incorrectly
	/// "shared" between different Items.
	/// </summary>
	private float toolTipDelay = 0.3f;
	
	/// <summary>
	/// The time until which the Player is stunned.
	/// </summary>
	private float stunTime;
	
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
	
	/// <summary>
	/// Movement speed of the Player, altered by buffs and debuffs.
	/// </summary>
	private float movementSpeed;

	public float MovementSpeed {
		get {
			return this.movementSpeed;
		}
		set {
			movementSpeed = value;
		}
	}

	private string name;
	
	/// <summary>
	/// Level of the Player. Every time the Player successfully kills Cassiopeia, their level increases by 1.
	/// Mobs become exponentially more difficult as the Player's level increases.
	/// </summary>
	private int level;

	public int Level {
		get {
			return this.level;
		}
		set {
			level = value;
		}
	}
	#endregion
	
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
	
	/// <summary>
	/// The width of each ability texture icon.
	/// </summary>
	private int IconWidth;
	
	/// <summary>
	/// The height of each ability texture icon.
	/// </summary>
	private int IconHeight;
	
	#region Tooltips
	/// <summary>
	/// The Courage tooltip text.
	/// </summary>
	private string tumbleTooltip = "Tumble \n Vayne quickly rolls toward the cursor \n and enhances her next attack \n to deal 30% percent \n of her weapon as bonus damage. \n Cost: 30 Mana ";
	
	/// <summary>
	/// The Decisive Strike tooltip text.
	/// </summary>
	private string sBoltsTooltip = "Silver Bolts \n Vayne quickly attacks target enemy \n 3 times. The third attack \n deals bonus damage (60% weapon \n damage), and snares the target \n for 1.5 seconds. \n Cost: 50 Mana";
	
	/// <summary>
	/// The Judgement tooltip text.
	/// </summary>
	private string condemnTooltip = "Condemn \n Vayne fires a huge bolt at \n her target, dealing 100% weapon \n damage and knocking them back. \n If they collide with terrain \n after being knocked back, they \n are dealt the same amount \n of damage again and are \n stunned for 2 seconds. \n Cost: 60 Mana";
	
	/// <summary>
	/// The Demacian Justice tooltip text.
	/// </summary>
	private string fHourTooltip = " Final Hour \n Vayne enters a heightened state \n of prowess, for 8 seconds where \n she deals 40% bonus damage, heals \n for 50% of her damage caused, \n and enters stealth for 1 \n second after using Tumble. \n Cost: 80 mana";
	
	private string swiftDeathTooltip = "Swift Death \n Vayne's attacks cause a bleed \n on the target, dealing 10% \n weapon damage over 5 seconds. \n The bleed will refresh with \n each attack, but does \n not stack. \n Generates 10 Mana";
	
	private string shadowBoltTooltip = "Shadow Bolt \n Vayne fires a number of bolts \n attacking all targets in a cone \n in front of her for 70% \n weapon damage, applying the \n bleed from Swift Death. \n Cost: 40 Mana ";
	
	#endregion
	
	/// <summary>
	/// The width of the debuff icon below the current enemy's health bar.
	/// </summary>
	private int debuffWidth = 30;
	
	/// <summary>
	/// The height of the debuff icon below the current enemy's health bar.
	/// </summary>
	private int debuffHeight = 30;
	
	/// <summary>
	/// The time delay between when the checkpointTexture is shown, and
	/// when it begins to fade.
	/// </summary>
	private float checkpointTextureDelay = 1.5f;
	
	/// <summary>
	/// The time at which the checkpoint texture will begin to fade.
	/// </summary>
	private float checkpointTextureFadeTime;
	
	/// <summary>
	/// The alpha value of the checkpoint texture, used to fade the texture out.
	/// When the alpha is 0, the texture is no longer drawn.
	/// </summary>
	private float checkpointTextureCurrentAlpha;
	
	/// <summary>
	/// The texture used to display when a Player has reached a checkpoint.
	/// </summary>
	private Texture2D checkpointTexture;
	
	/// <summary>
	/// The height of the tooltip window.
	/// </summary>
	private float tooltipWindowHeight = 0;
	
	/// <summary>
	/// The width of the tooltip window.
	/// </summary>
	private float tooltipWindowWidth = 0;
	
	/// <summary>
	/// Determines if the next attack should have damage modified by tumble.
	/// </summary>
	private bool tumbleAttack;
	
	/// <summary>
	/// The index of the last item that was left clicked. This is used to determine is a Player
	/// is attempting to destroy an item.
	/// </summary>
	private int lastInventoryIndex;
	
	/// <summary>
	/// The time of the last left click on an inventory item. Used to determine a quick "double
	/// click" of an item for deletion.
	/// </summary>
	private float lastInventoryClick;
	
	/// <summary>
	/// Determines if the item deletion confirmation dialogue should be shown.
	/// </summary>
	private bool confirmPopup;
	
	/// <summary>
	/// The location Rect of the item deletion confirmation dialogue.
	/// </summary>
	private Rect confirmRect;
	
	/// <summary>
	/// The location Rect of the continue button when the game is either won or lost.
	/// </summary>
	private Rect continueRect;
	
	/// <summary>
	/// The portrait of the Player displayed when a checkpoint is reached.
	/// </summary>
	private Texture2D playerPortrait;
	
	
	/// <summary>
	/// The texture displayed when the Player successfully kills Cassiopeia.
	/// </summary>
	private Texture2D winTexture;
	
	/// <summary>
	/// The texture displayed when the Player is killed.
	/// </summary>
	private Texture2D lossTexture;
	
	/// <summary>
	/// The texture on the continue button when it is not highlighted.
	/// </summary>
	private Texture2D continueButton;
	
	/// <summary>
	/// The texture on the continue button when it is highlighted.
	/// </summary>
	private Texture2D continueButtonHighlighted;
	
	/// <summary>
	/// Determines if the game has ended due to the death of Cassiopeia.
	/// </summary>
	private bool gameWon;
	
	/// <summary>
	/// Determines if the game has ended due to the death of the Player.
	/// </summary>
	private bool gameLoss;
	
	/// <summary>
	/// The Player's current Enemy. Used both as the target of certain attacks, as well as 
	/// displaying the health bar at the top center of the screen.
	/// </summary>
	private EnemyScript currentEnemy;
	
	/// <summary>
	/// Draws the game GUI, including spells icons, cooldowns, health bars, and Items.
	/// </summary>
	void OnGUI() {
		if(Time.time > toolTipExpireTime){
			toolTipString = null;
			toolTipColor = Color.white;
		}
		
		GUIStyle labelStyle = new GUIStyle();
		labelStyle.alignment = TextAnchor.UpperCenter;
		labelStyle.fontSize = 20;
		labelStyle.normal.textColor = Color.white;
		
		GUIStyle cooldownNumberStyle = new GUIStyle();
		cooldownNumberStyle.alignment = TextAnchor.MiddleCenter;
		cooldownNumberStyle.fontSize = 20;
		cooldownNumberStyle.normal.textColor = Color.white;
		
		#region Inventory
		if(inventoryOpen)
			inventoryRect = GUI.Window(0, inventoryRect, inventoryDraw, "Inventory");
		#endregion
		
		if(characterSheetOpen)
			equipmentRect = GUI.Window(1, equipmentRect, equipmentDraw, "Equipment");
		
		GUI.BeginGroup(new Rect(Screen.width / 2 - (2 * IconWidth), Screen.height - 98, IconWidth * 4, IconHeight * 4));
		
		GUI.Button(new Rect(0, 0, IconWidth, IconHeight), new GUIContent(tumbleTexture, tumbleTooltip));
		GUI.Label(new Rect(IconWidth * (2.8f/4), IconHeight * (2.8f/4), IconWidth / 4, IconHeight / 4), "1", cooldownNumberStyle);
		GUI.Button(new Rect(IconWidth, 0, IconWidth, IconHeight), new GUIContent(sBoltsTexture, sBoltsTooltip));
		GUI.Label(new Rect(IconWidth + (IconWidth * (2.8f/4)), IconHeight * (2.8f/4), IconWidth / 4, IconHeight / 4), "2", cooldownNumberStyle);
		GUI.Button(new Rect(IconWidth * 2, 0, IconWidth, IconHeight), new GUIContent(condemnTexture, condemnTooltip));
		GUI.Label(new Rect(2 * IconWidth + (IconWidth * (2.8f/4)), IconHeight * (2.8f/4), IconWidth / 4, IconHeight / 4), "3", cooldownNumberStyle);
		GUI.Button(new Rect(IconWidth * 3, 0, IconWidth, IconHeight), new GUIContent(fHourTexture, fHourTooltip));
		GUI.Label(new Rect(3 * IconWidth + (IconWidth * (2.8f/4)), IconHeight * (2.8f/4), IconWidth / 4, IconHeight / 4), "4", cooldownNumberStyle);
		GUI.EndGroup();
		
		GUI.BeginGroup(new Rect(Screen.width - (2 * IconWidth) - 30, Screen.height - 78, IconWidth * 2, IconHeight * 2));
		GUI.Button(new Rect(0, 0, IconWidth, IconHeight), new GUIContent(swiftDeathTexture, swiftDeathTooltip));
		GUI.Label(new Rect((1 * IconWidth) / 1.5f, IconHeight / 2.7f, IconWidth, IconHeight / 1.5f), leftClickOverlay);
		GUI.Button(new Rect(IconWidth, 0, IconWidth, IconHeight), new GUIContent(shadowBoltTexture, shadowBoltTooltip));
		GUI.Label(new Rect((2.5f * IconWidth) / 1.5f, IconHeight / 2.7f, IconWidth, IconHeight / 1.5f), rightClickOverlay);
		GUI.EndGroup();
		
		if(!string.IsNullOrEmpty(GUI.tooltip)){
			toolTipString = GUI.tooltip;
			toolTipColor = Color.white;
			toolTipExpireTime = Time.time + toolTipDelay;
		}
		
		//Creates mask over buttons based on cooldown.
		
		GUI.BeginGroup(new Rect(Screen.width / 2 - (2 * IconWidth), Screen.height - 98, IconWidth * 4, IconHeight * 4));
		
		GUI.Button(new Rect(0, 0, IconWidth, IconHeight * tumble.getCooldown()), "");
		GUI.Button(new Rect(IconWidth, 0, IconWidth, IconHeight * silverBolts.getCooldown()), "");
		GUI.Button(new Rect(IconWidth * 2, 0, IconWidth, IconHeight * condemn.getCooldown()), "");
		GUI.Button(new Rect(IconWidth * 3, 0, IconWidth, IconHeight * finalHour.getCooldown()), "");
		
		GUI.EndGroup();
		
		
		#region Health Bars and Buffs
		//Current Enemy Healthbar
		int buttonLength = 300;
		int offsetX;
		if(currentEnemy){
		
			GUI.Box(new Rect((Screen.width / 2) - (buttonLength / 2), 40, buttonLength, 30), "");
			
			GUI.BeginGroup(new Rect((Screen.width / 2) - (buttonLength / 2), 40, buttonLength * currentEnemy.getHealthPercent(), 30));
			GUI.Box(new Rect(0, 0, buttonLength,30) , enemyHealthTexture);
			GUI.EndGroup();
			
			//Enemy Debuffs
			GUI.BeginGroup(new Rect((Screen.width / 2) - (buttonLength / 2), 80, buttonLength, 30));
			Hashtable enemyDebuffs = currentEnemy.getDebuffs();
			offsetX = 0;
			string[] keys = new string[enemyDebuffs.Keys.Count];
			enemyDebuffs.Keys.CopyTo(keys, 0);
		
			//Check for expired debuffs
			foreach(string obj in keys){
				Debuff debuff = (Debuff) enemyDebuffs[obj];
				GUI.Box(new Rect(offsetX, 0, debuffWidth, debuffHeight), new GUIContent(debuff.getTexture(), debuff.description()));
				offsetX += debuffWidth;
			}
			GUI.EndGroup();
			
			if(!string.IsNullOrEmpty(GUI.tooltip)){
			toolTipString = GUI.tooltip;
			toolTipColor = Color.white;
			toolTipExpireTime = Time.time + toolTipDelay;
			}
			
		}
		
		//Player Healthbar and Manabar
			GUI.Box(new Rect((Screen.width / 2) - (buttonLength / 2), Screen.height - 131, buttonLength, 30), "");
			
			GUI.BeginGroup(new Rect((Screen.width / 2) - (buttonLength / 2), Screen.height - 131, buttonLength * getHealthPercent(), 30));
			GUI.Box(new Rect(0, 0, buttonLength,30) , playerHealthTexture);
			GUI.EndGroup();
			GUI.Label(new Rect((Screen.width / 2) - (buttonLength / 2), Screen.height - 129, buttonLength, 30), ((int)currentHealth + "/" + (int)maxHealth), labelStyle);		
			GUI.Box(new Rect((Screen.width / 2) - (buttonLength / 2), Screen.height - 30, buttonLength, 30), "");
			
			GUI.BeginGroup(new Rect((Screen.width / 2) - (buttonLength / 2), Screen.height - 30, buttonLength * getManaPercent(), 30));
			GUI.Box(new Rect(0, 0, buttonLength,30) , playerManaTexture);
			GUI.EndGroup();
			GUI.Label(new Rect((Screen.width / 2) - (buttonLength / 2), Screen.height - 28, buttonLength, 30), (int) currentMana + "/" + (int) maxMana, labelStyle);
		
		//Player Debuffs
			GUI.BeginGroup(new Rect((Screen.width / 2) - (buttonLength / 2), Screen.height - 180, buttonLength, 30));
			offsetX = 0;
			foreach(Object debuffObject in debuffs.Values){
				Debuff debuff = (Debuff) debuffObject;
				GUI.Box(new Rect(offsetX, 0, debuffWidth, debuffHeight), new GUIContent(debuff.getTexture(), debuff.description()));
				offsetX += debuffWidth;
			}
			GUI.EndGroup();
		
		if(!string.IsNullOrEmpty(GUI.tooltip)){
			toolTipString = GUI.tooltip;
			toolTipColor = Color.white;
			toolTipExpireTime = Time.time + toolTipDelay;
		}
		#endregion
		
		if(confirmPopup)
			confirmRect = GUI.Window(4, confirmRect, confirmDraw, "Confirm");
		
		
		int numCharsPerLine = 37;
		
		// .01 screen height per line
		
		tooltipWindowHeight = 0;
		tooltipWindowWidth = 0;
		if(!string.IsNullOrEmpty(toolTipString)){
			string temp = toolTipString;
//			Debug.Log("Length : " + toolTipString.Length);
			int numLines = Mathf.CeilToInt(toolTipString.Length / 37.0f);
			while(temp.Contains("\n")){
				//Store longest line width
				if(temp.IndexOf("\n") > tooltipWindowWidth)
					tooltipWindowWidth = temp.IndexOf("\n");
				temp = temp.Substring(temp.IndexOf("\n") + 1);
				numLines++;
			}
			if(tooltipWindowWidth == 0)
				tooltipWindowWidth = toolTipString.Length;
			
			tooltipWindowWidth *= (Screen.width * 0.0050f);
			
			numLines += 4;
			tooltipWindowHeight = (Screen.height / 100) * numLines;
			GUI.Window(3, new Rect ((Screen.width / 2) - (tooltipWindowWidth) - (buttonLength / 2) - 10,  Screen.height - 170, tooltipWindowWidth, tooltipWindowHeight), tooltipWindow, ""); 
		}
		
		#region End of Game Textures
		
		GUIStyle continueButtonStyle = new GUIStyle();
		continueButtonStyle.normal.background = null;
		
		if(gameWon){
			GUI.Label(new Rect((Screen.width / 2) - (winTexture.width / 2), Screen.height / 10, winTexture.width, winTexture.height), winTexture);
			Vector3 mousePosition = Input.mousePosition;
			mousePosition.y = Screen.height - mousePosition.y;
			if(continueRect.Contains(mousePosition)){
				if(GUI.Button(continueRect, continueButtonHighlighted, continueButtonStyle)){
					//Simulate restart of level while keeping gear
					GameObject topLevelObject = GameObject.FindGameObjectWithTag("TopLevelObject");
					topLevelObject.transform.position = new Vector3(-144.2164f, 5.702944f, -365.1236f);
					reinitializeLevel();
					gameWon = false;
					level++;
					Save();
				}
			}
			else{
				if(GUI.Button(continueRect, continueButton, continueButtonStyle)){
				}
			}
		}
		else if(gameLoss){
			GUI.Label(new Rect((Screen.width / 2) - (lossTexture.width / 2), Screen.height / 10, lossTexture.width, lossTexture.height), lossTexture);
			Vector3 mousePosition = Input.mousePosition;
			mousePosition.y = Screen.height - mousePosition.y;
			if(continueRect.Contains(mousePosition)){
				if(GUI.Button(continueRect, continueButtonHighlighted, continueButtonStyle)){
					Load();
					currentHealth = maxHealth;
					gameLoss = false;
					alive = true;
					a.Play("Idle1");
				}
			}
			else{
				if(GUI.Button(continueRect, continueButton, continueButtonStyle)){
					Load();
					currentHealth = maxHealth;
					gameLoss = false;
					alive = true;
					a.Play("Idle1");
				}
			}
		}
		
		#endregion
		
		
		if(checkpointTextureCurrentAlpha > 0){
			GUI.color = new Color(1, 1, 1, checkpointTextureCurrentAlpha);
			GUI.DrawTexture(new Rect((Screen.width) / 2 - (checkpointTexture.width / 2), Screen.height / 5, checkpointTexture.width, checkpointTexture.height), checkpointTexture);
			GUI.DrawTexture(new Rect((Screen.width) / 2 - (checkpointTexture.width / 2) + 25, Screen.height / 3.25f, playerPortrait.width, playerPortrait.height), playerPortrait);
			GUI.DrawTexture(new Rect((Screen.width) / 2 + (checkpointTexture.width / 3.2f), Screen.height / 3.25f, playerPortrait.width, playerPortrait.height), playerPortrait);
			GUI.color = new Color(1, 1, 1, 1);
			if(Time.time > checkpointTextureFadeTime)
				checkpointTextureCurrentAlpha -= 0.004f;
		}

	}
	
	/// <summary>
	/// Reinitializes the level by setting each spawn point as untriggered.
	/// </summary>
	private void reinitializeLevel(){
		GameObject[] spawnZones = GameObject.FindGameObjectsWithTag("SpawnPoint");
		foreach(GameObject spawnZone in spawnZones){
			SpawnZoneScript spawnScript = spawnZone.GetComponent(typeof(SpawnZoneScript)) as SpawnZoneScript;
			spawnScript.setTriggered(false);
		}
		
	}
	
	/// <summary>
	/// Draws the inventory window, showing the current state of the inventory array.
	/// </summary>
	/// <param name='windowID'>
	/// Unique window identifier.
	/// </param>
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
					if(Event.current.button == 1)
						inventory[i-1] = equipItem(item);
					else if (Event.current.button == 0){
						if(lastInventoryIndex == i && Time.time - lastInventoryClick < 0.3f)
							confirmPopup = true;
						else{
							lastInventoryIndex = i;
							lastInventoryClick = Time.time;
						}
					}
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
			
			//We are very hesitant to catch all exceptions like this. However, in this single case, we are positive that the exception is throw in parsing the number, due to
			//possible cascading of GUI.tooltip. In future iterations, we would like to clean this cascading up, and avoid this catch.
			try{
				int index = int.Parse(GUI.tooltip);
				Item item = inventory[index];
				toolTipString = item.getStats();
				Color[] itemRarityColors = {
				Color.gray, Color.white, Color.green, new Color(0.01f, 0.80f, 0.85f, 1), Color.yellow, Color.red	
				};
				toolTipColor = itemRarityColors[item.getItemRarity()];
				toolTipExpireTime = Time.time + toolTipDelay;
			}
			catch {
				return;	
			}
		}
		
		GUI.DragWindow();
	}
	
	/// <summary>
	/// Draws the item deletion confirmation window.
	/// </summary>
	/// <param name='windowID'>
	/// Unique window identifier.
	/// </param>
	void confirmDraw(int windowID){
		if(lastInventoryIndex < 1)
			return;
		
		GUIStyle textStyle = new GUIStyle();
		textStyle.alignment = TextAnchor.UpperCenter;
		textStyle.fontSize = 16;
		textStyle.normal.textColor = Color.white;
		
		GUI.Label(new Rect(0, 50, 300, 100), "Are you sure you want to delete \n this item?", textStyle);
		GUI.Label(new Rect(125, 100, 50, 50), new GUIContent(inventory[lastInventoryIndex-1].getTexture(), inventory[lastInventoryIndex-1].getStats()));
		if(GUI.Button(new Rect(50, 200, 75, 50), "Cancel")){
			confirmPopup = false;
		}
		if(GUI.Button(new Rect(175, 200, 75, 50), "Confirm")){
			inventory[lastInventoryIndex-1] = null;
			confirmPopup = false;	
		}
		
		if(!string.IsNullOrEmpty(GUI.tooltip)){
			Item item = inventory[lastInventoryIndex-1];
			toolTipString = item.getStats();
			Color[] itemRarityColors = {
			Color.gray, Color.white, Color.green, new Color(0.01f, 0.80f, 0.85f, 1), Color.yellow, Color.red	
			};
			toolTipColor = itemRarityColors[item.getItemRarity()];
			toolTipExpireTime = Time.time + toolTipDelay;
		}
	}
	
	/// <summary>
	/// Draws the tooltip window.
	/// </summary>
	/// <param name='windowID'>
	/// Unique window identifier.
	/// </param>
	void tooltipWindow(int windowID){
		GUIStyle tooltipStyle = new GUIStyle();
		tooltipStyle.fontSize = 14;
		tooltipStyle.normal.textColor = toolTipColor;
		tooltipStyle.alignment = TextAnchor.MiddleCenter;
		tooltipStyle.wordWrap = true;
		GUI.Label(new Rect(0, 0, tooltipWindowWidth, tooltipWindowHeight), toolTipString, tooltipStyle);
	}
	
	/// <summary>
	/// Draws the equipment window, showing the current state of the equipment array, as well as
	/// general Player statistics.
	/// </summary>
	/// <param name='windowID'>
	/// Unique window identifier.
	/// </param>
	void equipmentDraw(int windowID){
		
		int[] x = {175, 120, 120, 175, 230, 175};
		
		int[] y = {20, 95, 145, 120, 120, 220};
		
		for(int i = 0; i < x.Length; i++){
			if(GUI.Button(new Rect(x[i], y[i], 50, 50), new GUIContent(equipment[i]!=null?equipment[i].getTexture():null, equipment[i]!=null?i+"":null))){
				if(Event.current.button == 1){
					Item item = equipment[i];
					if(item == null)
						return;
					equipment[i] = null;
					awardItem(item);
					recalculateStats();
				}
			}
		}
		
		//Player Stats
		GUIStyle statStyle = new GUIStyle();
		statStyle.fontSize = 13;
		statStyle.alignment = TextAnchor.MiddleCenter;
		statStyle.normal.textColor = Color.white;
		GUI.Label(new Rect(5, 20, 100, 300), getStats(), statStyle);
		//Set tooltip window if applicable
		if(!string.IsNullOrEmpty(GUI.tooltip)){
			int Index = int.Parse(GUI.tooltip);
			Item item = equipment[Index];
			toolTipString = item.getStats();
			Color[] itemRarityColors = {
			Color.gray, Color.white, Color.green, new Color(0.01f, 0.80f, 0.85f, 1), Color.yellow, Color.red	
			};
			toolTipColor = itemRarityColors[item.getItemRarity()];
			toolTipExpireTime = Time.time + toolTipDelay;
		}
		
		GUI.DragWindow();
	}
	
	/// <summary>
	/// Gets the string representation of the Player's statistics. Used primarily in tooltips.
	/// </summary>
	/// <returns>
	/// String representation of Player stats.
	/// </returns>
	public string getStats(){
		return "Strength: " + strength + "\n" +
			"Dexterity: " + dexterity + "\n" +
			"Intelligence: " + intelligence + "\n" +
			"Vitality: " + vitality + "\n" +
			"Damage: " + weaponDamage.ToString("0.00") + "\n" +
			"Attack Speed: " + weaponSpeed.ToString("0.00") + "\n" + 
			"Level: " + level + "\n";
	}
	
	/// <summary>
	/// Returns boolean indicating if the next attack will be modified by tumble.
	/// </summary>
	/// <returns>
	/// If the next attack will be modified by tumble.
	/// </returns>
	public bool getTumbleAttack(){
		return tumbleAttack;	
	}
	
	// Use this for initialization
	void Start () {
		
		//Initialize Abilities - TODO Should this be done statically?
		tumble = new Tumble();
		tumble.setScript(this);
		silverBolts = new Silver_Bolts();
		silverBolts.setScript(this);
		condemn = new Condemn();
		condemn.setScript(this);
		finalHour = new Final_Hour();
		finalHour.setScript(this);
		shadowBolt = new ShadowBolt();
		shadowBolt.setScript(this);
		
		IconWidth = tumbleTexture.width;
		IconHeight = tumbleTexture.height;
		
		level = 1;
		
		//Initially not running
		running = false;
		
		idling = true;
		
		a = gameObject.GetComponent(typeof(Animation)) as Animation;
		
		originalRotation = transform.localRotation;
		
		alive = true;
		
		inventory = new Item[inventorySize];
		
		Bow startingBow = new Bow();
		startingBow.randomizeWeapon(1, 1);
		awardItem(startingBow);
		
		Helm startingHelm = new Helm();
		startingHelm.randomizeArmor(1, 1);
		awardItem(startingHelm);
		
		Chest startingChest = new Chest();
		startingChest.randomizeArmor(1, 1);
		awardItem(startingChest);
		
		Gloves startingGloves = new Gloves();
		startingGloves.randomizeArmor(1, 1);
		awardItem(startingGloves);
		
		Boots startingBoots = new Boots();
		startingBoots.randomizeArmor(1, 1);
		awardItem(startingBoots);
		
		checkpointTexture = Resources.Load("CheckpointTexture/CheckpointMaybe") as Texture2D;
		playerPortrait = Resources.Load("CheckpointTexture/vayne_circle") as Texture2D;
		playerManaTexture = Resources.Load("PlayerTextures/mana") as Texture2D;
		swiftDeathTexture = Resources.Load("VayneTextures/swiftDeath") as Texture2D;
		shadowBoltTexture = Resources.Load("VayneTextures/ShadowBolt") as Texture2D;
		
		leftClickOverlay = Resources.Load("InstructionPage/leftclick") as Texture2D;
		rightClickOverlay = Resources.Load("InstructionPage/rightclick") as Texture2D;
		
		winTexture = Resources.Load("GUITextures/victoryFull") as Texture2D;
		lossTexture = Resources.Load("GUITextures/defeatFull") as Texture2D;
		continueButton = Resources.Load("InstructionPage/Continue") as Texture2D;
		continueButtonHighlighted = Resources.Load("InstructionPage/Continue2") as Texture2D;
					
		equipment = new Item[6]; //TODO decide on size
		
		equipmentRect = new Rect(50, 50, 300, 300);
		inventoryRect = new Rect(Screen.width - 350, 50, 300, Mathf.Ceil(inventorySize / 6) * 50 + 20);
		confirmRect = new Rect((Screen.width / 2) - 150, Screen.height / 4, 300, 300);
		continueRect = new Rect((Screen.width / 2) - (continueButton.width / 2), Screen.height * (3.0f/4.0f), continueButton.width, continueButton.height);
		
		movementSpeed = 1;
		
		recalculateStats();
		
		if(PlayerPrefs.GetString("IsSaveGame") == "true"){
			name = PlayerPrefs.GetString("SaveFileName");
			Load();
		}
		
		currentHealth = maxHealth;
		currentMana = maxMana;
		
		Save();
		
	}
	
	/// <summary>
	/// Specifies if the Player is currently in a stunned state. Stunned players are unable to
	/// move, attack, or loot. A Player may be stunned by enemy spells, or due to having recently
	/// performed an attack.
	/// </summary>
	public bool stunned(){
		return Time.time < stunTime;
	}
	
	//Keeps object from being destroyed on scene transition.
	void Awake() {
		DontDestroyOnLoad(this.gameObject);
	}
	
	
	// Update is called once per frame
	void Update () {
		
		tumble.Update();
		silverBolts.Update();
		condemn.Update();
		
		if(!alive)
			return;
		
		//Currently, we are experiencing a bug where Vayne will randomly rotate herself when attacking,
		//causing movements to be rejected. Until we are able to better understand the reason behind this
		//rotation, we have applied a temporary fix. This should be overridden as soon as a solution
		//becomes apparent.
		GameObject topObject = GameObject.FindGameObjectWithTag("TopLevelObject");
		Vector3 newPosition = topObject.transform.position;
		newPosition.y = 5.702944f;
		topObject.transform.position = newPosition;
		
		Quaternion rot = topObject.transform.localRotation;
		//rot.y = 90f;
		rot.x = 0;
		rot.z = 0;
		topObject.transform.localRotation = rot;
		
		if(currentMana < maxMana){
			currentMana += Time.deltaTime * manaRegenRate;
			if(currentMana > maxMana)
				currentMana = maxMana;
		}
		
		if(lastCombatTime + healthRegenDelay < Time.time){
			if(currentHealth < maxHealth){
				currentHealth += Time.fixedDeltaTime * (healthRegenRate / maxHealth) * 400;
				if(currentHealth > maxHealth)
					currentHealth = maxHealth;
			}
		}
		
		string[] keys = new string[debuffs.Keys.Count];
		debuffs.Keys.CopyTo(keys, 0);
		
		//Check for expired debuffs
		foreach(string obj in keys){
			Debuff debuff = (Debuff) debuffs[obj];
			if(debuff.hasExpired()){
				debuff.expire(this);
				debuffs.Remove(debuff.name());
			}
		}
		
		if(stunned())
			return;
		
		//Open Inventory
		if(Input.GetKeyDown(KeyCode.I))
			inventoryOpen = !inventoryOpen;

		//Open Character Sheet
		if(Input.GetKeyDown(KeyCode.C))
			characterSheetOpen = !characterSheetOpen;
		
		
		//Check for spells being cast
		if(Input.GetKeyDown(KeyCode.Alpha1)){
			tumble.Execute();
		}
		
		else if(Input.GetKeyDown(KeyCode.Alpha2)){
			silverBolts.Execute();
		}
		
		else if(Input.GetKeyDown(KeyCode.Alpha3)){
			condemn.Execute();
		}
		
		else if(Input.GetKeyDown(KeyCode.Alpha4)){
			finalHour.Execute();
		}
		
		else if(Input.GetButtonDown("Fire2") && !guiInteraction(Input.mousePosition)){ //Right click
			shadowBolt.Execute();
		}
		
		if(running && !a.IsPlaying("Run")) {
			a.Stop();
			a.Play("Run");
			idling = false;
		}
		
		//TODO Add modifications to allow for tumble, ult, and tumbleult idles
		else if (!a.isPlaying)
			a.Play("Idle1");
		
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
		
		//Debug.Log (finalHourActive + " " + (finalHour.getStartTime () + finalHour.getActiveDuration()) + " " + Time.time);
		
		if(finalHourActive && (finalHour.getStartTime() + finalHour.getActiveDuration() < Time.time))
		{
			activateFinalHour(false);
		}
	
	}
	
	/// <summary>
	/// Save the inventory, equipment, and state data of the Player.
	/// </summary>
	public void Save(){
		
		if(!System.IO.Directory.Exists("saves"))
			System.IO.Directory.CreateDirectory("saves");
		
				//If this has not been saved before, look for an unused file name.
		if(name == null){
			for(int i = 0; ; i++){
				if(!System.IO.File.Exists("saves/Vayne"+i+".txt")){
					name = "saves/Vayne" + i + ".txt";
					break;
				}
			}
		}
		
		System.IO.StreamWriter file = new System.IO.StreamWriter(name);
		
		file.WriteLine("<Position>");
		GameObject topLevelObject = (GameObject) GameObject.FindGameObjectWithTag("TopLevelObject");
		file.WriteLine(topLevelObject.transform.position.x + "," + topLevelObject.transform.position.y + "," + topLevelObject.transform.position.z);
		file.WriteLine("</Position>");
		
		file.WriteLine("<Inventory>");
		foreach(Item item in inventory){
			if(item == null)
				continue;
			file.WriteLine(item.Save());
		}
		file.WriteLine("</Inventory>");
		
		file.WriteLine("<DateStamp>");
		file.WriteLine(System.DateTime.Now.ToShortDateString() + " " + System.DateTime.Now.ToShortTimeString());
		file.WriteLine("</DateStamp>");
		
		file.WriteLine("<Level>");
		file.WriteLine(level+"");
		file.WriteLine("</Level>");
		
		file.WriteLine("<Equipment>");
		foreach(Item item in equipment){
			if(item == null)
				continue;
			file.WriteLine(item.Save());
		}
		file.WriteLine("</Equipment>");
		
		file.Flush();
		file.Close();
		
		checkpointTextureCurrentAlpha = 1;
		checkpointTextureFadeTime = Time.time + checkpointTextureDelay;
	}
	
	/// <summary>
	/// Reads from a save file, and updates the Player's position, inventory, and equipment. Any progress made by the character since the last
	/// save will be overriden when this function is called.
	/// </summary>
	private void Load(){
		for(int i = 0; i < inventory.Length; i++){
			inventory[i] = null;
		}
		
		for(int i = 0; i < equipment.Length; i++){
			equipment[i] = null;	
		}
		
		System.IO.StreamReader reader = new System.IO.StreamReader(name);
		string fullString = reader.ReadToEnd();
		reader.Close();
		GameObject topLevelObject = (GameObject) GameObject.FindGameObjectWithTag("TopLevelObject");
		
		string[] position = getTaggedLine("<Position>", fullString).Split(',');
		topLevelObject.transform.position = new Vector3(float.Parse(position[0]), float.Parse(position[1]), float.Parse(position[2]));
		
		string[] inventoryArray = System.Text.RegularExpressions.Regex.Split(getTaggedLine("<Inventory>", fullString), "\r\n");
		foreach(string itemString in inventoryArray){
			if(itemString.Split(',')[0] == "Weapon")
				awardItem(Weapon.Load(itemString));
			else if (itemString.Split(',')[0] == "Armor")
				awardItem(Armor.Load(itemString));
		}
		
		string[] equipmentArray = System.Text.RegularExpressions.Regex.Split(getTaggedLine("<Equipment>", fullString), "\r\n");
		foreach(string equipString in equipmentArray){
			if(equipString.Split(',')[0] == "Weapon")
				equipItem(Weapon.Load(equipString));
			else if (equipString.Split(',')[0] == "Armor")
				equipItem(Armor.Load(equipString));
		}
		
		(topLevelObject.GetComponent(typeof(MouseMovement)) as MouseMovement).setMoving(false);
		
		level = int.Parse(getTaggedLine("<Level>", fullString));
		
		recalculateStats();
		
		
	}
	
	/// <summary>
	/// Gets the string between two tagged areas. Primarily used in parsing data from a load file.
	/// </summary>
	/// <returns>
	/// The string between the two tagged areas.
	/// </returns>
	/// <param name='tag'>
	/// The tag to look for. IE: "<Position>".
	/// </param>
	/// <param name='data'>
	/// The string to be parsed.
	/// </param>
	/// <example>
	/// Given the tag "<Position>" and the string "<Position>Example String</Position>", this function
	/// would return "Example String".
	/// </example>
	private string getTaggedLine(string tag, string data){
		return data.Substring(data.IndexOf(tag) + tag.Length, data.IndexOf(tag.Replace("<", "</")) - data.IndexOf(tag) - tag.Length);
	}
	
	/// <summary>
	/// Sets the game state to "won".
	/// </summary>
	/// <param name='win'>
	/// If the game has been won.
	/// </param>
	public void setWin(bool win){
		gameWon = win;	
	}
	
	/// <summary>
	/// Places the specified item into the Player's inventory, if it is not already full.
	/// The caller is responsible for handling the case of a full inventory before awarding
	/// an item.
	/// </summary>
	/// <param name='item'>
	/// Item.
	/// </param>
	public void awardItem(Item item){
		for(int i = 0; i < inventory.Length; i++)
			if(inventory[i] == null){
			inventory[i] = item;
			return;
		}
	}
	
	/// <summary>
	/// Equips the item, if possible, returning the item previously in the equipment slot.
	/// </summary>
	/// <returns>
	/// The item previously in the equipment slot taken by the newly equipped item.
	/// If no item previously occupied the equipment slot, a null item is returned.
	/// If the item is not equippable by this class, the item is returned.
	/// </returns>
	/// <param name='item'>
	/// Item to be equipped by the Player.
	/// </param>
	private Item equipItem(Item item){
		System.Type[] types = new System.Type[6];
		types[0] = typeof(Helm);
		types[1] = typeof(Bow);
		types[2] = typeof(Bow);
		types[3] = typeof(Chest);
		types[4] = typeof(Gloves);
		types[5] = typeof(Boots);
		
		int index = System.Array.IndexOf(types, item.GetType());
		
		if(index != -1){
			Item toReturn = equipment[index];
			if(item.GetType() == typeof(Bow) && equipment[1] != null){
				toReturn = equipment[2];
				equipment[2] = item;
			}
			else
				equipment[index] = item;
			recalculateStats();
			return toReturn;
		}
		
		return item;
	}
	
	/// <summary>
	/// Recalculates the Player stats using all equipped Items.
	/// </summary>
	private void recalculateStats(){
		//Restore base stats
		strength = 10;
		dexterity = 7;
		intelligence = 7;
		vitality = 10;
		weaponDamage = 5;
		weaponSpeed = 1.5f;
		maxHealth = 100;
		maxMana = 100;
		
		//Add each item
		foreach(Item item in equipment){
			if(item == null)
				continue;
			item.Equip(this);
		}
		
		//If weilding two bows, use the average of their attack speeds.
		if(equipment[1] != null && equipment[2] != null){
			weaponSpeed /= 2;	
		}
		
		//Class specific enhancements based on stats
		this.weaponDamage += this.dexterity;
		this.maxMana += (this.intelligence * 0.5f);
		this.maxHealth = 100 + (vitality * 10);
		
		
		if(this.currentHealth > this.maxHealth)
			this.currentHealth = this.maxHealth;
		if(this.currentMana > this.maxMana)
			this.currentMana = this.maxMana;
	}
	
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
	public bool guiInteraction(Vector3 mousePosition){
		//Y position is inversed
		mousePosition.y = Screen.height - mousePosition.y;
		
		if(inventoryOpen)
			if(inventoryRect.Contains(mousePosition))
				return true;
			
		if(characterSheetOpen)
			if(equipmentRect.Contains(mousePosition))
				return true;
		
		if(confirmPopup)
			if(confirmRect.Contains(mousePosition))
				return true;
		
		if(gameWon || gameLoss)
			if(continueRect.Contains(mousePosition))
				return true;
		
		return false;
	}
	
	/// <summary>
	/// Begins a randomly chosen idle animation.
	/// </summary>
	public void playIdleSequence(){
		if(!a.isPlaying){
			int num = ((int) (Random.value * 3)) + 1;
			a.Play("Idle"+num);
		}
		
		idling = true;
	}
	
	/// <summary>
	/// Determines if the Player's animator is currently playing the run animation.
	/// </summary>
	/// <returns>
	/// If the run animation is playing.
	/// </returns>
	public bool isRunAnimation(){
		return a.IsPlaying("Run");	
	}
	
	/// <summary>
	/// If the Player's animator is currently playing.
	/// </summary>
	/// <returns>
	/// If an animation is playing.
	/// </returns>
	public bool noAnimation(){
		return !a.isPlaying;	
	}
	
	/// <summary>
	/// Determines if the Player is currently alive.
	/// </summary>
	/// <returns>
	/// If the Player is alive.
	/// </returns>
	public bool isAlive(){
		return alive;
	}
	
	/// <summary>
	/// Stops the Player's animator.
	/// </summary>
	public void stopAnimation(){
		a.Stop();	
	}
	
	/// <summary>
	/// Gets the current health percent of the Player.
	/// </summary>
	/// <returns>
	/// Player's health percentage.
	/// </returns>
	public float getHealthPercent(){
		return currentHealth / maxHealth;
	}
	
	/// <summary>
	/// Gets the current mana percent of the Player.
	/// </summary>
	/// <returns>
	/// Player's mana percentage.
	/// </returns>
	public float getManaPercent(){
		return currentMana / maxMana;	
	}
	
	/// <summary>
	/// Sets the Player's current enemy.
	/// </summary>
	/// <param name='enemy'>
	/// New Enemy.
	/// </param>
	public void setCurrentEnemy(EnemyScript enemy){
		this.currentEnemy = enemy;	
	}
	
	/// <summary>
	/// Sets the idling state of the Player, used for determining when idling animations
	/// should begin.
	/// </summary>
	/// <param name='idle'>
	/// If the player should now be idling.
	/// </param>
	public void setIdling(bool idle){
		idling = idle;
	}
	
	/// <summary>
	/// Gets the Player's current enemy.
	/// </summary>
	/// <returns>
	/// The current Player's enemy.
	/// </returns>
	public EnemyScript getCurrentEnemy(){
		return currentEnemy;	
	}
	
	/// <summary>
	/// Gets the Player's weapon damage, with primary stat effects already
	/// included.
	/// </summary>
	/// <returns>
	/// The Player's current weapon damage.
	/// </returns>
	public float getWeaponDamage(){
		return WeaponDamage;	
	}
	
	/// <summary>
	/// Sets the Player's weapon damage.
	/// </summary>
	/// <param name='dmg'>
	/// The new Player's weapon damage.
	/// </param>
	public void setWeaponDamage(float dmg){
		WeaponDamage = dmg;	
	}	
	
	/// <summary>
	/// Gets the Player's weapon speed, with additional buff and debuff effects
	/// already applied.
	/// </summary>
	/// <returns>
	/// The Player's current weapon speed.
	/// </returns>
	public float getWeaponSpeed(){
		return WeaponSpeed;
	}
	
	/// <summary>
	/// Gets the GameObject to which the Player is attached.
	/// </summary>
	/// <returns>
	/// Player's GameObject.
	/// </returns>
	public GameObject getGameObject(){
		return gameObject;	
	}
	
	/// <summary>
	/// Plays the specified animation, if exists.
	/// </summary>
	/// <param name='animationName'>
	/// The name of the animation to play.
	/// </param>
	public void playAnimation(string animationName){
		if(a.IsPlaying(animationName))
			return;
		a.Stop();
		a.Play(animationName);
	}
	
	/// <summary>
	/// Sets Player's running state, used to determine what actions are possible, and what
	/// animations should be played.
	/// </summary>
	/// <param name='active'>
	/// The Player's new running state.
	/// </param>
	public void setRunning(bool active){
		running = active;
	}
	
	/// <summary>
	/// Sets Player's tumbling state, used to determine damage increases.
	/// </summary>
	/// <param name='active'>
	/// The Player's new tumbling state.
	/// </param>
	public void setTumbled(bool active){
		tumbled = active;
	}
	
	/// <summary>
	/// Gets the Player's attack range.
	/// </summary>
	/// <returns>
	/// Player's attack range.
	/// </returns>
	public float getRange(){
		return range;
	}
	
	/// <summary>
	/// Stun the Player for the specified time.
	/// </summary>
	/// <param name='time'>
	/// Time for Player to remain stunned.
	/// </param>
	public void Stun(float time){
		stunTime = Time.time + time;	
	}
	
	/// <summary>
	/// Activates the Final Hour Buff.
	/// </summary>
	/// <param name='active'>
	/// New Final Hour active state
	/// </param>
	public void activateFinalHour(bool active){
			finalHourActive = active;
	}
	
	/// <summary>
	/// Awards the Player additional health, up to a maximum health value.
	/// </summary>
	/// <param name='amount'>
	/// Amount of additional Player health.
	/// </param>
	public void awardHealth(float amount){
		//Calculate additional healing based on passive or skills
		currentHealth += amount;
		if(currentHealth > maxHealth)
			currentHealth = maxHealth;
	}
	
	/// <summary>
	/// Damage the Player by specified amount, before mitigation is taken into consideration.
	/// </summary>
	/// <param name='amount'>
	/// Amount of pre-mitigation damage to the Player.
	/// </param>
	public void damage(float amount){
		if(!alive)
			return;
		
		//Apply damage
		currentHealth -= amount;
		if(currentHealth < 0){
			currentHealth = 0;
			alive = false;
			a.Stop();
			a.Play("Death");
			gameLoss = true;
		}
		
		lastCombatTime = Time.time;

	}
	
	/// <summary>
	/// Sets the Player's poisoned state.
	/// </summary>
	/// <param name='poisoned'>
	/// Player's new poisoned state.
	/// </param>
	public void setPoisoned(bool poisoned){
		this.poisoned = poisoned;
	}
	
	/// <summary>
	/// Determines if the Player is currently poisoned.
	/// </summary>
	/// <returns>
	/// Player's poisoned state.
	/// </returns>
	public bool isPoisoned(){
		return poisoned;
	}
	
	/// <summary>
	/// Applies the given debuff. If the debuff is already on the Player, and the debuff is allowed to be refreshed, its
	/// duration is reset.
	/// </summary>
	/// <param name='debuff'>
	/// The debuff to apply.
	/// </param>
	public void applyDebuff(Debuff debuff){
		if(debuffs.ContainsKey(debuff.name()) && debuff.prolongable()){
			Debuff currentDebuff = (Debuff) debuffs[debuff.name()];
			currentDebuff.refresh();
		}
		else {
			debuff.apply(this);
			debuffs.Add(debuff.name(), debuff);
		}
	}
	
	/*
	 * Returns the time of the next auto-attack
	 * */
	public float autoAttack(EnemyScript enemy){
		float damageAmount = getWeaponDamage();
		if(finalHourActive){
			damageAmount *= 1.4f;
			awardHealth (damageAmount * .5f);
		}
		
		if(tumbleAttack){
			damageAmount *= 1.3f;
			tumbleAttack = false;
		}
		
		currentMana += 10;
		if(currentMana > maxMana)
			currentMana = maxMana;
		
		string[] keys = new string[debuffs.Keys.Count];
		debuffs.Keys.CopyTo(keys, 0);
		
		//Check for expired debuffs
		foreach(string obj in keys){
			Debuff debuff = (Debuff) debuffs[obj];
			damageAmount = debuff.applyDebuff(damageAmount);
		}
		
		GameObject bolt = (GameObject) Instantiate(boltObject, gameObject.transform.position, Quaternion.identity);
		SeekingProjectileScript seekScript = bolt.GetComponent(typeof(SeekingProjectileScript)) as SeekingProjectileScript;
		seekScript.setTarget(enemy.gameObject);
		seekScript.setDamage(damageAmount);
		seekScript.setSpeed(1.5f);
		enemy.applyDebuff(new SwiftDeathDebuff(enemy), false);
		
		Stun(1f);
		
		a.Stop();	
		a.Play("Attack" + (((int)(Random.value * 2)) + 1));
		running = false;
		lastCombatTime = Time.time;
		//enemy.damage(damageAmount);
		return Time.time + WeaponSpeed;
	}
	
	/// <summary>
	/// The Tumble ability moves the Player towards the mouse's current location, and increases the damage of the next auto-attack by 30%.
	/// </summary>
	class Tumble : Ability {
		
		private VayneScript player;
		//private double totalDamage = 1.30d;
		private float nextAttack = 0;
		private float coolDown = 6;
		private string animationName = "Spell1";
		private float originalMovementSpeed;
		private float tumbleTime;
		private Vector3 dest;
		private float manaCost = 30;
		
		private GameObject topLevelObject;
		
		
		public void Execute(){
			
			//Check if the ability is on cooldown
			if(nextAttack > Time.time || player.currentMana < manaCost)
				return;
			else
				player.currentMana -= manaCost;
			
			player.playAnimation(animationName);
			originalMovementSpeed = player.MovementSpeed;
			player.MovementSpeed *= 4;
			
			player.setTumbled(true);
			
			tumbleTime = Time.time + 0.5f;
			nextAttack = Time.time + coolDown;
			
			RaycastHit hit;
			
			int layermask = 1 << 10;
			layermask = ~layermask;
			
			Ray ray = Camera.allCameras[0].ScreenPointToRay(Input.mousePosition);
			
			if(Physics.Raycast(ray, out hit, 10000, layermask)){
				dest = hit.point;
				dest.y = player.transform.position.y;
				player.transform.LookAt(dest);
			}
			
			player.Stun(0.7f);
			
			(topLevelObject.GetComponent(typeof(MouseMovement)) as MouseMovement).setMoving(false);
			player.setRunning(false);
			
			if(!player.tumbleAttack)
				player.applyDebuff(new TumbleBuff());
		}
		
		public void setScript(VayneScript script){
			player = script;		
			//totalDamage = player.getWeaponDamage() * 1.3f;
		}
		
		public void Update(){
			if(topLevelObject == null)
				topLevelObject = GameObject.FindGameObjectWithTag("TopLevelObject");
			
			if(Time.time > tumbleTime && tumbleTime > 0){
				player.movementSpeed = originalMovementSpeed;
				return;	
			}
			else if(tumbleTime == 0)
				return;
			
			player.playAnimation(animationName);
			
			float distance = Vector3.Distance(topLevelObject.transform.position, dest);
			if(distance < 1f){ //Reach destination
				dest.y = topLevelObject.transform.position.y;
				topLevelObject.transform.position = dest;
				return;
			}
			
			CharacterController controller = topLevelObject.GetComponent(typeof(CharacterController)) as CharacterController;
			dest.y = topLevelObject.transform.position.y;
			controller.Move((dest - topLevelObject.transform.position).normalized * player.MovementSpeed);
			
			
		}
		
		public float getCooldown(){
			if(Time.time > nextAttack)
				return 0;
			else
				return (float)((nextAttack - Time.time) / coolDown);
		}
		
	}
	
	/// <summary>
	/// The Silver bolts ability performs three swift, consecutive attacks, with the third attack dealing bonus damage.
	/// </summary>
	class Silver_Bolts : Ability {
		private VayneScript player;
		private bool active = false;
		private float cooldown = 10;
		private float nextAttack = 0;
		private float nextShot = 0;
		private string animationName = "Crit";
		private int shotsFired = 0;
		private float manaCost = 50;
	
		
		public void Update(){
			if((shotsFired > 2) || !active)
				return;
			
			if(nextShot < Time.time){
				SilverAttack();	
			}
			
//			if(nextAttack < Time.time && active && shotsFired == 3){
//				active = false;
//				//player.activateCourage(false);
//				shotsFired = 0;
//				Debug.Log ("SilverBolts finished!");
//				return;
//			}
//			Execute();
			//shotsFired++; Now updates in silverBoltAttack
		}
		
		public void Execute(){
			
			//Check if on cooldown
			if(Time.time < nextAttack || (player.currentEnemy == null) || player.currentMana < manaCost)
				return;
			else
				player.currentMana -= manaCost;
			
			nextAttack = Time.time + cooldown;
			active = true;
			shotsFired = 0;
			
		}
		
		public void setScript(VayneScript script){
			player = script;
		}
		
		public float getCooldown(){
			if(Time.time > nextAttack)
				return 0;
			else
				return (float)((nextAttack - Time.time) / cooldown);
		}
		
		//TODO Get "run until in range aspect added"
		public void SilverAttack(){
			//Check for mouse click and update destination
			if(active){
				
				Debug.Log("Shots fired : " + shotsFired);
				
				if(player.currentEnemy == null){
					active = false;
					shotsFired = 3;
					return;
				}

				
				
				if(Vector3.Distance(player.currentEnemy.transform.position, player.getGameObject().transform.position) <= player.getRange()){
					float singleShotDamage = player.getWeaponDamage();
					if(player.finalHourActive)
						singleShotDamage *= 1.4f;
					if(shotsFired == 2){
						singleShotDamage *= 1.3f;
						active = false; //This is the last shot
						player.currentEnemy.snare(1.5f);
					}
					
					shotsFired++;
					
					GameObject bolt = (GameObject) Instantiate(player.boltObject, player.getGameObject().transform.position, Quaternion.identity);
					SeekingProjectileScript seekScript = bolt.GetComponent(typeof(SeekingProjectileScript)) as SeekingProjectileScript;
					seekScript.setTarget(player.currentEnemy.gameObject);
					seekScript.setDamage(singleShotDamage);
					seekScript.setSpeed(2f);
					player.Stun(0.5f);
					player.stopAnimation();
					player.playAnimation("Attack" + (((int)(Random.value * 2)) + 1));
					nextShot = Time.time + 0.5f;
					
					
					Vector3 enemyLookAt = player.getCurrentEnemy().gameObject.transform.position;
					enemyLookAt.y = player.getGameObject().transform.position.y;
					player.getGameObject().transform.LookAt(enemyLookAt);
				}
				player.lastCombatTime = Time.time;

			}
		}
	}
	
	/// <summary>
	/// The Condemn ability knocks the enemy backward, dealing damage and stunning if the enemy hits a wall.
	/// </summary>
	class Condemn : Ability {
		private VayneScript player;
		private float distance = 60;
		private double totalDamage;
		private double nextAttack = 0;
		private double coolDown = 20;
		private string animationName = "Spell3";
		
		private float totalMovingTime = 1;
		private float startTime;
		private float endTime;
		private bool moving;
		private Vector3 originalPosition;
		private Vector3 destinationPosition;
		private Vector3 hitPoint;
		private float manaCost = 60;
		
		public void Execute(){
			
			//If this ability is on cooldown, don't do anything.
			//TODO Play "On Cooldown" sound?
			if(nextAttack > Time.time || (player.currentEnemy == null) || player.currentMana < manaCost)
				return;
			else
				player.currentMana -= manaCost;
			
			hitPoint = Vector3.zero;
			
			player.playAnimation(animationName);
			
			nextAttack = Time.time + coolDown; //Put ability on cooldown
			
			moving = true;
			
			EnemyScript enemy = player.currentEnemy;
			
			//Make the enemy look at the player
			Vector3 playerLookat = player.getGameObject().transform.position;
			playerLookat.y = player.currentEnemy.transform.position.y;
			player.currentEnemy.transform.LookAt(playerLookat);
			
			Vector3 enemyActualPosition = (enemy.gameObject.GetComponent(typeof(SphereCollider)) as SphereCollider).center;
			
			enemyActualPosition.y += 5;
							
			Vector3 direction = -1 * enemy.gameObject.transform.forward;
			
			originalPosition = enemy.transform.position;
			destinationPosition = enemy.transform.position + (direction * distance);
			
			GameObject bolt = (GameObject) Instantiate(player.boltObject, player.getGameObject().transform.position, Quaternion.identity);
			SeekingProjectileScript seekScript = bolt.GetComponent(typeof(SeekingProjectileScript)) as SeekingProjectileScript;
			seekScript.setTarget(enemy.gameObject);
			seekScript.setDamage(player.WeaponDamage);
			seekScript.setSpeed(10f);
			
			//Knockback
			RaycastHit hit;
			
			Ray ray = new Ray(enemyActualPosition, direction);
			Debug.DrawRay(enemyActualPosition, direction, Color.yellow);
			
			int layermask = (1 << 10);
			
			if(Physics.Raycast(ray, out hit, Vector3.Distance(player.getGameObject().transform.position, enemyActualPosition) + distance, layermask)){
				Debug.Log("hit!");
				enemy.damage(player.WeaponDamage);
				enemy.stun(2.5f);
				hitPoint = hit.point;
				hitPoint.y = enemy.transform.position.y;
			}
				
			
			
			startTime = Time.time;
			endTime = Time.time + totalMovingTime;
			player.lastCombatTime = Time.time;
		
		}
		
		public void setScript(VayneScript script){
			player = script;
			totalDamage = 1.0f * player.getWeaponDamage();
		}
		
		public void Update(){
			
			if(Time.time > endTime || player.currentEnemy == null)
				return;
			
			float deltaTime = (Time.time - startTime) / totalMovingTime;
			
			if(hitPoint.magnitude != 0 && Vector3.Distance(originalPosition, hitPoint) < Vector3.Distance(originalPosition, Vector3.Lerp(originalPosition, destinationPosition, deltaTime)))
				player.currentEnemy.transform.position = hitPoint;
			else
				player.currentEnemy.transform.position = Vector3.Lerp(originalPosition, destinationPosition, deltaTime);
			
		}
		
		public float getCooldown(){
			if(Time.time > nextAttack)
				return 0;
			else
				return (float)((nextAttack - Time.time) / coolDown);
		}
	}
	
	/// <summary>
	/// The Final Hour ability increases damage done from all sources by a flat percentage, and provides healing for a percentage
	/// of damage done.
	/// </summary>
	class Final_Hour : Ability {
		private VayneScript player;
		private float cooldown = 90;
		private float nextAttack = 0;
		private int duration = 9;
		private float bonusDamage = 1.4f;
		private float lifeSteal = 1.5f;
		private bool isUlted = false;
		private float startTime;
		private float manaCost = 80;
		
		
		private bool active = false;
		
	
		
		public void Update(){
			if((startTime + duration) < Time.time && active){
				active = false;
				player.activateFinalHour(false);
				Debug.Log("Final Hour finished!");
			}
		}
		
		public void Execute(){
			
			//Check if on cooldown
			if(Time.time < nextAttack || player.currentMana < manaCost)
				return;
			else
				player.currentMana -= manaCost;
			
			player.activateFinalHour(true);
			player.applyDebuff(new FinalHourBuff());
		
			nextAttack = Time.time + cooldown;
			startTime = Time.time;
			//player.playAnimation("Ult" + animationName);
			active = true;
			Debug.Log("Final Hour active!");
			Debug.Log (startTime + ": " + Time.time);
		}
		
		public void setScript(VayneScript script){
			player = script;
		}
		
		public float getCooldown(){
			if(Time.time > nextAttack)
				return 0;
			else
				return (float)((nextAttack - Time.time) / cooldown);
		}
		
		public float getActiveDuration(){
			return duration;
		}
		public float getStartTime(){
			return startTime;
		}
		
		
	}
	
	class ShadowBolt : Ability {
		private VayneScript player;
		private float nextAttack = 0;
		private int radius = 60;
		private string animationName = "Attack2";
		private float manaCost = 40;
		
		public void Execute(){
			
			if(player.currentMana < manaCost)
				return;
			else
				player.currentMana -= manaCost;
			
			player.playAnimation(animationName);
			
			Vector3 position = player.getGameObject().transform.position;
			int index = 0;
			Collider[] targets = Physics.OverlapSphere(position, radius);
			foreach(Collider t in targets){
				EnemyScript enemy = t.gameObject.GetComponent(typeof(EnemyScript)) as EnemyScript;
				if(enemy){
					Vector3 direction = Vector3.Normalize(enemy.gameObject.transform.position - player.gameObject.transform.position);
					float dot = Vector3.Dot(direction, player.gameObject.transform.forward);
					if(dot > 0.707f){
						GameObject bolt = (GameObject) Instantiate(player.boltObject, player.getGameObject().transform.position, Quaternion.identity);
						SeekingProjectileScript seekScript = bolt.GetComponent(typeof(SeekingProjectileScript)) as SeekingProjectileScript;
						seekScript.setTarget(enemy.gameObject);
						seekScript.setDamage(player.WeaponDamage * 0.7f);
						seekScript.setSpeed(1.5f);
						enemy.applyDebuff(new SwiftDeathDebuff(enemy), false);
						player.lastCombatTime = Time.time;
					}
				}
			}
		}
		
		public void setScript(VayneScript script){
			player = script;
		}
	}
	
}
