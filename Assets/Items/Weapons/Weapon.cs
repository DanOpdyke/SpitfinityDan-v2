using UnityEngine;
using System.Collections;

public interface Weapon {
	
	#region Weapon Stats
	float WeaponDamage{
		get;
		set;
	}
	
	float WeaponSpeed{
		get;
		set;
	}
	
	int Strength{
		get;
		set;
	}
	
	int Dexterity{
		get;
		set;
	}
	
	int Intelligence{
		get;
		set;
	}
	
	int Vitality{
		get;
		set;
	}
	
	string Name{
		get;
		set;
	}
	
	#endregion
	
	string getStats();
	
	void randomizeWeapon(int level);
}
