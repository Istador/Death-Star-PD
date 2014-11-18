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

	private static float[] range_table = { 20f, 24f, 28f, 32f }; // maximale Distanz zum Ziel
	public override float[] RangeTable { get{ return range_table; } }

	private static float[] attack_cooldown_table = { 0.5f, 0.6f, 0.7f, 0.8f }; // Angriffe die Sekunde
	public override float[] AttackCooldownTable { get{ return attack_cooldown_table; } }

	protected override void DoAttack(){
		//TODO Lars: sichtbaren Effekt für Laserstrahl erzeugen
		GameObject go = (GameObject)Instantiate("Lazer", Pos + Pos.normalized * transform.localScale.z * 0.25f);
		PLazer lazer = go.GetComponent<PLazer>();
		lazer.target = Target;
		go.transform.parent = ProjectileManager.Container;

		DoDamage(Target, Damage);
		//DoDamage(Target, Damage, 0.5); //Alternativ: erst nach 0,5 sekunden Schaden verursachen
	}

}
