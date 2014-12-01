using UnityEngine;

public class ProjectileManager : ScriptableObject {
	private GameObject[] projectiles;
	private int size {get {return projectiles.Length;}}
	private int count = 0;

	public void Init(int size){
		projectiles = new GameObject[size];
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



	/// <summary>
	/// Container in den alle Projektile kommen
	/// </summary>
	public static Transform Container { get{
			if(_container == null)
				_container = GameObject.Find("Projectiles").transform;
			return _container;
		}
	}
	private static Transform _container = null;


}
