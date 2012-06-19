using UnityEngine;
using System.Collections;

public interface PlayerScript {
	
	void playIdleSequence();
	
	bool isRunAnimation();
	
	bool noAnimation();
	
	bool isAlive();
	
	void stopAnimation();
	
	float getHealthPercent();
	
	void setCurrentEnemy(EnemyScript enemy);
	
	void setIdling(bool idle);

	EnemyScript getCurrentEnemy();
	
	float getWeaponDamage();
	
	float getWeaponSpeed();
	
	void playAnimation(string animationName);
	
	void setRunning(bool active);
	
	float getRange();

	void awardHealth(float amount);

	void damage(float amount);
	
	float autoAttack(EnemyScript enemy);
	
	GameObject getGameObject();
	
	bool guiInteraction(Vector3 mousePosition);
	
	void awardItem(Item item);
	
	bool stunned();
	
	void setPoisoned(bool poisoned);
	
	bool isPoisoned();
}
