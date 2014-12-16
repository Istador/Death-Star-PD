using UnityEngine;
using System.Collections;

public class MGTower : Tower {
	
	private static int[] health_table = { 50, 60, 70, 80 }; // Trefferpunkte
	public override int[] MaxHealthTable { get{ return health_table; } }

	public readonly static int[] cookie_table = { 1, 0, 1, 1 };
	public override int[] CookieTable { get{ return cookie_table;} }
	
	public readonly static int[] money_table = { 100, 150, 200, 250 };
	public override int[] MoneyTable { get{ return money_table;} }

	private static int[] damage_table = { 5, 6, 8, 10 }; // Schaden pro Treffer
	public override int[] DamageTable { get{ return damage_table; } }
	
	private static float[] range_table = { 30f, 32.5f, 35f, 37.5f }; // maximale Distanz zum Ziel
	public override float[] RangeTable { get{ return range_table; } }
	
	private static float[] attack_cooldown_table = { 0.25f, 0.225f, 0.20f, 0.175f }; // Zeit in Sekunden zw. Angriffen
	public override float[] AttackCooldownTable { get{ return attack_cooldown_table; } }

	public override EAttackPattern AttackPattern { get {return EAttackPattern.SingleTarget;} }

	private Transform left, right;
	private bool seitenwechsel = false;

	protected override void Start (){
		Transform p = transform.FindChild("turretUP.uvg").FindChild("mg_barrelAnchor").FindChild("turretGuns.uvg");
		left = p.FindChild("left");
		right = p.FindChild("right");

		base.Start();
	}

	protected override void DoAttack(MovableEntity target){
		//nicht in der mitte erzeugen, sondern an Abschussposition
		Vector3 pos = Vector3.zero;
		if(seitenwechsel) pos = left.position;
		else pos = right.position;
		seitenwechsel = !seitenwechsel;

		//Erzeugen
		PTurboLaser r = Instantiate("TurboLaser", pos).GetComponent<PTurboLaser>();
		r.target = target;
		r.transform.parent = ProjectileManager.Container;

		DoDamage(target, Damage, 0.5f);

		if(audio != null && audio.enabled)
			gameObject.GetComponent<AudioSource>().Play();
	}
	
}

