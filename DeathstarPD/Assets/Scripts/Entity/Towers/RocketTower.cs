using UnityEngine;
using System.Collections;

public class RocketTower : Tower {

	/*
	public int test_health = 80;
	public int test_damage = 20; //20 Schaden pro Treffer
	public float test_range = 8f;
	public float test_cooldown = 3f; // 0,33~ Angriffe die Sekunde
	*/
	
	private static int[] health_table = { 80, 90, 100, 110 }; // Trefferpunkte
	public override int[] MaxHealthTable { get{ return health_table; } }
	
	private static int[] damage_table = { 20, 25, 30, 35 }; // Schaden pro Treffer
	public override int[] DamageTable { get{ return damage_table; } }
	
	private static float[] range_table = { 18f, 22f, 26f, 30f }; // maximale Distanz zum Ziel
	public override float[] RangeTable { get{ return range_table; } }
	
	private static float[] attack_cooldown_table = { 3.0f, 2.8f, 2.6f, 2.4f }; // Zeit in Sekunden zw. Angriffen
	public override float[] AttackCooldownTable { get{ return attack_cooldown_table; } }

	public override EAttackPattern AttackPattern { get {return EAttackPattern.SingleTarget;} }

	private Vector3 projectilePos;

	protected override void Start (){
		BoxCollider bc = GetComponent<BoxCollider>();
		float height = (bc.size.y + bc.center.y) * transform.localScale.y;
		projectilePos = Pos + Pos.normalized * height * 0.325f;

		base.Start();
	}

	protected override void DoAttack(MovableEntity target){

		//nicht in der mitte erzeugen, sondern an Abschussposition (Mitte der oberen Hälfte)
		PRocket r = Instantiate("Rocket", projectilePos).GetComponent<PRocket>();
		r.transform.parent = ProjectileManager.Container;
		r.Owner = this;
		r.Init(target);
	}
	
}

