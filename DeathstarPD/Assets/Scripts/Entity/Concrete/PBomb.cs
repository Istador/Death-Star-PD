using UnityEngine;
using System.Collections;

public class PBomb : MonoBehaviour {

	public float speed = 10;
	public Entity target;
	public ParticleSystem ps;

	private float lifetime = 0;
	private bool count = false;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		if(count == true){
			lifetime += Time.deltaTime;
		}
		if(lifetime >= 3f){
			Destroy(gameObject);
		}
		if(target && !count){
			if(Vector3.Distance(transform.position, target.Pos) <= 1){
				ps.startSpeed = 5;
				ps.startLifetime = 2;
				ps.loop = false;
				gameObject.GetComponent<AudioSource>().Play();
				count = true;
			}else{
				transform.Translate(Vector3.forward * Time.deltaTime * speed);
			}
		}
	}

	public void setTarget(Entity t){
		target = t;
		transform.LookAt(target.Pos);
	}
}
