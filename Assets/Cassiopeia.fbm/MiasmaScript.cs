using UnityEngine;
using System.Collections;

public class MiasmaScript : MonoBehaviour {
	private float nextTick = Time.time + 4;
	private float lifeTime;
	
	private float particle_radius;
	private float collider_radius;
	
	private ParticleSystem particleSystem;
	private SphereCollider collider;
	
	// Use this for initialization
	void Start () {
		lifeTime = Time.time + 7;
		particleSystem = gameObject.GetComponent(typeof(ParticleSystem)) as ParticleSystem;
		collider = gameObject.GetComponent(typeof(SphereCollider)) as SphereCollider;
		
		//To change the radius of the miasma, simple change the lifetime of each particle
		particle_radius = particleSystem.startLifetime;
	}
	
	// Update is called once per frame
	void Update () {
		if(Time.time > lifeTime)
			Destroy(gameObject);
	}
	
	public void trigger(PlayerScript player){
		//TODO This should apply a debuff that should damage the player, and automatically wear off after a set duration.
		if (Time.time > nextTick){
			//player.damage(4);
			particle_radius += 2;
			particleSystem.startLifetime = particle_radius;
			
			collider_radius += 13;
			collider.radius = collider_radius;
			Debug.Log("miasma!");
			nextTick = Time.time + 3;
		}
	}
}
