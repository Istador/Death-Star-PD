using UnityEngine;
using System.Collections;

public class Fighter : Ship {

	protected override void Start(){
		MaxHealth = 100;
		
		MaxSpeed = 20f;
		MaxForce = 15f;
		
		//Steering.DoSeek(Vector3.zero);

		base.Start();
	}

	public override int Damage { get{ return 5; } }

	public override float Range { get{ return 20f; } }

	public override float AttackCooldown { get{ return 1f; } }

}
