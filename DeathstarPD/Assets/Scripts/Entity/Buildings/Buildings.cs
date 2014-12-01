using UnityEngine;
using System;
using System.Collections.Generic;

public class Buildings {


	
	/// <summary>
	/// Container in den alle Gebäude kommen
	/// </summary>
	public static GameObject Container {
		get{
			if(_container == null)
				_container = GameObject.Find("Buildings");
			return _container;
		}
	}
	private static GameObject _container = null;
	
	
	//Alle Gebäude
	private HashSet<Building> set = new HashSet<Building>();
	
	
	//Alle Gebäude, die einer bestimmten Klasse angehören
	private Dictionary<Type, HashSet<Building>> map = new Dictionary<Type, HashSet<Building>>();
	
	
	
	/// <summary>
	/// Alle lebendigen Gebäude, iterierbar.
	/// </summary>
	public IEnumerable<Building> All {
		get{return set;}
	}
	
	
	
	/// <summary>
	/// Alle lebendigen Gebäude einer bestimmten Klasse, iterierbar.
	/// </summary>
	/// <typeparam name="B">Die Klasse der gewünschten Gebäude.</typeparam>
	public IEnumerable<Building> ByClass<B>() where B: Building { 
		Type type = typeof(B);

		//Klasse ist vorhanden
		if(map.ContainsKey(type)) return map[type];
		//Klasse ist nicht vorhanden
		else return System.Linq.Enumerable.Empty<Building>();
	}
	
	
	
	/// <summary>
	/// Fügt ein neues Gebäude der Collection hinzu.
	/// </summary>
	/// <param name="t">das neue Gebäude.</param>
	public void Add(Building b) {
		if(!set.Contains(b)){
			//zu allen hinzufügen
			set.Add(b);
			
			//zum Klassen-Set hinzufügen
			Type type = b.GetType();
			if(!map.ContainsKey(type))
				map.Add(type, new HashSet<Building>());
			map[type].Add(b);
		}
	}
	
	
	
	/// <summary>
	/// Entfernt ein Gebäude.
	/// </summary>
	/// <param name="t">das zu entfernende Gebäude.</param>
	public void Remove(Building b){
		if(set.Contains(b)){
			//entferne von allen
			set.Remove(b);

			//entferne vom Klassen-Set
			Type type = b.GetType();
			if(map.ContainsKey(type))
				map[type].Remove(b);
		}
	}



	/// <summary>
	/// Gebäude, welches am dichtesten an der geforderten Position liegt
	/// </summary>
	/// <returns>Building or null</returns>
	/// <param name="pos">Position in 3D-Koordinatensystem.</param>
	public Building NearestTo(Vector3 pos){
		Building nearest = null;
		float ndist = float.PositiveInfinity;

		//Iteriere über alle Gebäude, und finde das mit der kleinsten Distanz
		foreach(Building b in All){
			float dist = b.DistanceSqTo(pos);
			if(nearest == null || dist < ndist){
				nearest = b;
				ndist = dist;
			}
		}

		return nearest;
	}
	


	/**
	 * Singleton
	*/
	private static Buildings instance;
	private Buildings(){}
	public static Buildings Instance{get{
			if(instance==null) instance = new Buildings();
			return instance;
		}}
	public static Buildings I{get{return Instance;}}
}
