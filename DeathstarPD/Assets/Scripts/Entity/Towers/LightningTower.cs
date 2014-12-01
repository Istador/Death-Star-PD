using UnityEngine;
using System.Collections;

public class LightningTower : Tower {

	private static int[] health_table = { 125, 150, 175, 200 }; // Trefferpunkte
	public override int[] MaxHealthTable { get{ return health_table; } }

	private static int[] damage_table = { 2, 3, 4, 5 }; // Schaden pro Treffer
	public override int[] DamageTable { get{ return damage_table; } }

	private static float[] range_table = { 20f, 22f, 24f, 26f }; // maximale Distanz zum Ziel
	public override float[] RangeTable { get{ return range_table; } }

	private static float[] attack_cooldown_table = { 0.5f, 0.45f, 0.4f, 0.35f }; // Zeit in Sekunden zw. Angriffen
	public override float[] AttackCooldownTable { get{ return attack_cooldown_table; } }

	public override EAttackPattern AttackPattern { get {return EAttackPattern.MultiTarget;} }

	protected override void DoAttack(MovableEntity target){
		//TODO Lars: anderen Angriffseffekt für den Blitzturm

		PLazer lazer = Instantiate("Lazer", Pos + Pos.normalized * transform.localScale.z * 0.25f).GetComponent<PLazer>();
		lazer.target = target;
		lazer.transform.parent = ProjectileManager.Container;
		
		DoDamage(target, Damage, 0.5f);
	}

}
