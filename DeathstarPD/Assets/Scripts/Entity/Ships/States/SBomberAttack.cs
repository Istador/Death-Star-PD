using UnityEngine;

public class SBomberAttack : State<MovableEntity> {

	private static bool Alive(Entity e){
		return e != null && !e.IsDead && e.enabled;
	}

	private static void Attack(Bomber s){
		//Greife das Ziel jetzt an
		if(s.SubTarget != null)
			s.DoAttack(s.SubTarget);
		else
			s.DoAttack(s.Target);

		//Nachricht an selbst für später
		MessageDispatcher.I.Dispatch(s, "AttackCooldownReached", s.AttackCooldown);
	}



	public override void Enter(MovableEntity owner){
		Bomber s = (Bomber) owner;

		// wenn der Cooldown ready ist
		if(!s.isAttackCooldownActive){
			//Angreifen
			Attack(s);
			//merke das der Cooldown läuft
			s.isAttackCooldownActive = true;
		}
	}



	public override void Execute(MovableEntity owner){
		Bomber s = (Bomber) owner;

		//Sub Target am Leben
		if( Alive(s.SubTarget) ){
			//es ist Zeit anzugreifen
			if(!s.isAttackCooldownActive){
				//Sub Target zu weit weg
				if(s.DistanceSqTo(s.SubTarget) > s.Range * s.Range){
					owner.MoveFSM.ChangeState(SBomberSteer.I);
					return;
				}
				//Ziel ist in Reichweite
				Attack(s);
			}
		}
		//Ziel am Leben
		else if( Alive(s.Target) ){
			//es ist Zeit anzugreifen
			if(!s.isAttackCooldownActive){
				//Ziel zu weit weg
				if( s.DistanceSqTo(s.Target) > s.Range * s.Range ){
					owner.MoveFSM.ChangeState(SBomberSteer.I);
					return;
				}
				//Ziel ist in Reichweite
				Attack(s);
			}
		}
		//Weder Target, noch Sub-Target sind am leben, suche neues Ziel
		else {
			owner.MoveFSM.ChangeState(SBomberSteer.I);
			return;
		}


	}



	public override bool OnMessage(MovableEntity owner, Telegram msg){
		switch(msg.message){
		case "AttackCooldownReached":
			Bomber s = (Bomber) owner;
			//Sub-Ziel existiert und ist in Reichweite 
			if( Alive(s.SubTarget) ){
				if(s.DistanceSqTo(s.SubTarget) <= s.Range * s.Range){
					Attack(s);
					return true;
				}
			}
			//Ziel existiert noch und ist in Reichweite 
			else if( Alive(s.Target) && s.DistanceSqTo(s.Target) <= s.Range * s.Range){
				Attack(s);
				return true;
			}

			//Ziel existiert nicht mehr oder ist nicht mehr in Reichweite
			s.isAttackCooldownActive = false;
			//Seek näher bzw. ein neues Ziel
			s.MoveFSM.ChangeState(SBomberSteer.I);
			return true;
		default:
			return false;
		}
	}


	
	/**
	 * Singleton
	*/
	private static SBomberAttack instance;
	private SBomberAttack(){}
	public static SBomberAttack Instance{get{
			if(instance==null) instance = new SBomberAttack();
			return instance;
		}}
	public static SBomberAttack I{get{return Instance;}}
}
