using UnityEngine;
using System.Collections;

/// 
/// Raketen verfolgen Gegner
/// 
public class PRocket : Projektile<Tower> {


	/// <summary>
	/// Das Ziel, dass verfolgt wird
	/// </summary>
	private MovableEntity Target;


	
	/// <summary>
	/// Explosionsradius der Rakete
	/// </summary>
	private static readonly float f_explosionsRadius = 8f;



	/// <summary>
	/// Radius in dem der volle Schaden verursacht wird.
	/// </summary>
	private static readonly float f_explosionsImpactRadius = 3f;



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
	
	

	public void Init(MovableEntity target){
		Target = target;
		f_explosionsDamage = Owner.Damage;
		d_timeToLife = Owner.Range * 2f;
		
		//Zielposition setzen
		TargetPos = Target.Pos;
		
		base.Init();
		
		//Geschwindigkeit setzen
		MaxSpeed = 25.0f;
		MaxForce = 25.0f;
		
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
		foreach(Collider c in cs) if(c.gameObject != null){
			//Entfernung des Objektes zum Explosionszentrum berechnen
			float impact = f_explosionsImpactRadius * f_explosionsImpactRadius;
			float range = (DistanceSqTo(c.bounds.center) - impact);
			//Im Explosionsmittelpunkt den vollen schaden
			if(range <= 0f) range = 0f;
			//ansonsten skalieren
			else range /= (f_explosionsRadius * f_explosionsRadius - impact);

			//Schaden proportional zur Entfernung berechnen
			int dmg = (int)( f_explosionsDamage * (1.0f - range) );
				
			//Schadensmeldung verschicken
			if(dmg > 0) Owner.DoDamage(c, dmg);
		}

		//normal sterben
		base.Death();
	}

	public override void DeathEffect(){
		//Partikeleffekt
		//Sound hängt am Effekt mit PlayOnAwake muss also nicht im Script gestartet werden.
		GameObject explosion = Instantiate("SmallExplosion", Pos);
		explosion.transform.LookAt(new Vector3(0,0,0));

		//nach 1.0 Sekunden Explosion wieder zerstören
		Destroy(explosion, 3f);
	}
	
	
}
