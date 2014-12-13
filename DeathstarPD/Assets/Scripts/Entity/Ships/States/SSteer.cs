using UnityEngine;

public class SSteer : State<MovableEntity> {



	public override void Enter(MovableEntity owner){
		Ship s = (Ship) owner;
		Debug.Log("Steer Enter");

		//falls kein Ziel, oder Ziel tot, suche neues Ziel
		if(s.Target == null || s.Target.IsDead || !s.Target.enabled){
			//Find das nächste Gebäude
			Building b = Buildings.I.NearestTo(owner.Pos);
			
			//Setze das Gebäude als Ziel
			s.Target = b;

			//Fliege zum Gebäude
			s.Steering.DoSeek(b.Pos);
		}

		s.Steering.Seeking = true;
	}



	public override void Execute(MovableEntity owner){
		Ship s = (Ship) owner;

		//Ziel ist tot, suche neues Ziel
		if(s.Target == null || s.Target.IsDead || !s.Target.enabled){
			owner.MoveFSM.ChangeState(SSteer.I);
			return;
		}

		//Ziel lebt noch, überprüfe ob in Angriffsreichweite
		if( s.DistanceSqTo(s.Target) <= s.Range * s.Range){
			// Greife Ziel an
			owner.MoveFSM.ChangeState(SAttack.I);
			return;
		}
	}



	public override void Exit(MovableEntity owner){
		owner.Steering.Seeking = false;
	}



	/**
	 * Singleton
	*/
	private static SSteer instance;
	private SSteer(){}
	public static SSteer Instance{get{
			if(instance==null) instance = new SSteer();
			return instance;
		}}
	public static SSteer I{get{return Instance;}}
}
