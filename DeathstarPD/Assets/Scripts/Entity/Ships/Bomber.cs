using UnityEngine;
using System.Collections;

public class Bomber : Ship {

	/// <summary>
	/// wie oft geupdatet werden soll.
	/// default: 7 (jeden 7. Frame)
	/// </summary>
	public static readonly int update_each_x_frames = 7;
	/// <summary>
	/// Dieser spezifische Bomber updatet bei Frame Nr. x
	/// </summary>
	public readonly int update_on_frame = Utility.Rnd.Next(update_each_x_frames);

	public override int Damage { get{ return 20; } }
	public override float Range { get{ return 15f; } }
	public override float AttackCooldown { get{ return 1.5f; } }

	/// <summary>
	/// Distanz, in welcher der Bomber seinen Kurs ändert um Türme anzugreifen
	/// </summary>
	public static readonly float f_towerRange = 30f;


	
	/// <summary>
	/// Das Sub-Ziel, dass verfolgt wird, also ein Turm, falls in Reichweite
	/// </summary>
	public Tower SubTarget { get; set; }


	protected override void Start(){
		MaxHealth = 70;
		
		MaxSpeed = 15f;
		MaxForce = 10f;

		// Positionen der Kanone
		bpos = transform.FindChild("bombpos");

		MoveFSM.CurrentState = SBomberLanding.I;

		base.Start();
	}



	private Transform bpos;

	// Ziel angreifen
	public override void DoAttack(Entity target){
		// Kanone abfeuern
		shoot(target, bpos.position);

		//Schaden verursachen
		DoDamage(target, Damage, AttackCooldown * 0.5f);
	}

	// Schuss-Effekt darstellen
	private void shoot(Entity target, Vector3 pos){
		//TODO Lars: Bomben-Effekt statt Laser-Effekt
		PLazer lazer = Instantiate("Lazer", pos).GetComponent<PLazer>();
		lazer.target = target;
		lazer.duration = AttackCooldown * 0.5f; // Anzeigedauer für die hälfte der Angriffszeit
		lazer.preload = 0.0f; // Sofort anzeigen
		lazer.lr.SetColors(Color.red, Color.red); // Rot statt grün
		lazer.particleSystem.renderer.enabled = false; // Keine Partikel, nur Laser
		lazer.transform.parent = ProjectileManager.Container;
	}

}
