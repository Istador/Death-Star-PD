using UnityEngine;
using System.Collections;

public class RocketTower : Tower {
	
	private static int[] health_table = { 80, 90, 100, 110 }; // Trefferpunkte
	public override int[] MaxHealthTable { get{ return health_table; } }

	private static int[] cookie_table = { 2, 4, 6, 8 };
	public override int[] CookieTable { get{ return cookie_table;} }
	
	private static int[] money_table = { 500, 600, 700, 800 };
	public override int[] MoneyTable { get{ return money_table;} }

	private static int[] damage_table = { 40, 50, 60, 70 }; // Schaden pro Treffer
	public override int[] DamageTable { get{ return damage_table; } }
	
	private static float[] range_table = { 20f, 24f, 28f, 32f }; // maximale Distanz zum Ziel
	public override float[] RangeTable { get{ return range_table; } }
	
	private static float[] attack_cooldown_table = { 3.0f, 2.8f, 2.6f, 2.4f }; // Zeit in Sekunden zw. Angriffen
	public override float[] AttackCooldownTable { get{ return attack_cooldown_table; } }

	public override EAttackPattern AttackPattern { get {return EAttackPattern.SingleTarget;} }

	private Transform projectilePos;

	protected override void Start (){
		projectilePos = transform.FindChild("rocketTurretUP.uvg").FindChild("rocket_barrelAnchor").FindChild("rocketTurretGun.uvg").FindChild("projectilePos");

		base.Start();
	}

	protected override void DoAttack(MovableEntity target){

		//nicht in der mitte erzeugen, sondern an Abschussposition (Mitte der oberen Hälfte)
		PRocket r = Instantiate("Rocket", projectilePos.position).GetComponent<PRocket>();
		r.transform.parent = ProjectileManager.Container;
		r.Owner = this;
		r.Init(target);
	}
	
}

