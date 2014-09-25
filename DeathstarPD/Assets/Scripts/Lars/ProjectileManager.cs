using UnityEngine;

public class ProjectileManager : ScriptableObject{
	private GameObject[] projectiles;
	private int size;
	private int count = 0;

	public ProjectileManager(int size){
		projectiles = new GameObject[size];
		this.size = size;
	}

	public void Add(GameObject obj){
		if(count >= size){
			Destroy(projectiles[count%size]);
			projectiles[count%size] = obj;
		}else{
			projectiles[count] = obj;
		}
		count += 1;
	}

	public void Clear(){
		foreach(GameObject o in projectiles){
			Destroy (o);
		}
		projectiles = new GameObject[size];
	}
}
