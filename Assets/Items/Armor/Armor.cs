using UnityEngine;
using System.Collections;

public interface Armor {
	
	#region Armor Stats
	
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
	
	void randomizeArmor(int level);
}
