using UnityEngine;
using System.Collections;

public class LaserTower : Tower {

	public int test_health = 100;
	public int test_damage = 10; //10 Schaden pro Treffer
	public float test_range = 10f;
	public float test_cooldown = 0.5f; // 2 Angriffe die Sekunde


	//private int[] health_table = { 100, 125, 150, 175 };
	//public override int CalcMaxHealth(int level){return health_table[level-1];}
	//public override int CalcMaxHealth(int level){return 75 + level*25;}
	public override int CalcMaxHealth(int level){return test_health;}

	public override int CalcDamage(int level){return test_damage;}

	public override float CalcRange(int level){return test_range;}

	public override float CalcAttackCooldown(int level){return test_cooldown;}



	protected override void DoAttack(){
		//TODO: sichtbaren Effekt erzeugen
		DoDamage(Target, Damage);
	}

}
