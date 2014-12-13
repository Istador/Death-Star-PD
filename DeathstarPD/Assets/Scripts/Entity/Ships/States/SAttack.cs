using UnityEngine;

public class SAttack : State<MovableEntity> {


	private static void Attack(Ship s, Building b){
		//Greife das Ziel jetzt an
		s.DoAttack(s.Target);
		//Nachricht an selbst für später
		MessageDispatcher.I.Dispatch(s, "AttackCooldownReached", s.AttackCooldown);
	}



	public override void Enter(MovableEntity owner){
		Ship s = (Ship) owner;

		// wenn der Cooldown ready ist
		if(!s.isAttackCooldownActive){
			//Angreifen
			Attack(s, s.Target);
			//merke das der Cooldown läuft
			s.isAttackCooldownActive = true;
		}
	}



	public override void Execute(MovableEntity owner){
		Ship s = (Ship) owner;

		//Ziel ist tot, suche neues Ziel
		if(s.Target == null || s.Target.IsDead || !s.Target.enabled){
			owner.MoveFSM.ChangeState(SSteer.I);
			return;
		}

		if(!s.isAttackCooldownActive && s.DistanceSqTo(s.Target) <= s.Range * s.Range){
			Attack(s, s.Target);
		}

	}



	public override bool OnMessage(MovableEntity owner, Telegram msg){
		switch(msg.message){
		case "AttackCooldownReached":
			Ship s = (Ship) owner;
			//Ziel existiert noch und ist in Reichweite 
			if(s.Target != null && s.Target.enabled && !s.Target.IsDead && s.DistanceSqTo(s.Target) <= s.Range * s.Range){
				Attack(s, s.Target);
			}
			//Ziel existiert nicht mehr oder ist nicht mehr in Reichweite
			else {
				s.isAttackCooldownActive = false;
				//Seek näher bzw. ein neues Ziel
				s.MoveFSM.ChangeState(SSteer.I);
			}
			return true;
		default:
			return false;
		}
	}


	
	/**
	 * Singleton
	*/
	private static SAttack instance;
	private SAttack(){}
	public static SAttack Instance{get{
			if(instance==null) instance = new SAttack();
			return instance;
		}}
	public static SAttack I{get{return Instance;}}
}
