using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

//Klasse in der die Beobachter gespeichert werden
public class Observer {

	//die beobachter
	private Dictionary<string, HashSet<MessageReceiver>> map = new Dictionary<string, HashSet<MessageReceiver>>();

	//letzter gemerkter Wert
	private Dictionary<string, object> cache = new Dictionary<string, object>();

	//Registriert einen, von potentiell vielen, Beobachtern (obj) für ein bestimmtes Ereignis(msg)
	public object Add(string message, MessageReceiver obj){

		//set bekommen
		HashSet<MessageReceiver> set;
		map.TryGetValue(message, out set);
		if(set == null){
			set = new HashSet<MessageReceiver>();
			map[message] = set;
		}

		//in set einfügen
		set.Add(obj);

		//sofern vorhanden den letzt bekannten Wert zurückgeben
		if(cache.ContainsKey(message))
			return cache[message];
		return null;
	}

	//Trägt einen Beobachter wieder aus
	public void Remove(string message, MessageReceiver obj){
		if(map.ContainsKey(message))
			map[message].Remove(obj); //wirft keine Exception, sondern false, wenn nicht vorhanden
	}

	//sendet an allen Beobachtern des Events (msg) die msg und das Objekt (z.B. einen aktualisierten Wert)
	public void Update(object sender, string message, object extraInfo){
		//Wert merken
		if(extraInfo != null)
			cache[message] = extraInfo;

		UpdateWithoutCache(sender, message, extraInfo);
	}

	//sendet an allen Beobachtern des Events (msg) die msg und das Objekt (z.B. einen aktualisierten Wert)
	public void UpdateWithoutCache(object sender, string message, object extraInfo){
		//Wert mitteilen
		if(map.ContainsKey(message)){
			//Debug.Log("update "+message+" "+map[message].Count);
			foreach(MessageReceiver receiver in map[message])
				MessageDispatcher.I.Dispatch(sender, receiver, message, 0, extraInfo);
		}
	}


	/**
	 * Singleton
	*/
	private static Observer instance;
	private Observer(){}
	public static Observer Instance{get{
			if(instance==null) instance = new Observer();
			return instance;
		}}
	public static Observer I{get{return Instance;}}
}