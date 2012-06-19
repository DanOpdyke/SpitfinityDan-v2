using UnityEngine;
using System.Collections;

public class ItemLootScript : MonoBehaviour {
	private Item item;
	private int level = 1;
	private Color[] itemRarityColors = {
		Color.gray, Color.white, Color.green, Color.blue, Color.yellow, Color.red	
	};
	
	private static Item[] items = {
		new TwoHandedSword(),
		new Helm(),
		new Chest(),
		new Boots(),
		new Gloves()
	};
	
	public void setLevel(int level){
		this.level = level;
	}

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void setItem(Item item){
		this.item = item;
		ParticleSystem particleSystem = gameObject.GetComponent(typeof(ParticleSystem)) as ParticleSystem;
		particleSystem.startColor = itemRarityColors[item.getItemRarity()];
	}
	
	public Item getItem(){
		return this.item;
	}
	
	public void randomizeItem(){
		this.item = items[Random.Range(0, items.Length)].getCopy();
		this.item.randomize(level);
		ParticleSystem particleSystem = gameObject.GetComponent(typeof(ParticleSystem)) as ParticleSystem;
		particleSystem.startColor = itemRarityColors[item.getItemRarity()];
	}
}
