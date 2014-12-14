using UnityEngine;

public class SBomberLanding : State<MovableEntity> {



	public override void Execute(MovableEntity owner){
		// wenn wir den Planeten erreicht haben
		if(owner.Pos.magnitude - Ship.orbit_range - Ship.orbit_interval < 0){
			//Suche Ziel
			owner.MoveFSM.ChangeState(SBomberSteer.I);
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



	/**
	 * Singleton
	*/
	private static SBomberLanding instance;
	private SBomberLanding(){}
	public static SBomberLanding Instance{get{
			if(instance==null) instance = new SBomberLanding();
			return instance;
		}}
	public static SBomberLanding I{get{return Instance;}}
}
