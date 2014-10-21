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



	/// <summary>
	/// Container in den alle Projektile kommen
	/// </summary>
	public static GameObject Container { get{
			if(_container == null)
				_container = GameObject.Find("Projectiles");
			return _container;
		}
	}
	private static GameObject _container = null;


}
