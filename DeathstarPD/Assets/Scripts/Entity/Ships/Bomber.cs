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

	public override int BaseHealth { get{ return 70; } }
	public override int BaseDamage { get{ return 20; } }
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
	public bool FollowingSubTarget { get; set; }


	protected override void Start(){		
		MaxSpeed = 15f;
		MaxForce = 10f;

		// Positionen der Kanone
		bpos = transform.FindChild("bombpos");

		FollowingSubTarget = false;

		MoveFSM.CurrentState = SBomberLanding.I;

		base.Start();
	}

	protected override void Update(){
		//beim Angreifen aufs Ziel ausrichten
		if(MoveFSM.IsInState(SBomberAttack.I)){
			Entity e = null;

			if(SubTarget != null && !SubTarget.IsDead && SubTarget.enabled){
				e = SubTarget;
			}
			else if(Target != null && !Target.IsDead && Target.enabled){
				e = Target;
			}

			if(e != null){
				transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Pos - e.Pos, Pos), Time.deltaTime);
				return;
			}
		}

		base.Update();
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
		PBomb bomb = Instantiate("Bomb", pos).GetComponent<PBomb>();
		bomb.target = target;

		bomb.transform.parent = ProjectileManager.Container;
	}

}
