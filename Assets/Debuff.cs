using UnityEngine;
using System.Collections;

public interface Debuff {

	// Use this for initialization
	void Start ();
	
	// Update is called once per frame
	void Update ();
	
	// Returns boolean indicating if the debuff is expired
	bool hasExpired();
	
	// Calculate how this debuff affects damage done
	float applyDebuff(float damage);
	
	// Applies additional stacks of the debuff
	void applyStack(int numAdditionalStacks);
	
	Texture2D getTexture();
}
