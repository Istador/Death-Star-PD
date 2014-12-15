using UnityEngine;
using System.Collections;

public abstract class Ship : MovableEntity {

	public readonly static float orbit_range = 110f;
	public readonly static float orbit_interval = 5f;

	/// <summary>
	/// Das Ziel, dass verfolgt wird
	/// </summary>
	public Building Target { get; set; }

	protected override void Start(){
		if(MoveFSM.CurrentState == null)
			MoveFSM.CurrentState = SLanding.I;

		FMode = ForceMode.Force;

		Steering.f_SeekFactor = 1f;
		Steering.f_AvoidFactor = 4f;

		isAttackCooldownActive = false;

		Strength = _strength;

		base.Start();

		transform.LookAt(Vector3.zero);
		transform.Rotate(180f, 0f, 0f); // richtig drehen
	}



	protected override void Update(){
		base.Update();

		//beim Angreifen aufs Ziel ausrichten
		if(MoveFSM.IsInState(SAttack.I) && Target != null && !Target.IsDead && Target.enabled){
			transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Pos - Target.Pos, Pos), Time.deltaTime);
			return;
		}

		//ansonsten ausrichten entsprechend dem Bewegungsvektor
		if(rigidbody.velocity != Vector3.zero && rigidbody.velocity.sqrMagnitude >= 10.0f){
			//transform.LookAt(Pos + rigidbody.velocity);
			//transform.Rotate(180f, 0f, 0f); // richtig drehen

			transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(-rigidbody.velocity, Pos), Time.deltaTime);
		}
	}



	protected override void FixedUpdate(){
		//Gravity
		Vector3 f = -Pos.normalized * MaxForce;
		if(Pos.magnitude < orbit_range + orbit_interval){

			float b = -(Pos.magnitude - orbit_range - orbit_interval) / (orbit_interval);
			f = f - f * b;
		}
		rigidbody.AddForce(f, ForceMode.Force);

		//KI Bewegung
		base.FixedUpdate();

		Debug.DrawLine(Pos, Pos + rigidbody.velocity);

		//if(Target != null) Debug.Log("Dist: "+DistanceTo(Target));

	}



	/// <summary>
	/// verursachter Schaden pro Treffer
	/// </summary>
	public int Damage { get; private set; }


	public abstract int BaseDamage { get; }
	public abstract int BaseHealth { get; }

	
	public float Strength {
		get{ return _strength; }
		set{
			_strength = value;
			MaxHealth = Mathf.RoundToInt((float) BaseHealth * value);
			Damage = Mathf.RoundToInt((float) BaseDamage * value);
		}
	}
	private float _strength = 1f;
	

	/// <summary>
	/// Angriffsreichtweite
	/// </summary>
	public abstract float Range { get; }
	
	
	
	/// <summary>
	/// Abklingzeit zwischen Angriffen in Sekunden.
	/// </summary>
	public abstract float AttackCooldown { get; }
	/// <summary>
	/// Ob der Cooldown Aktiv ist
	/// </summary>
	public bool isAttackCooldownActive { get; set; }



	/// <summary>
	/// Der Angriffscooldown ist abgelaufen und das Target existiert noch
	/// </summary>
	public abstract void DoAttack(Entity target);


}
