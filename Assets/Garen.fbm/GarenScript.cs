/*
 * Filename: GarenScript.cs
 * 
 * Author:
 * 		Programming: Daniel Opdyke
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
/// GarenScript representing the Garen class. Garen is a melee champion from League of Legends,
/// who utilizes speed and area of affect damage in combat. Garen has been equipped with modified
/// variations of his League of Legends abilities, as well as two new, unique abilities.
/// </summary>
public class GarenScript : MonoBehaviour, PlayerScript {
	
	#region Spell Objects
	
	/// <summary>
	/// The Decisive Strike ability object used to determine cooldown and execute ability.
	/// </summary>
	private Decisive_Strike dStrike;
	
	/// <summary>
	/// The Courage ability object used to determine cooldown and execute ability.
	/// </summary>
	private Courage courage;
	
	/// <summary>
	/// The Judgement ability object used to determine cooldown and execute ability.
	/// </summary>
	private Judgement judgement;
	
	/// <summary>
	/// The Demacian Justice ability object used to determine cooldown and execute ability.
	/// </summary>
	private Demacian_Justice dJustice;
	
	/// <summary>
	/// The Valor ability object used to determine cooldown and execute ability.
	/// </summary>
	private Valor valor;
	
	#endregion
	
	#region Spell Textures
	
	/// <summary>
	/// The Decisive Strike ability texure.
	/// </summary>
	public Texture2D dStrikeTexture;
	
	/// <summary>
	/// The Courage ability texure.
	/// </summary>
	public Texture2D courageTexture;
	
	/// <summary>
	/// The Judgement ability texture.
	/// </summary>
	public Texture2D judgementTexture;
	
	/// <summary>
	/// The Demacian Justice ability texture.
	/// </summary>
	public Texture2D dJusticeTexture;
	
	/// <summary>
	/// The Valor ability texture.
	/// </summary>
	public Texture2D valorTexture;
	
	/// <summary>
	/// The Might of Demacia ability texture.
	/// </summary>
	public Texture2D mightOfDemaciaTexture;
	
	/// <summary>
	/// The Valor debuff texture.
	/// </summary>
	public Texture2D valorDebuffTexture;
	
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
	
	
	#region Health Bar Textures
	
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
	/// The texture for the current selected enemy health bar, shown in the top center of the
	/// screen. Generally the same texture as the "enemyHealthTexture".
	/// </summary>
	/// <see cref="enemyHealthTexture"/>
	public Texture2D healthTexture;
	
	/// <summary>
	/// The texture to use for the Player's mana bar, shown in the center
	/// of the screen. This texture should be the unique to Mana.
	/// texture.
	/// </summary>
	private Texture2D playerManaTexture;
	
	
	#endregion
	
	/// <summary>
	/// The texture to be used as a background for the tooltip window.
	/// </summary>
	public Texture2D tooltipTexture;
	
	/// <summary>
	/// Player's current living status. When set to false, the Player is unable to perform actions,
	/// and the game is terminated.
	/// </summary>
	private bool alive;
	
	#region Status
	
	/// <summary>
	/// Hashtable matching debuff names to debuff objects.
	/// </summary>
	private Hashtable debuffs = new Hashtable();
	
	/// <summary>
	/// Boolean indicating if the Player is currently poisoned. Used primarily to determine if certain
	/// enemy abilities are amplified. The actual damage of the poison should be applied using a debuff.
	/// </summary>
	bool poisoned;
	
	#endregion
	
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
	/// The maximum mana of the Player. For Garen, this is 0.
	/// </summary>
	private float maxMana = 0;
	
	/// <summary>
	/// Boolean indicating if Courage is currently active. When set to true, the Player takes reduced
	/// damage from attacks.
	/// </summary>
	private bool courageActive;
	
	/// <summary>
	/// The original rotation of the Player's transform, used to re-initialize Player state.
	/// </summary>
	private Quaternion originalRotation;
	
	/// <summary>
	/// Boolean indicating if the Player is currently spinning. Spinning occurs only when the
	/// Judgement ability is activated, and should not be set for any other reason.
	/// </summary>
	private bool spinning;
	
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
	private float range = 30;
	
	/// <summary>
	/// Boolean indicating if the Player is currently idling. If true, the Player will begin to play
	/// idle animations after a few seconds.
	/// </summary>
	private bool idling;
	
	/// <summary>
	/// The minimal range requirement for the Player to automatically pick items up off the ground.
	/// </summary>
	private float pickUpRange = 10;
	
	/// <summary>
	/// The width of each ability texture icon.
	/// </summary>
	private int IconWidth;
	
	/// <summary>
	/// The height of each ability texture icon.
	/// </summary>
	private int IconHeight;
	
	/// <summary>
	/// The width of the debuff icon below the current enemy's health bar.
	/// </summary>
	private int debuffWidth = 30;
	
	/// <summary>
	/// The height of the debuff icon below the current enemy's health bar.
	/// </summary>
	private int debuffHeight = 30;
	
	#region Stats
	/// <summary>
	/// The stregnth characteristic of the Player, used to determine Weapon Damage.
	/// </summary>
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
	
	/// <summary>
	/// The dexterity characteristic of the Player.
	/// </summary>
	private int dexterity;

	public int Dexterity {
		get {
			return this.dexterity;
		}
		set {
			dexterity = value;
		}
	}	
	
	/// <summary>
	/// The intelligence characteristic of the Player.
	/// </summary>
	private int intelligence;
	
	/// <summary>
	/// The vitality characteristic of the Player, used to determine maximum health.
	/// </summary>
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
	
	/// <summary>
	/// The Weapon Damage of the Player, determined by the currently equipped weapon, the strength
	/// attribute, and the Player buffs / debuffs.
	/// </summary>
	private float weaponDamage;
	
	/// <summary>
	/// The Weapon Speed of the Player, determined by the currentl equipped weapon, and the Player
	/// buffs / debuffs.
	/// </summary>
	/// 
	private float weaponSpeed;
	
	/// <summary>
	/// The name of the Player. Currently unused, but included for future game iterations.
	/// </summary>
	/// <value>
	/// The name.
	/// </value>
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
	
	#region Tooltips
	/// <summary>
	/// The Decisive Strike tooltip text.
	/// </summary>
	private string dStrikeTooltip = "Decisive Strike \n Garen becomes invulnerable to \n slows/snares for 1 second, in \n addition his next melee attack will \n deal increased damage in a short \n cone in front of him. ";
	
	/// <summary>
	/// The Courage tooltip text.
	/// </summary>
	private string courageTooltip = "Courage \n  Garen shields himself, decreasing \n all damage taken by 30% for \n 3 seconds. ";
	
	/// <summary>
	/// The Judgement tooltip text.
	/// </summary>
	private string judgementTooltip = "Judgment \n Garen rapidly spings his sword \n around his body for 3 seconds, \n dealing 180% weapon damage as \n AoE over the duration. ";
	
	/// <summary>
	/// The Demacian Justice tooltip text.
	/// </summary>
	private string demacianJusticeTooltip = "Demacian Justice \n Garen brings down Demacian Justice \n on his opponents, dealing 300% \n weapon damage spread across 5 closest \n enemies (within melee range x 2) \n and stunning for 1.5 seconds.";
	
	/// <summary>
	/// The Might of Demacia tooltip text.
	/// </summary>
	private string mightOfDemaciaTooltip = "Might of Demacia \n Garen attacks targetted enemy for \n weapon damage. Subsequent attacks against \n this target deal 10% increased \n damage (stacks 3 times, max 30% \n increased damage for target attacked 3+ \n consecutive times)";
	
	/// <summary>
	/// The Valor tooltip text.
	/// </summary>
	private string valorTooltip = "Valor \n Garen cleaves up to 3 \n targets in front of him \n for 50% weapon damage each.";
	#endregion
	
	/// <summary>
	/// The Player's current Enemy. Used both as the target of certain attacks, as well as 
	/// displaying the health bar at the top center of the screen.
	/// </summary>
	private EnemyScript currentEnemy;
	
	#region Inventory
	/// <summary>
	/// Array containing the Items currently owned by the Player. Additions to the Player inventory
	/// should be done using the "awardItem" method.
	/// </summary>
	private Item[] inventory;
	
	/// <summary>
	/// The maximum size of the inventory.
	/// </summary>
	private int inventorySize = 30;
	
	/// <summary>
	/// Boolean indicating if the inventory window is currently open (GUI).
	/// </summary>
	private bool inventoryOpen = false;
	
	/// <summary>
	/// The rectangle position of the inventory window.
	/// </summary>
	private Rect inventoryRect;
	#endregion
	
	#region Equipment
	/// <summary>
	/// Array containing the Player's currently equipped Items. Modifications to the equipment array
	/// should be done using the "equipItem" function.
	/// </summary>
	private Item[] equipment;
	
	/// <summary>
	/// Boolean indicating if the character sheet, including Player stats and equipment, is currently open (GUI).
	/// </summary>
	private bool characterSheetOpen = false;
	
	/// <summary>
	/// The rectangle position of the equipment window.
	/// </summary>
	private Rect equipmentRect;
	#endregion
	
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
	/// The rate at which a Player will regenerate health when out of combat. In future
	/// version of the game, buffs and debuffs will be added to modify this value.
	/// </summary>
	private float healthRegenRate = 6f;
	
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
	/// The height of the tooltip window.
	/// </summary>
	private float tooltipWindowHeight = 0;
	
	/// <summary>
	/// The width of the tooltip window.
	/// </summary>
	private float tooltipWindowWidth = 0;
	
	/// <summary>
	/// The portrait of the Player displayed when a checkpoint is reached.
	/// </summary>
	private Texture2D playerPortrait;
	
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
	/// Called when the GameObject is awakened. Ensures the GameObject is not destroyed between Scene
	/// transitions.
	/// </summary>
	void Awake() {
		DontDestroyOnLoad(this);
	}
	
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
	
		#region Spells
		GUI.BeginGroup(new Rect(Screen.width / 2 - (2 * IconWidth), Screen.height - 98, IconWidth * 4, IconHeight * 4));
		
		GUI.Button(new Rect(0, 0, IconWidth, IconHeight), new GUIContent(dStrikeTexture, dStrikeTooltip));
		GUI.Label(new Rect(IconWidth * (2.8f/4), IconHeight * (2.8f/4), IconWidth / 4, IconHeight / 4), "1", cooldownNumberStyle);
		GUI.Button(new Rect(IconWidth, 0, IconWidth, IconHeight), new GUIContent(courageTexture, courageTooltip));
		GUI.Label(new Rect(IconWidth + (IconWidth * (2.8f/4)), IconHeight * (2.8f/4), IconWidth / 4, IconHeight / 4), "2", cooldownNumberStyle);
		GUI.Button(new Rect(IconWidth * 2, 0, IconWidth, IconHeight), new GUIContent(judgementTexture, judgementTooltip));
		GUI.Label(new Rect(2 * IconWidth + (IconWidth * (2.8f/4)), IconHeight * (2.8f/4), IconWidth / 4, IconHeight / 4), "3", cooldownNumberStyle);
		GUI.Button(new Rect(IconWidth * 3, 0, IconWidth, IconHeight), new GUIContent(dJusticeTexture, demacianJusticeTooltip));
		GUI.Label(new Rect(3 * IconWidth + (IconWidth * (2.8f/4)), IconHeight * (2.8f/4), IconWidth / 4, IconHeight / 4), "4", cooldownNumberStyle);

		GUI.EndGroup();
		
		
		GUI.BeginGroup(new Rect(Screen.width - (2 * IconWidth) - 30, Screen.height - 78, IconWidth * 2, IconHeight * 2));
		GUI.Button(new Rect(0, 0, IconWidth, IconHeight), new GUIContent(mightOfDemaciaTexture, mightOfDemaciaTooltip));
		GUI.Label(new Rect((1 * IconWidth) / 1.5f, IconHeight / 2.7f, IconWidth, IconHeight / 1.5f), leftClickOverlay);
		GUI.Button(new Rect(IconWidth, 0, IconWidth, IconHeight), new GUIContent(valorTexture, valorTooltip));
		GUI.Label(new Rect((2.5f * IconWidth) / 1.5f, IconHeight / 2.7f, IconWidth, IconHeight / 1.5f), rightClickOverlay);
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
		
		GUI.BeginGroup(new Rect(Screen.width / 2 - (2 * IconWidth), Screen.height - 98, IconWidth * 4, IconHeight * 4));
		
		GUI.Button(new Rect(0, 0, IconWidth, IconHeight * dStrike.getCooldown()), "");
		GUI.Button(new Rect(IconWidth, 0, IconWidth, IconHeight * courage.getCooldown()), "");
		GUI.Button(new Rect(IconWidth * 2, 0, IconWidth, IconHeight * judgement.getCooldown()), "");
		GUI.Button(new Rect(IconWidth * 3, 0, IconWidth, IconHeight * dJustice.getCooldown()), "");
		
		GUI.EndGroup();
		
		#endregion
		
		#region Health Bars
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
			
		}
		
		//Player Healthbar and Manabar
			GUI.Box(new Rect((Screen.width / 2) - (buttonLength / 2), Screen.height - 131, buttonLength, 30), "");
			
			GUI.BeginGroup(new Rect((Screen.width / 2) - (buttonLength / 2), Screen.height - 131, buttonLength * getHealthPercent(), 30));
			GUI.Box(new Rect(0, 0, buttonLength,30) , playerHealthTexture);
			GUI.EndGroup();
			GUI.Label(new Rect((Screen.width / 2) - (buttonLength / 2), Screen.height - 129, buttonLength, 30), ((int)currentHealth + "/" + (int)maxHealth), labelStyle);		
			GUI.Box(new Rect((Screen.width / 2) - (buttonLength / 2), Screen.height - 30, buttonLength, 30), "");
			
			GUI.BeginGroup(new Rect((Screen.width / 2) - (buttonLength / 2), Screen.height - 30, buttonLength * 0, 30));
			GUI.Box(new Rect(0, 0, buttonLength,30) , playerManaTexture);
			GUI.EndGroup();
			GUI.Label(new Rect((Screen.width / 2) - (buttonLength / 2), Screen.height - 30, buttonLength, 30), (int) currentMana + "/" + (int) maxMana, labelStyle);
		
		//Manabar (for Garen this is empty, but used to fill the space consistently
			GUI.BeginGroup(new Rect((Screen.width / 2) - (buttonLength / 2), Screen.height - 30, buttonLength * 0, 30));
			GUI.Box(new Rect(0, 0, buttonLength,30) , playerHealthTexture);
			GUI.EndGroup();
			GUI.Label(new Rect((Screen.width / 2) - (buttonLength / 2), Screen.height - 28, buttonLength, 30), "0/0", labelStyle);
		
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
					topLevelObject.transform.position = new Vector3(-172.0242f, 22, -361.1372f);
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
			int index = int.Parse(GUI.tooltip);
			Item item = inventory[index];
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
		tooltipStyle.wordWrap = true;
		tooltipStyle.fontSize = 14;
		tooltipStyle.normal.textColor = toolTipColor;
		tooltipStyle.alignment = TextAnchor.MiddleCenter;
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
		
		int[] x = {175, 120, 175, 230, 175};
		
		int[] y = {20, 120, 120, 120, 220};
		
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
	
	// Use this for initialization
	void Start () {
		
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
		
		level = 1;
		
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
		
		TwoHandedSword startingSword = new TwoHandedSword();
		startingSword.randomizeWeapon(1, 1);
		awardItem(startingSword);
		
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
		playerPortrait = Resources.Load("CheckpointTexture/garen_circle") as Texture2D;		
		playerManaTexture = Resources.Load("PlayerTextures/mana") as Texture2D;
		
		leftClickOverlay = Resources.Load("InstructionPage/leftclick") as Texture2D;
		rightClickOverlay = Resources.Load("InstructionPage/rightclick") as Texture2D;
		
		winTexture = Resources.Load("GUITextures/victoryFull") as Texture2D;
		lossTexture = Resources.Load("GUITextures/defeatFull") as Texture2D;
		continueButton = Resources.Load("InstructionPage/Continue") as Texture2D;
		continueButtonHighlighted = Resources.Load("InstructionPage/Continue2") as Texture2D;
					
		equipment = new Item[5]; //TODO decide on size
		
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
		
		Save();
	}
	
	// Update is called once per frame
	void Update () {
		
		if(!alive)
			return;
		
		//Currently, we are experiencing a bug where Garen will randomly rotate himself when attacking,
		//causing movements to be rejected. Until we are able to better understand the reason behind this
		//rotation, we have applied a temporary fix. This should be overridden as soon as a solution
		//becomes apparent.
		GameObject topObject = GameObject.FindGameObjectWithTag("TopLevelObject");
		Vector3 newPosition = topObject.transform.position;
		newPosition.y = 22;
		topObject.transform.position = newPosition;
		
		Quaternion rot = topObject.transform.localRotation;
		rot.y = 331.0f;
		rot.x = 0;
		rot.z = 0;
		topObject.transform.localRotation = rot;
		
		
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
		
		if(lastCombatTime + healthRegenDelay < Time.time){
			if(currentHealth < maxHealth){
				currentHealth += Time.fixedDeltaTime * (healthRegenRate / maxHealth) * 400;
				if(currentHealth > maxHealth)
					currentHealth = maxHealth;
			}
		}
		
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
		
		else if(Input.GetButtonDown("Fire2") && !guiInteraction(Input.mousePosition)){ //Right click
			valor.Execute();
		}
		
		if(running && !a.IsPlaying("Run") && !a.IsPlaying("Spell3")) {
			//a.Stop();
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
	/// Specifies if the Player is currently in a stunned state. Stunned players are unable to
	/// move, attack, or loot. A Player may be stunned by enemy spells, or due to having recently
	/// performed an attack.
	/// </summary>
	public bool stunned(){
		return Time.time < stunTime;		
	}
	
	/// <summary>
	/// Stun the Player for the specified time.
	/// </summary>
	/// <param name='time'>
	/// Time for Player to remain stunned.
	/// </param>
	public void Stun(float time){
		this.stunTime = Time.time + time;	
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
				if(!System.IO.File.Exists("saves/Garen"+i+".txt")){
					name = "saves/Garen" + i + ".txt";
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
		
		return false;
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
		
		//Add each item
		foreach(Item item in equipment){
			if(item == null)
				continue;
			item.Equip(this);
		}
		
		//Class specific enhancements based on stats
		this.weaponDamage += this.strength;
		this.maxHealth = 100 + (vitality * 10);
		
		if(this.currentHealth > this.maxHealth)
			this.currentHealth = this.maxHealth;
	}
	
	/// <summary>
	/// Begins a randomly chosen idle animation.
	/// </summary>
	public void playIdleSequence(){
		if(!a.isPlaying){
			int num = ((int) (Random.value * 4)) + 1;
			a.Play("Idle"+num);
		}
		
		idling = true;
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
	/// Plays the specified animation, if exists.
	/// </summary>
	/// <param name='animationName'>
	/// The name of the animation to play.
	/// </param>
	public void playAnimation(string animationName){
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
	/// Sets Player's spinning state, and plays the appropriate animation. The Player's spinning
	/// state determines which animations will be played for future movements.
	/// </summary>
	/// <param name='active'>
	/// The Player's new spinning state.
	/// </param>
	public void setSpinning(bool active){
		string animationName = active?"Spell3":"Idle2";
		spinning = active;
			a.Stop();
			a.Play(animationName);
	}
	
	/// <summary>
	/// Determines if the Player is currently spinning.
	/// </summary>
	/// <returns>
	/// The Player's spinning state.
	/// </returns>
	public bool isSpinning(){
		return spinning;	
	}
	
	/// <summary>
	/// Sets the Player's courage state. When active, courage reduces the damage the Player takes
	/// from attacks by a flat percentage.
	/// </summary>
	/// <param name='active'>
	/// The Player's new courage state.
	/// </param>
	public void activateCourage(bool active){
			courageActive = active;
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
	/// Gets the GameObject to which the Player is attached.
	/// </summary>
	/// <returns>
	/// Player's GameObject.
	/// </returns>
	public GameObject getGameObject(){
		return gameObject;	
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
	
	/// <summary>
	/// Performs a simple auto-attack on the specified player. The calling procedure is responsible
	/// to ensure that the auto-attack is only called when off cooldown.
	/// </summary>
	/// <returns>
	/// The new cooldown of the auto-attack.
	/// </returns>
	/// <param name='enemy'>
	/// The enemy to attack.
	/// </param>
	public float autoAttack(EnemyScript enemy){
			a.Stop();
			setRunning(false);
			a.Play("Attack" + (((int)(Random.value * 3)) + 1));
			enemy.damage(WeaponDamage);
			ValorDebuff debuff = new ValorDebuff(valorDebuffTexture);
			enemy.applyDebuff(debuff, true);
			lastCombatTime = Time.time;
			stunTime = Time.time + 1f; //Delay after attack
			return Time.time + WeaponSpeed;
	}
	
	/// <summary>
	/// Decisive Strike attacks enemies in a cone in front of the Player for 160% Weapon Damage.
	/// </summary>
	class Decisive_Strike : Ability {
		
		private GarenScript player;
		private double totalDamage;
		private float nextAttack = 0;
		private float coolDown = 8;
		private string animationName = "Spell1";
		
		
		public void Execute(){
			
			totalDamage = player.getWeaponDamage() * 1.6f;
			
			//Check if the ability is on cooldown
			if(nextAttack > Time.time)
				return;
			
			player.setRunning(false);
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
					player.lastCombatTime = Time.time;
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
	
	/// <summary>
	/// Courage reduces damage taken by the Player by 30% for a specified period of time.
	/// </summary>
	class Courage : Ability {
		private GarenScript player;
		private bool active = false;
		private float cooldown = 20;
		private float nextAttack = 0;
		private float duration = 3;
		private float courageEndTime;
		private string animationName = "Spell2";
	
		
		public void Update(){
			if(courageEndTime < Time.time && active){
				active = false;
				player.activateCourage(false);
			}
		}
		
		public void Execute(){
			
			//Check if on cooldown
			if(Time.time < nextAttack)
				return;
			
			player.setRunning(false);
			player.activateCourage(true);
			nextAttack = Time.time + cooldown;
			player.playAnimation(animationName);
			player.applyDebuff(new CourageBuff());
			courageEndTime = Time.time + duration;
			active = true;
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
	
	/// <summary>
	/// Judgement hits all enemies around the Player for 180% Weapon Damage over three seconds. Damage is
	/// evenly distributed over the three seconds, and enemies which leave the damage radius between ticks
	/// do not suffer additional damage.
	/// </summary>>
	class Judgement : Ability {
		private GarenScript player;
		private float nextTickTime = 0;
		private int radius = 30;
		private int numTicks = 0;
		private double totalDamage;
		private double damagePerTick = 0;
		private double nextAttack = 0;
		private double coolDown = 20;
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
					player.lastCombatTime = Time.time;
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
	
	/// <summary>
	/// Demacian Justice attacks up to five targets in a cone in front of the Player for 300% Weapon Damage.
	/// Damage is evenly distruted among targets, but will always sum to 300% total damage.
	/// <example>
	/// If a single target is hit by the ability, it will suffer 300% Weapon Damage. If three targets are hit,
	/// each will suffer 100% Weapon Damage.
	/// </example>
	/// </summary>	
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
			
			player.setRunning(false);
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
			player.lastCombatTime = Time.time;
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
	
	/// <summary>
	/// Valor attacks up to three targets in a cone in front of the Player for 50% Weapon Damage. This damage
	/// is not split between targets.
	/// </summary>	
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
			
			player.setRunning(false);
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
			
			player.lastCombatTime = Time.time;
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
