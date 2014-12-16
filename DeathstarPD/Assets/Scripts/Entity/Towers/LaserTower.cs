using UnityEngine;
using System.Collections;

public class LaserTower : Tower {

	private static int[] health_table = { 100, 125, 150, 175 }; // Trefferpunkte
	public override int[] MaxHealthTable { get{ return health_table; } }

	public readonly static int[] cookie_table = { 1, 1, 0, 0 };
	public override int[] CookieTable { get{ return cookie_table;} }

	public readonly static int[] money_table = { 150, 175, 250, 325 };
	public override int[] MoneyTable { get{ return money_table;} }

	private static int[] damage_table = { 25, 30, 35, 40 }; // Schaden pro Treffer
	public override int[] DamageTable { get{ return damage_table; } }

	private static float[] range_table = { 35f, 40f, 45f, 50f }; // maximale Distanz zum Ziel
	public override float[] RangeTable { get{ return range_table; } }

	private static float[] attack_cooldown_table = { 0.8f, 0.7f, 0.6f, 0.5f }; // Zeit in Sekunden zw. Angriffen
	public override float[] AttackCooldownTable { get{ return attack_cooldown_table; } }

	public override EAttackPattern AttackPattern { get {return EAttackPattern.SingleTarget;} }

	private Transform projectilePos;
	
	protected override void Start (){
		projectilePos = transform.FindChild("laserUp").FindChild("laser_barrelAnchor").FindChild("satelliteturretUP.uvg").FindChild("projectilePos");
		
		base.Start();
	}

	protected override void DoAttack(MovableEntity target){

		PLazer lazer = Instantiate("Lazer", projectilePos.position).GetComponent<PLazer>();
		lazer.target = target;
		lazer.transform.parent = ProjectileManager.Container;

		DoDamage(target, Damage, 0.5f);
	}

}
