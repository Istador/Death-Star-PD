using UnityEngine;
using System.Collections;

/// 
/// Raketen verfolgen Gegner
/// 
public class PRocket : Projektile<RocketTower> {


	/// <summary>
	/// Das Ziel, dass verfolgt wird
	/// </summary>
	private MovableEntity Target;


	
	/// <summary>
	/// Explosionsradius der Rakete
	/// </summary>
	private static readonly float f_explosionsRadius = 3f;



	/// <summary>
	/// Schaden den die Explosion maximal verursachen kann
	/// </summary>
	private float f_explosionsDamage;



	/// <summary>
	/// Schaden den das Projektil bei Gegnern verursacht.
	/// 0, weil kein direkter Schaden, sondern Flächenschaden.
	/// </summary>
	/// <value>
	/// in ganzen Trefferpunkten
	/// </value>
	protected override int Damage { get{return 0;} }



	/// <summary>
	/// Zeitpunkt an dem die Rakete erstellt wurde, um die Lebenszeit berechnen zu 
	/// können und wenn es so weit ist zu sterben.
	/// </summary>
	private double d_startTime;
	
	
	
	/// <summary>
	/// Zeit in Sekunden nach der die Rakete automatisch stirbt
	/// </summary>
	private double d_timeToLife; // in Sekunden
	


	/// <summary>
	/// Richtung in die der Gegner guckt.
	/// Lokales Koordinatensystem.
	/// </summary>
	public override Vector3 Heading {get{return TargetPos - Pos;}}
	
	

	public override void Init(){
		Target = Owner.Target;
		f_explosionsDamage = Owner.Damage;
		d_timeToLife = Owner.Range * 2f;
		
		//Zielposition setzen
		TargetPos = Target.Pos;
		
		base.Init();
		
		//Geschwindigkeit setzen
		MaxSpeed = 15.0f;
		MaxForce = 15.0f;
		
		//Startzeitpunkt merken
		d_startTime = Time.time;
	}
	
	
	
	protected override void FixedUpdate() {
		//Target lebt nicht mehr?
		if(Target == null || !Target.enabled || Target.IsDead){
			Death();
			return;
		}

		TargetPos = Target.Pos;

		Debug.DrawLine(Pos, TargetPos, Color.green);
				
		//Rotieren zum Ziel, Bewegung umsetzen
		base.FixedUpdate();
		

		//Sterben nach einer bestimmten Zeit oder Ziel erreicht
		if(d_startTime + d_timeToLife <= Time.time || DistanceSqTo(TargetPos) < 0.1)
			Death();
	}
	
	
	
	//Überschreiben um beim Tod zu explodieren
	public override void Death(){
		//wer Kollidiert alles mit der runden Explosion
		Collider[] cs = Physics.OverlapSphere(Pos, f_explosionsRadius, (int)Layer.Enemy);
		foreach(Collider c in cs){
			//Entfernung des Objektes zum Explosionszentrum berechnen
			float range = DistanceSqTo(c.bounds.center) / (f_explosionsRadius * f_explosionsRadius);

			//Schaden proportional zur Entfernung berechnen
			int dmg = (int)( f_explosionsDamage * (1.0f - range) );
				
			//Schadensmeldung verschicken
			if(dmg > 0) Owner.DoDamage(c, dmg);
		}

		//TODO Lars: sichtbare Explosion
		//GameObject explosion = Instantiate("prefab kleineExplosion");
		//nach 0.5 Sekunden Explosion wieder auflösen
		//Destroy(explosion, 1f);
		
		//TODO: Explosionsgeräusch
		//PlaySound("explode");
		
		//ansonsten normal sterben
		base.Death();
	}
	
	
	
}