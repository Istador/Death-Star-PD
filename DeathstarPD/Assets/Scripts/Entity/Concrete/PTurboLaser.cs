using UnityEngine;
using System.Collections;

public class PTurboLaser : MonoBehaviour {
	
	public float speed = 100;
	public Entity target { get; set; }

	private float d_startTime;
	private float d_timeToLife = 1000;

	// Use this for initialization
	void Start () {
		transform.LookAt(target.Pos);
		d_startTime = Time.time;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if(target){
			transform.Translate(Vector3.forward * Time.deltaTime * speed);
			if(d_startTime + d_timeToLife <= Time.time || Vector3.Distance(transform.position, target.Pos) < 3){
				Destroy(gameObject);
			}
		}else{
			Destroy(gameObject);
		}
	}
}
