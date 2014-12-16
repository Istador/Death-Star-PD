using UnityEngine;
using System.Collections;

public class PLightning : MonoBehaviour {

	public float speed = 10;
	public float wobble = 0.5f;
	public float lifetime = 1;
	public float trailLifetime = 0.25f;

	public MovableEntity target;

	private float t = 0;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		//Update kann auch bei pausiertem Spiel aufgerufen werden.
		if(Pause.I.Paused || Time.timeScale == 0f) return;


		t += Time.deltaTime;
		if(t > trailLifetime){
			gameObject.GetComponent<TrailRenderer>().enabled = false;
		}
		if(t > lifetime){
			Destroy(gameObject);
		}
		if(target){
			Vector3 newPos = Vector3.Lerp(transform.position, target.transform.position, speed);
			float r = Random.Range(-wobble, wobble);
			newPos.x += r;
			r = Random.Range(-wobble, wobble);
			newPos.y += r;
			r = Random.Range(-wobble, wobble);
			newPos.z += r;
			transform.position = newPos;
		}
	}
}
