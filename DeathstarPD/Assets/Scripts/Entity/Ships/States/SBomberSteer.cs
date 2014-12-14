using UnityEngine;

public class SBomberSteer : State<MovableEntity> {
	
	/// <summary>
	/// Die aktuelle Framenummer
	/// </summary>
	private int frame_nr = 0;
	/// <summary>
	/// Wann der letzte Frame war. (um zu erkennen wann ein neuer Frame ist)
	/// </summary>
	private float last_frame_time = 0f;


	private static Tower FindNearestTower(Bomber s){
		Tower t = null;
		float tdistSq = float.PositiveInfinity;

		//Kollisionstest
		Collider[] cs = Physics.OverlapSphere(s.Pos, Bomber.f_towerRange, (int)GeneralObject.Layer.Tower);

		//finde den Turm mit der kleinsten Distanz zum Bomber
		foreach(Collider c in cs){
			Tower n = c.gameObject.GetComponent<Tower>();
			if(n != null && !n.IsDead && n.enabled){
				float distSq = s.DistanceSqTo(c);
				if(distSq < tdistSq){
					t = n;
					tdistSq = distSq;
				}
			}
		}

		return t;
	}



	public override void Enter(MovableEntity owner){
		Bomber s = (Bomber) owner;

		//falls noch kein Ziel, oder Ziel tot, suche neues Ziel
		if(s.Target == null || s.Target.IsDead || !s.Target.enabled){
			//Find das nächste Gebäude
			s.Target = Buildings.I.NearestTo(owner.Pos);
		}

		//Falls wir noch kein Sub-Ziel haben (Turm)
		if(s.SubTarget == null || s.SubTarget.IsDead || !s.SubTarget.enabled){
			//Find den nahesten Turm in Reichweite
			s.SubTarget = FindNearestTower(s);
		}

		//Target Selection
		if(s.SubTarget != null){
			//Fliege zu dem Turm
			s.Steering.DoAvoid(s.SubTarget.Pos, s.Range);
			s.FollowingSubTarget = true;
		} else if(s.Target != null){
			//Fliege zum Gebäude
			s.Steering.DoAvoid(s.Target.Pos, s.Range);
			s.FollowingSubTarget = false;
		} else {
			//Kein Ziel? o.O
			s.Steering.Stop();
		}

		s.Steering.f_SeekFactor = 2f;
	}



	public override void Execute(MovableEntity owner){
		Bomber s = (Bomber) owner;

		//Falls wir noch kein Sub-Target haben (Turm)
		if(s.SubTarget == null || s.SubTarget.IsDead || !s.SubTarget.enabled){

			//wenn der Sub-Target dem wir folgten gestorben ist
			if(s.FollowingSubTarget){
				//neues Ziel finden
				owner.MoveFSM.ChangeState(SBomberSteer.I);
				return;
			}

			//berechnen der neuen Framenummer
			if(last_frame_time < Time.time){
				frame_nr = (frame_nr + 1) % Bomber.update_each_x_frames;
				last_frame_time = Time.time;
			}
			
			//nur alle x Frames ausführen
			if(frame_nr == s.update_on_frame){
				//Find den nahesten Turm in Reichweite
				s.SubTarget = FindNearestTower(s);

				// Falls wir einen Turm gefunden haben, strebe den an
				if(s.SubTarget != null){
					owner.MoveFSM.ChangeState(SBomberSteer.I);
					return;
				}
			}
		}
		// Wir haben ein Sub-Target.
		else {
			// Prüfe ob das Sub-Target in Reichweite ist
			if( s.DistanceSqTo(s.SubTarget) <= s.Range * s.Range){
				// Greife Ziel an
				owner.MoveFSM.ChangeState(SBomberAttack.I);
				return;
			}
		}

		//Ziel ist tot, suche neues Ziel
		if(s.Target == null || s.Target.IsDead || !s.Target.enabled){
			owner.MoveFSM.ChangeState(SBomberSteer.I);
			return;
		}

		//Ziel lebt noch, überprüfe ob in Angriffsreichweite
		if( s.DistanceSqTo(s.Target) <= s.Range * s.Range){
			// Greife Ziel an
			owner.MoveFSM.ChangeState(SBomberAttack.I);
			return;
		}
	}



	public override bool OnMessage(MovableEntity owner, Telegram msg){
		switch(msg.message){
		case "AttackCooldownReached":
			((Bomber)owner).isAttackCooldownActive = false;
			return true;
		default:
			return false;
		}
	}



	public override void Exit(MovableEntity owner){
		((Bomber)owner).Steering.f_SeekFactor = 1f;
	}



	/**
	 * Singleton
	*/
	private static SBomberSteer instance;
	private SBomberSteer(){}
	public static SBomberSteer Instance{get{
			if(instance==null) instance = new SBomberSteer();
			return instance;
		}}
	public static SBomberSteer I{get{return Instance;}}
}
