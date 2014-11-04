using UnityEngine;
using System.Collections;

public class LaserTower : Tower {

	/*
	public int test_health = 100;
	public int test_damage = 10; //10 Schaden pro Treffer
	public float test_range = 10f;
	public float test_cooldown = 0.5f; // 2 Angriffe die Sekunde
	*/

	private static int[] health_table = { 100, 125, 150, 175 }; // Trefferpunkte
	public override int[] MaxHealthTable { get{ return health_table; } }

	private static int[] damage_table = { 10, 12, 14, 16 }; // Schaden pro Treffer
	public override int[] DamageTable { get{ return damage_table; } }

	private static float[] range_table = { 10f, 12f, 14f, 16f }; // maximale Distanz zum Ziel
	public override float[] RangeTable { get{ return range_table; } }

	private static float[] attack_cooldown_table = { 0.5f, 0.6f, 0.7f, 0.8f }; // Angriffe die Sekunde
	public override float[] AttackCooldownTable { get{ return attack_cooldown_table; } }

	protected override void DoAttack(){
		//TODO: sichtbaren Effekt erzeugen
		DoDamage(Target, Damage);
	}

}
