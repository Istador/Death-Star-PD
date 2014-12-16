using UnityEngine;
using System.Collections;

public class PBomb : MonoBehaviour {

	public float speed = 100;
	public Entity target;

	// Use this for initialization
	void Start () {
		transform.LookAt(target.Pos);
	}
	
	// Update is called once per frame
	void Update () {
		if(target){
			if(Vector3.Distance(transform.position, target.Pos) <= 1){
				ParticleSystem ps = gameObject.GetComponent<ParticleSystem>();
				ps.startSpeed = 15;
				ps.startLifetime = 2;
				ps.loop = false;
				gameObject.GetComponent<AudioSource>().Play();
			}else{
				transform.Translate(Vector3.forward * Time.deltaTime * speed);
			}
		}
	}
}
