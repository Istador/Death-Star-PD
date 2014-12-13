using UnityEngine;

/*
 * Steering Behaviors System
 * 
 * Ein zentrales Objekt um als Gegner aus einer Vielzahl von unterschiedlichen
 * Steering Behaviors wählen zu können, und bei mehreren ausgewählten diese
 * gemeinsam zu einem resultierenden Kraft-Vektor zu berechnen.
 * 
 * Quelle:
 * Mat Buckland - Programming Game AI by Example
*/
public class SteeringBehaviors {
	
	
	
	/// <summary>
	/// Besitzer dieser Instanz
	/// </summary>
	private MovableEntity owner;
	
	
	
	/// <summary>
	/// Ziel das für einige Behaviors benötigt wird
	/// </summary>
	public MovableEntity Target {get; set;}
	
	
	
	/// <summary>
	/// Offset vom Ziel das fOffset Pursuit benötigt wird
	/// </summary>
	public Vector3 Offset {get; set;}



	/// <summary>
	/// Entfernung zum Ziel die nicht unterschritten werden soll
	/// </summary>
	/// <value>The avoid distance.</value>
	public float AvoidDistance {get; set;}
	
	
	
	/// <summary>
	/// Zielkoordinaten
	/// </summary>
	public Vector3 TargetPos {
		get{
			if(_TargetPos == Vector3.zero && Target != null)
				return Target.Pos;
			return _TargetPos;
		}
		set{_TargetPos = value;}
	}
	private Vector3 _TargetPos = Vector3.zero; //Instanzvariable
	
	
	
	/// <summary>
	/// Initializes a new instance of the <see cref="SteeringBehaviors`1"/> class.
	/// </summary>
	/// <param name='owner'>
	/// Besitzer dieser Instanz
	/// </param>
	public SteeringBehaviors(MovableEntity owner){
		this.owner = owner;
	}
	
	
	
	/// <summary>
	/// Interne Methode.
	/// Anstreben der Zielkoordinaten mit maximaler Geschwindigkeit
	/// </summary>
	/// <param name='targetPos'>
	/// Zielkoordinaten
	/// </param>
	private Vector3 Seek(Vector3 targetPos){
		Vector3 desiredVelocity = 
			(targetPos - owner.Pos).normalized * owner.MaxForce;
		return desiredVelocity - owner.rigidbody.velocity;
	}
	
	/// <summary>
	/// Anstreben der Zielkoordinaten mit maximaler Geschwindigkeit
	/// </summary>
	/// <param name='targetPos'>
	/// Zielkoordinaten
	/// </param>
	public void DoSeek(Vector3 targetPos){
		Stop();
		Seeking = true;
		TargetPos = targetPos;
	}
	
	/// <summary>
	/// Anstreben der Zielkoordinaten des Objektes mit maximaler Geschwindigkeit
	/// </summary>
	/// <param name='targetPos'>
	/// Zielkoordinaten
	/// </param>
	public void DoSeek(GeneralObject target){
		DoSeek(target.Pos);
	}
	
	
	
	/// <summary>
	/// Interne Methode.
	/// Fliehen vor den Zielkoordinaten mit maximaler Geschwindigkeit
	/// </summary>
	/// <param name='targetPos'>
	/// Zielkoordinaten
	/// </param>
	private Vector3 Flee(Vector3 targetPos){
		Vector3 desiredVelocity = 
			(owner.Pos - targetPos).normalized * owner.MaxForce;
		
		return desiredVelocity - owner.rigidbody.velocity;
	}
	
	/// <summary>
	/// Fliehen vor den Zielkoordinaten mit maximaler Geschwindigkeit
	/// </summary>
	/// <param name='targetPos'>
	/// Zielkoordinaten
	/// </param>
	public void DoFlee(Vector3 targetPos){
		Stop();
		Fleeing = true;
		TargetPos = targetPos;
	}
	
	/// <summary>
	/// Fliehen vor den Zielkoordinaten des Objektes mit maximaler Geschwindigkeit
	/// </summary>
	/// <param name='targetPos'>
	/// Zielkoordinaten
	/// </param>
	public void DoFlee(GeneralObject target){
		DoFlee(target.Pos);
	}
	
	
	
	/// <summary>
	/// Interne Methode.
	/// vorsichtiges Anstreben der Zielkoordinaten mit passender Geschwindigkeit
	/// </summary>
	/// <param name='targetPos'>
	/// Zielkoordinaten
	/// </param>
	private Vector3 Arrive(Vector3 targetPos){
		Vector3 ToTarget = targetPos - owner.Pos;
		
		float dist = ToTarget.magnitude;
		
		if(dist > 0){
			float speed = dist / f_ArriveDeceleration;
			
			Utility.MinMax(ref speed, 0.0f, owner.MaxForce);
			
			Vector3 DesiredVelocity = ToTarget * speed / dist;
			
			return DesiredVelocity - owner.rigidbody.velocity;
		}
		
		return Vector3.zero;
	}
	
	private static float f_ArriveDeceleration = 0.1f;


	/// <summary>
	/// Distanz zum Ziel halten.
	/// </summary>
	/// <param name="targetPos">Target position.</param>
	private Vector3 Avoid(Vector3 targetPos){
		Vector3 ToTarget = targetPos - owner.Pos;
		
		float dist = ToTarget.magnitude;

		if(dist > AvoidDistance) return Vector3.zero;

		float factor = 1f - dist / AvoidDistance;

		// vom Ziel weg
		return factor * Flee(targetPos);
	}

	public void DoAvoid(Vector3 targetPos, float distance){
		Stop();
		Seeking = true;
		Avoiding = true;
		AvoidDistance = distance;
		TargetPos = targetPos;
	}
	
	/// <summary>
	/// Abfangen eines Objektes anhand dessen vorraussichtlich zukünftigen Position.
	/// </summary>
	/// <param name='evader'>
	/// Objekt das abgefangen werden soll
	/// </param>
	private Vector3 Pursuit(MovableEntity target) {
		Vector3 toEvader = target.Pos - owner.Pos;
		
		/*
		if(
			( Vector3.Dot(toEvader, owner.transform.rotation.eulerAngles) > 0 )
			&& 
			( Vector3.Dot(owner.transform.rotation, evader.transform.rotation.eulerAngles) < -0.95f )
			){
			return Seek(evader.transform.position);
		}
		*/
						
		float LAT = toEvader.magnitude / ( owner.MaxForce + target.rigidbody.velocity.magnitude );
		return Seek(target.Pos + target.rigidbody.velocity * LAT);
	}
	
	
	
	/// <summary>
	/// Ausweichen anhand der vorraussichtlich zukünftigen Position eines Verfolgers.
	/// </summary>
	/// <param name='evader'>
	/// Der Verfolger dem man asuweichen will
	/// </param>
	private Vector3 Evade(MovableEntity target) {
		Vector3 toPersuer = target.Pos - owner.Pos;
		float LAT = toPersuer.magnitude / ( owner.MaxForce + target.rigidbody.velocity.magnitude );
		return Flee(target.Pos + target.rigidbody.velocity * LAT);
	}
	
	
	/// <summary>
	/// Entität wandert zufällig durch den Raum umher
	/// </summary>
	private Vector3 Wander(){
		
		//Step 3.5 A) Adding a small random displacement to the target.
		float f1 = Utility.NextFloat(-1.0f, 1.0f);
		float f2 = Utility.NextFloat(-1.0f, 1.0f);
		v_WanderTarget += new Vector3(f1 * f_WanderJitter, 0.0f, f2 * f_WanderJitter);
		
		//Step 3.5 B) Re-Projecting the target back onto the wander circle.
		v_WanderTarget = v_WanderTarget.normalized * f_WanderRadius;
		
		//Step 3.5 C) Projecting the target in front of the vehicle
		Vector3 head = owner.rigidbody.velocity.normalized;
		if(head == Vector3.zero) head = new Vector3(1.0f, 0.0f, 0.0f);
		Vector3 targetLocal = head * f_WanderDistance + v_WanderTarget;
		
		return Seek(targetLocal + owner.Pos);
	}
	private static float f_WanderRadius = 6.0f;
	private static float f_WanderDistance = 5.0f;
	private static float f_WanderJitter = 0.6f;
	private Vector3 v_WanderTarget = new Vector3(-f_WanderRadius, 0.0f, 0.0f);
	
	
	
	
	/// <summary>
	/// Folgen eines Targets mit Offset
	/// </summary>
	/// <param name='evader'>
	/// Objekt das abgefangen werden soll
	/// </param>
	private Vector3 OffsetPursuit(MovableEntity target, Vector3 offset) {
		Vector3 WorldOffset = target.Pos + offset;
		
		Vector3 ToOffset = WorldOffset - owner.Pos;
		
		float LAT = ToOffset.magnitude / ( owner.MaxForce + target.rigidbody.velocity.magnitude );
		return Arrive(WorldOffset + target.rigidbody.velocity * LAT);
	}



	/// <summary>
	/// Entität wandert zufällig durch den Raum umher
	/// </summary>
	private Vector3 WallAvoidance(Vector3 force){

		Vector3 f = Vector3.zero;
		Vector3 pos = owner.Pos;
		Vector3 npos = pos + force;

		RaycastHit hit;
		bool hitted = owner.Linecast(pos, npos, out hit, GeneralObject.Layer.Level);

		//hat eine Wand getroffen
		if(hitted){
			Debug.DrawLine(pos, npos, Color.green);

			//Vector3 overshoot = force.normalized * (Mathf.Abs(force.magnitude) - hit.distance);
			Debug.DrawLine(hit.point, npos, Color.red);
			f = (hit.point + hit.normal * Mathf.Abs(pos.z - hit.point.z) - pos).normalized;
			//von der Wand abgehend
			f = (force.normalized * hit.distance) + f * (force.magnitude - hit.distance);

			Debug.DrawLine(pos, pos+f, Color.blue);
		}

		return f;
	}
	
	

	/// <summary>
	/// Anstreben ein-/ausschalten
	/// </summary>
	/// <param name='on'>
	/// true=ein, false=aus
	/// </param>
	public bool Seeking {get; set;}
	
	/// <summary>
	/// Fliehen ein-/ausschalten
	/// </summary>
	/// <param name='on'>
	/// true=ein, false=aus
	/// </param>
	public bool Fleeing {get; set;}
	
	/// <summary>
	/// vorsichtiges Anstreben ein-/ausschalten
	/// </summary>
	/// <param name='on'>
	/// true=ein, false=aus
	/// </param>
	public bool Arriving {get; set;}

	/// <summary>
	/// Distanz zum Ziel halten ein-/ausschalten
	/// </summary>
	/// <param name='on'>
	/// true=ein, false=aus
	/// </param>
	public bool Avoiding {get; set;}
	
	/// <summary>
	/// Abfangen ein-/ausschalten
	/// </summary>
	/// <param name='on'>
	/// true=ein, false=aus
	/// </param>
	public bool Pursuing {get; set;}
	
	/// <summary>
	/// Ausweichen ein-/ausschalten
	/// </summary>
	/// <param name='on'>
	/// true=ein, false=aus
	/// </param>
	public bool Evading {get; set;}
	
	/// <summary>
	/// Umherwandern ein-/ausschalten
	/// </summary>
	/// <param name='on'>
	/// true=ein, false=aus
	/// </param>
	public bool Wandering {get; set;}
	
	/// <summary>
	/// Offset Pursuit ein-/ausschalten
	/// </summary>
	/// <param name='on'>
	/// true=ein, false=aus
	/// </param>
	public bool OffsetPursuing {get; set;}
	
	/// <summary>
	/// Wall Avoidance ein-/ausschalten
	/// </summary>
	/// <param name='on'>
	/// true=ein, false=aus
	/// </param>
	public bool WallAvoiding {get; set;}


	
	
	
	/// <summary>
	/// Alles anhalten
	/// </summary>
	public void Stop(){
		Seeking = false;
		Fleeing = false;
		Arriving = false;
		Avoiding = false;
		Pursuing = false;
		Evading = false;
		Wandering = false;
		OffsetPursuing = false;
		WallAvoiding = false;
	}
	
	
	
	/// <summary>
	/// berechnet die resultierende Kraft aller Steering Behaviours
	/// </summary>
	/// <returns>
	/// Kraft-Vektor zu der sich bewegt werden soll
	/// </returns>
	public Vector3 Calculate(){
		Vector3 f = Vector3.zero;

		if(Seeking) f += Seek(TargetPos) * f_SeekFactor;
		if(Fleeing) f += Flee(TargetPos) * f_FleeFactor;
		if(Arriving) f += Arrive(TargetPos) * f_ArriveFactor;
		if(Avoiding) f += Avoid(TargetPos) * f_AvoidFactor;
		if(Pursuing && Target != null) f += Pursuit(Target) * f_PursueFactor;
		if(Evading && Target != null) f += Evade(Target) * f_EvadeFactor;
		if(Wandering) f += Wander() * f_WanderFactor;
		if(OffsetPursuing && Target != null && Offset != Vector3.zero) 
			f += OffsetPursuit(Target, Offset) * f_OffPursueFactor;
		if(WallAvoiding) f += WallAvoidance(f) * f_WallAvoidFactor;

		//truncat
		if(f != Vector3.zero && Mathf.Abs(f.magnitude) > owner.MaxForce)
			f = f.normalized * owner.MaxForce;
		
		return f;
	}
	

	public float f_SeekFactor = 1f;
	public float f_FleeFactor = 1f;
	public float f_ArriveFactor = 1f;
	public float f_AvoidFactor = 1f;
	public float f_PursueFactor = 1f;
	public float f_EvadeFactor = 1f;
	public float f_WanderFactor = 1f;
	public float f_OffPursueFactor = 1f;
	public float f_WallAvoidFactor = 1f;


	
}
