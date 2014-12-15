using UnityEngine;
using System.Collections;

public class LightningTower : Tower {

	private static int[] health_table = { 125, 150, 175, 200 }; // Trefferpunkte
	public override int[] MaxHealthTable { get{ return health_table; } }

	public readonly static int[] cookie_table = { 1, 0, 1, 2 };
	public override int[] CookieTable { get{ return cookie_table;} }
	
	public readonly static int[] money_table = { 200, 300, 400, 500 };
	public override int[] MoneyTable { get{ return money_table;} }

	private static int[] damage_table = { 2, 3, 4, 5 }; // Schaden pro Treffer
	public override int[] DamageTable { get{ return damage_table; } }

	private static float[] range_table = { 20f, 22f, 24f, 26f }; // maximale Distanz zum Ziel
	public override float[] RangeTable { get{ return range_table; } }

	private static float[] attack_cooldown_table = { 0.5f, 0.45f, 0.4f, 0.35f }; // Zeit in Sekunden zw. Angriffen
	public override float[] AttackCooldownTable { get{ return attack_cooldown_table; } }

	public override EAttackPattern AttackPattern { get {return EAttackPattern.MultiTarget;} }

	private Transform projectilePos;

	protected override void Start (){
		projectilePos = transform.FindChild("particles1");
		
		base.Start();
	}

	protected override void DoAttack(MovableEntity target){
		PLightning lightning = Instantiate("LightningShot", projectilePos.position).GetComponent<PLightning>();
		lightning.target = target;
		lightning.transform.parent = ProjectileManager.Container;
		
		DoDamage(target, Damage, 0.5f);
	}

}
