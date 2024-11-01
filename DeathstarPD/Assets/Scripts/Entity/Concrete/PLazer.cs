﻿using UnityEngine;
using System.Collections;

public class PLazer : MonoBehaviour {

	public LineRenderer lr;
	public GameObject sparks;
	public float preload = 0.5f;
	public float duration = 0.5f;
	public Entity target { get; set; }

	private float t = 0.0f;

	// Use this for initialization
	void Start () {
		LineRenderer lr = gameObject.GetComponent<LineRenderer>();
		lr.SetPosition(0, transform.position);
		duration += preload;
	}
	
	// Update is called once per frame
	void Update () {
		t += Time.deltaTime;
		if(t >= duration){
			Destroy(gameObject);
		}
		if(t >= preload){
			if(target != null){
				lr.SetPosition(1, target.Pos);
				sparks.transform.position = target.Pos;
				sparks.SetActive(true);
				if(audio != null && audio.enabled)
					audio.Play();
			}
		}
	}
}
