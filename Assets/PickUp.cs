using UnityEngine;
using System.Collections;

public interface PickUp {

	// Use this for initialization
	void Start ();
	
	// Update is called once per frame
	void Update ();
	
	// Primarily function to be implemented by child classes.
	void trigger(GarenScript player);
	
}
