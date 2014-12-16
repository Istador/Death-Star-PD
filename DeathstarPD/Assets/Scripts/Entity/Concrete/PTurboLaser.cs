using UnityEngine;
using System.Collections;

public class PTurboLaser : MonoBehaviour {

	public float speed = 100;
	public Entity target { get; set; }

	private float d_startTime;
	private float d_timeToLife = 1000;

	void Start () {
		transform.LookAt(target.Pos);
		d_startTime = Time.time;
	}

	void FixedUpdate () {
		if(target){
			//TODO Lars: never ever use Time.deltaTime in FixedUpdate() !!!
			// FixedUpdate() : Time.fixedDeltaTime
			// Update() : Time.deltaTime
			transform.Translate(Vector3.forward * Time.deltaTime * speed);

			if(d_startTime + d_timeToLife <= Time.time || Vector3.Distance(transform.position, target.Pos) < 3){
				Destroy(gameObject);
			}
		}else{
			Destroy(gameObject);
		}
	}
}
