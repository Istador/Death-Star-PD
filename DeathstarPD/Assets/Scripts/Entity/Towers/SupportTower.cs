using UnityEngine;
using System.Collections;

public class SupportTower : Tower {

	private static int[] health_table = { 150, 200, 250, 300 }; // Trefferpunkte
	public override int[] MaxHealthTable { get{ return health_table; } }

	public readonly static int[] cookie_table = { -1, -1, -1, -1 };
	public override int[] CookieTable { get{ return cookie_table;} }

	public readonly static int[] money_table = { 300, 400, 500, 600 };
	public override int[] MoneyTable { get{ return money_table;} }

	private static int[] damage_table = { 0, 0, 0, 0 }; // Schaden pro Treffer
	public override int[] DamageTable { get{ return damage_table; } }

	private static float[] range_table = { 30f, 35f, 40f, 45f }; // maximale Distanz zum Ziel
	public override float[] RangeTable { get{ return range_table; } }

	private static float[] attack_cooldown_table = { 1f, 1f, 1f, 1f }; // Zeit in Sekunden zw. Angriffen
	public override float[] AttackCooldownTable { get{ return attack_cooldown_table; } }

	public override EAttackPattern AttackPattern { get {return EAttackPattern.None;} }

	private Transform projectilePos;
	
	protected override void Start(){
		// TODO suche nahe Türme
		base.Start();
	}

	protected override void DoAttack(MovableEntity target){}

}
