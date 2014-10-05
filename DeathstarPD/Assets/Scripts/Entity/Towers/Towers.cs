using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Towers : IEnumerable<Tower> {

	//Alle Türme
	private HashSet<Tower> set = new HashSet<Tower>();


	//Alle Türme, die einer bestimmten Klasse angehören
	private Dictionary<Type, HashSet<Tower>> map = new Dictionary<Type, HashSet<Tower>>();



	/// <summary>
	/// Alle lebendigen Türme, iterierbar.
	/// </summary>
	public IEnumerable<Tower> All {
		get{return set;}
	}



	/// <summary>
	/// Alle lebendigen Türme einer bestimmten Klasse, iterierbar.
	/// </summary>
	/// <typeparam name="T">Die Klasse der gewünschten Türme.</typeparam>
	public IEnumerable<Tower> ByClass<T>() where T: Tower { 
		Type type = typeof(T);

		//Klasse ist vorhanden
		if(map.ContainsKey(type)) return map[type];
		//Klasse ist nicht vorhanden
		else return System.Linq.Enumerable.Empty<Tower>();
	}



	/// <summary>
	/// Fügt einen neuen Turm der Collection hinzu.
	/// </summary>
	/// <param name="t">der neue Turm.</param>
	public void Add(Tower t) {
		if(!set.Contains(t)){
			//zu allen hinzufügen
			set.Add(t);

			//zum Klassen-Set hinzufügen
			Type type = t.GetType();
			if(!map.ContainsKey(type))
				map.Add(type, new HashSet<Tower>());
			map[type].Add(t);
		}
	}



	/// <summary>
	/// Entfernt einen Turm.
	/// </summary>
	/// <param name="t">der zu entfernende Turm.</param>
	public void Remove(Tower t){
		if(set.Contains(t)){
			//entferne von allen
			set.Remove(t);

			//entferne vom Klassen-Set
			Type type = t.GetType();
			if(map.ContainsKey(type))
				map[type].Remove(t);
		}
	}



	//IEnumerable
	public IEnumerator<Tower> GetEnumerator(){ return set.GetEnumerator(); }
	IEnumerator IEnumerable.GetEnumerator(){ return set.GetEnumerator(); }



	/**
	 * Singleton
	*/
	private static Towers instance;
	private Towers(){}
	public static Towers Instance{get{
			if(instance==null) instance = new Towers();
			return instance;
		}}
	public static Towers I{get{return Instance;}}
}
