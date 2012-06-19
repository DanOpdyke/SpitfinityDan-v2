using UnityEngine;
using System.Collections;

public interface Item {
	
	Texture2D getTexture();
	
	void Equip();
	
	string getStats();
	
	int getItemRarity();
	
	Item getCopy();
	
	void randomize(int level);
}
