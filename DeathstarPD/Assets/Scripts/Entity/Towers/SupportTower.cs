using UnityEngine;
using System.Collections.Generic;

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

	private static float[] attack_cooldown_table = { 0f, 0f, 0f, 0f }; // Zeit in Sekunden zw. Angriffen
	public override float[] AttackCooldownTable { get{ return attack_cooldown_table; } }

	public override EAttackPattern AttackPattern { get {return EAttackPattern.None;} }

	protected override void DoAttack(MovableEntity target){}


	private HashSet<Tower> towers = new HashSet<Tower>();



	protected override void Start(){

		//Finde Türme in Reichweite
		Collider[] hits = Physics.OverlapSphere(Pos, Range, (int)Layer.Tower);
		foreach(Collider hit in hits){
			Tower t = hit.GetComponent<Tower>();
			//falls am leben, und nicht er selbst
			if(t != null && !t.IsDead && t.enabled && !t.Equals(this)){
				t.Buffs++;
				towers.Add(t);
			}
		}

		//werde laufend über neue/tote Türme informiert
		Observer.I.Add("AddedTower", this);
		Observer.I.Add("RemovedTower", this);

		base.Start();
	}



	protected void OnDestroy(){
		//Entferne den Buff von allen Türmen
		foreach(Tower t in towers) if(t != null && !t.IsDead && t.enabled){
			t.Buffs--;
		}
		towers.Clear();

		//benötigt keine Updates mehr
		Observer.I.Remove("AddedTower", this);
		Observer.I.Remove("RemovedTower", this);
	}



	public override bool HandleMessage(Telegram msg){
		if(this == null) return true;
		Tower t = msg.extraInfo as Tower;
		switch(msg.message){
		case "AddedTower":
			//Lebt, und nicht er selbst
			if(t != null && !t.IsDead && t.enabled && !t.Equals(this)){
				//ist in Reichweite
				if(DistanceSqTo(t) <= Range * Range){
					t.Buffs++;
					towers.Add(t);
				}
			}
			return true;
		case "RemovedTower":
			if(towers.Contains(t)){
				towers.Remove(t);
			}
			return true;
		default: return base.HandleMessage(msg);
		}
	}



}
