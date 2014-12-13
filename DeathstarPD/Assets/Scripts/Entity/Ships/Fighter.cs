using UnityEngine;
using System.Collections;

public class Fighter : Ship {



	public override int Damage { get{ return 10; } }
	public override float Range { get{ return 20f; } }
	public override float AttackCooldown { get{ return 1f; } }
	


	protected override void Start(){
		MaxHealth = 100;
		
		MaxSpeed = 20f;
		MaxForce = 15f;

		// Positionen der 4 Kanonen
		tl = transform.FindChild("topleft");
		tr = transform.FindChild("topright");
		bl = transform.FindChild("bottomleft");
		br = transform.FindChild("bottomright");

		base.Start();
	}



	Transform tl, tr, bl, br;

	// Ziel angreifen
	public override void DoAttack(Entity target){
		// ein zufällige Kanone abfeuern
		switch(Utility.Rnd.Next(0, 4)){
		default: lazer(target, tl.position); break;
		case 1:  lazer(target, tr.position); break;
		case 2:  lazer(target, bl.position); break;
		case 3:  lazer(target, br.position); break;
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
