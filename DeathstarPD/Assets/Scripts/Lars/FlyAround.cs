using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FlyAround : MovableEntity {

	private float rotation = 0;
	public float radius = 102f;
	public float topRotation = 0f;
	public float speed = -0.01f;
	/* private GameObject[] pew; */ //unused
	private int timer = 0;

	private ProjectileManager pewManager;

	public GameObject projectile;

	// Use this for initialization
	protected override void Start() {
		/* pew = new GameObject[10]; */ //unused
		Vector3 newPos = new Vector3 (Mathf.Cos (rotation) * radius, 0f, Mathf.Sin (rotation) * radius);
		transform.position = newPos;
		pewManager = new ProjectileManager(10);
	}
	
	// Update is called once per frame
	protected override void Update() {
		rotation += speed;
		float nradius = Mathf.Cos (topRotation) * radius;
		Vector3 newPos = new Vector3 (Mathf.Cos (rotation) * nradius, Mathf.Sin(topRotation)*radius, Mathf.Sin (rotation) * nradius);
		transform.position = newPos;
		transform.LookAt(new Vector3(0,0,0));
		timer += 1;
		if (timer >= 30) {
			timer = 0;
			shoot();
		}
	}

	protected override void FixedUpdate(){}

	void shoot(){
		//Projektil erzeugen
		GameObject p = (GameObject)GameObject.Instantiate (projectile);
		p.transform.parent = ProjectileManager.Container.transform; //in Projektil-Container
		p.transform.position = transform.position;
		p.transform.rotation = transform.rotation;
		pewManager.Add (p);

	}

	public override void ApplyDamage(Vector3 damage){
		int dmg = System.Convert.ToInt32(damage.magnitude);
		Debug.Log("Avion wurde getroffen mit "+dmg+" Schaden.");
	}
}
