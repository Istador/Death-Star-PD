using UnityEngine;

public class SLanding : State<MovableEntity> {



	public override void Execute(MovableEntity owner){
		// wenn wir den Planeten erreicht haben
		if(owner.Pos.magnitude - Ship.orbit_range - Ship.orbit_interval < 0){
			//Suche Ziel
			owner.MoveFSM.ChangeState(SSteer.I);
		}
	}


	
	public override bool OnMessage(MovableEntity owner, Telegram msg){
		switch(msg.message){
		case "AttackCooldownReached":
			((Ship)owner).isAttackCooldownActive = false;
			return true;
		default:
			return false;
		}
	}



	/**
	 * Singleton
	*/
	private static SLanding instance;
	private SLanding(){}
	public static SLanding Instance{get{
			if(instance==null) instance = new SLanding();
			return instance;
		}}
	public static SLanding I{get{return Instance;}}
}
