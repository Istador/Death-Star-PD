using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class Resource {
	
	
	
	///
	/// Öffentliches Interface, um die Klasse selbst private zu behalten
	/// 
	/// T ist ein Object oder erbt von Object
	/// 
	public interface IRes<T> where T: Object {
		T this[string name] { get; }
	}
	
	
	
	/// 
	/// Klasse um gemeinsame Resourcen mit dem selben Type zu sammeln
	/// 
	private class Res<T> : IRes<T> where T: Object {
		//Pfad innerhalb des Resources Ordner
		private string path;
		
		//Konstruktor nur für die einschließende Klasse
		internal Res(string path = ""){
			this.path = path;
		}
		
		//Map
		private Dictionary<string, T> map = new Dictionary<string, T>();
		
		//Zugriff mittels Array-Klammern
		public T this[string name]{
			get{
				//wenn noch nicht vorhanden
				if(! map.ContainsKey(name))
					//laden
					map.Add(name, (T)Resources.Load(path + name));
				//zurückgeben
				return map[name];
			}
		}
	}
	
	
	
	/// 
	/// Resourcen Container instantiieren
	/// 
	
	public static IRes<AudioClip> Sound = new Res<AudioClip>("Sounds/");
	
	public static IRes<Object> Prefab = new Res<Object>("Prefabs/");
	
	public static IRes<Texture2D> Texture = new Res<Texture2D>("Textures/");
	
	public static IRes<Material> Materials = new Res<Material>("Materials/");
	
	//Materialien in einem Array-Format um sie schnell auszuwechseln
	private static Dictionary<string, Material[]> mats_map = new Dictionary<string, Material[]>();
	public static Material[] UsableMaterial(string name){
		if(! mats_map.ContainsKey(name))
			mats_map.Add(name, new Material[]{Materials[name]});
		return mats_map[name];
	}
	
}
