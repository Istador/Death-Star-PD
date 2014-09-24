using UnityEngine;
using System.Collections;

/// 
/// Abstrakte Oberklasse für bewegliche Gegner
/// 
public abstract class MovableEntity : Entity {
	
	/// <summary>
	/// Zustandsautomat für die Bewegung.
	/// </summary>
	public readonly StateMachine<MovableEntity> MoveFSM;
	
	/// <summary>
	/// Richtung (Links/Rechts) in die der Gegner guckt.
	/// Lokales Koordinatensystem.
	/// </summary>
	public virtual Vector3 Heading {get{return Moving;}}
	
	/// <summary>
	/// Richtung (Links/Rechts/Oben/Unten) in die sich der Gegner bewegt.
	/// </summary>
	public Vector3 Moving {
		get{
			//wenn kein Moving angegeben
			if(_Moving == Vector3.zero && Mathf.Abs(rigidbody.velocity.magnitude) > 0.5f)
				//ermittel das aus der Geschwindigkeit
				return Utility.ToHeading(rigidbody.velocity);
			//sonst die Instanzvariable
			return _Moving;
		}
		set{_Moving = value;}
	}
	private Vector3 _Moving;
	
	//Hilfsmethode
	private void Move(Vector3 heading){
		Moving = heading;
		Steering.DoSeek(Pos + heading * MaxSpeed);
	}
	
	/// <summary>
	/// Bewegung nach oben
	/// </summary>
	public void MoveUp(){ Move(Vector3.up); }
	/// <summary>
	/// Bewegung nach unten
	/// </summary>
	public void MoveDown(){ Move(Vector3.down); }
	/// <summary>
	/// Bewegung nach links
	/// </summary>
	public void MoveLeft(){ Move(Vector3.left); }
	/// <summary>
	/// Bewegung nach rechts
	/// </summary>
	public void MoveRight(){ Move(Vector3.right); }
	/// <summary>
	/// Hört auf sich zu bewegen
	/// </summary>
	public void StopMoving(){
		//anhalten
		rigidbody.velocity = Vector3.zero;
		rigidbody.angularVelocity = Vector3.zero;
		Steering.Stop();
		Moving = Vector3.zero;
	}



	/// <summary>
	/// SpeedBonus, um temporär die geschwindigkeit anpassen zu können
	/// </summary>
	public float SpeedBonus { get; set;}


	
	/// <summary>
	/// maximale Geschwindigkeit
	/// </summary>
	public float MaxSpeed { 
		get{ return _maxSpeed + SpeedBonus; }
		set{ _maxSpeed = value; }
	}
	private float _maxSpeed;
	
	
	
	/// <summary>
	/// maximale Kraft der Steering Behaviors
	/// </summary>
	public float MaxForce { 
		get{ return _maxForce + SpeedBonus; }
		set{ _maxForce = value; }
	}
	private float _maxForce;
	
	
	
	/// <summary>
	/// Steering Behavior Komponente, die den "Wunsch nach Bewegung" ausdrückt
	/// </summary>
	public readonly SteeringBehaviors Steering;
	
	
	
	/// <summary>
	/// Initializes a new instance of the <see cref="MovableEnemy`1"/> class.
	/// </summary>
	/// <param name='maxHealth'>
	/// Maximale Trefferpunkte des Gegners. Bei 0 HP stirbt der Gegner.
	/// </param>
	public MovableEntity(int maxHealth) : base(maxHealth){
		//Zustandsautomaten erstellen
		MoveFSM = new StateMachine<MovableEntity>(this);
		
		//Steering Behavior Komonente erstellen
		Steering = new SteeringBehaviors(this);

		SpeedBonus = 0.0f;
		MaxSpeed = 1.0f;
		MaxForce = 1.0f;
	}
	
	
	protected override void Start() {
		base.Start();
		
		//Zustandsautomaten starten (Enter)
		MoveFSM.Start();
	}
	
	
	
	/// <summary>
	/// Filtert den Kraftvektor vor der Anwendung auf den rigidbody
	/// </summary>
	/// <returns>
	/// Die gefilterte Kraft
	/// </returns>
	/// <param name='vin'>
	/// Eingehende Kraft der Steering Behaviour Komponente
	/// </param>
	protected virtual Vector3 FilterForce(Vector3 vIn){
		return vIn;
	}
	
	
	
	// Update
	
	/// <summary>
	/// Zustandsautomaten, Animation
	/// </summary>
	protected override void Update() {
		//Update der Zustandsautomaten
		MoveFSM.Update();
		
		//Animation
		base.Update();
	}
	
	
	
	/// <summary>
	/// Steering Behaviors berechnen und anwenden
	/// </summary>
	protected virtual void FixedUpdate () {
		
		//resultierende Kraft der verschiedenen Steering Behaviors berechnen
		Vector3 f = FilterForce(Steering.Calculate());
		
		if(f != Vector3.zero){
			
			//Kraft auf die Unity-Physik-Engine übertragen, um Bewegung zu erzeugen
			//rigidbody.AddRelativeForce(f); //nicht für Projektile
			rigidbody.AddForce(f, ForceMode.Impulse);
				
			//Bewegungsgeschwindigkeit limitieren
			if(Mathf.Abs(rigidbody.velocity.magnitude) > MaxSpeed)
				rigidbody.velocity = rigidbody.velocity.normalized * MaxSpeed;
		}
	}
	
	
	
	// HandleMessage
	
	/// <summary>
	/// Nachricht empfangen, deligieren an Zustandsautomaten
	/// </summary>
	/// <returns>
	/// ob die Nachricht von einem der Automaten angenommen wurde
	/// </returns>
	/// <param name='msg'>
	/// Die Nachricht
	/// </param>
	public override bool HandleMessage(Telegram msg){
		//Move-Zustandsautomat
		return IsDead || MoveFSM.HandleMessage(msg);
	}
	
	
	
}
