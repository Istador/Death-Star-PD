using UnityEngine;

public class SAttack : State<MovableEntity> {



	public override void Enter(MovableEntity owner){
		Debug.Log("Attack Enter");
		owner.StopMoving();
	}



	public override void Execute(MovableEntity owner){
		Ship s = (Ship) owner;

		//Ziel ist tot, suche neues Ziel
		if(s.Target == null || s.Target.IsDead || !s.Target.enabled){
			owner.MoveFSM.ChangeState(SSteer.I);
			return;
		}

		/*
		//Ziel lebt noch, Distanz ist zu groß
		if( s.DistanceSqTo(s.Target) > s.Range * s.Range){
			// fliege zum Ziel
			owner.MoveFSM.ChangeState(SSteer.I);
			return;
		}
		*/
	}



	public override void Exit(MovableEntity owner){}



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
