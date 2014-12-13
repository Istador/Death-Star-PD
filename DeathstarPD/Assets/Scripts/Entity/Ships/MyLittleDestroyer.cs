using UnityEngine;
using System.Collections;

public class MyLittleDestroyer : Ship {



	public override int Damage { get{ return 2; } }
	public override float Range { get{ return 18f; } }
	public override float AttackCooldown { get{ return 0.5f; } }
	


	protected override void Start(){
		MaxHealth = 40;
		
		MaxSpeed = 25f;
		MaxForce = 20f;

		// Positionen der 4 Kanonen
		l = transform.FindChild("left");
		r = transform.FindChild("right");

		base.Start();
	}



	private Transform l, r;

	// Ziel angreifen
	public override void DoAttack(Entity target){
		// ein zufällige Kanone abfeuern
		switch(Utility.Rnd.Next(0, 2)){
		default: lazer(target, l.position); break;
		case 1:  lazer(target, r.position); break;
		}

		//Schaden verursachen
		DoDamage(target, Damage, AttackCooldown * 0.5f);
	}

	// Laser Effekt darstellen
	private void lazer(Entity target, Vector3 pos){
		PLazer lazer = Instantiate("Lazer", pos).GetComponent<PLazer>();
		lazer.target = target;
		lazer.duration = AttackCooldown * 0.5f; // Anzeigedauer für die hälfte der Angriffszeit
		lazer.preload = 0.0f; // Sofort anzeigen
		lazer.lr.SetColors(Color.red, Color.red); // Rot statt grün
		lazer.particleSystem.renderer.enabled = false; // Keine Partikel, nur Laser
		lazer.transform.parent = ProjectileManager.Container;
	}

}
