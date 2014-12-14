using UnityEngine;
using System.Collections;

public class LaserTower : Tower {

	private static int[] health_table = { 100, 125, 150, 175 }; // Trefferpunkte
	public override int[] MaxHealthTable { get{ return health_table; } }

	private static int[] cookie_table = { 1, 2, 3, 4 };
	public override int[] CookieTable { get{ return cookie_table;} }

	private static int[] money_table = { 250, 300, 350, 400 };
	public override int[] MoneyTable { get{ return money_table;} }

	private static int[] damage_table = { 10, 12, 14, 16 }; // Schaden pro Treffer
	public override int[] DamageTable { get{ return damage_table; } }

	private static float[] range_table = { 20f, 24f, 28f, 32f }; // maximale Distanz zum Ziel
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
