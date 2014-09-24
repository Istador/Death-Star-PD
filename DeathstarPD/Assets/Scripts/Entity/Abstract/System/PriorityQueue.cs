using System;
using System.Collections;
using System.Collections.Generic;

/*
 * Priorisierte Warteschlange ohne doppelte Elemente
 * 
 * basiert auf den Vergleichsoperator CompareTo des Interfaces IComparable
*/
public class PriorityQueue<T> where T: IComparable<T> {
	
	
	
	/// <summary>
	/// Sortierte Liste anhand IComparable
	/// </summary>
	private SortedList<T, T> list; //TODO: Austauschen gegen einen Heap für bessere Laufzeiten
	
	
	
	/// <summary>
	/// Menge, um doppelte Elemente auszuschließen
	/// </summary>
	private HashSet<T> menge;
	
	
	
	/// <summary>
	/// Konstruktor: Leere Warteschlange
	/// </summary>
	public PriorityQueue(){
		list = new SortedList<T,T>();
		menge = new HashSet<T>();
	}
	
	
	
	/// <summary>
	/// Konstruktor: Füllen mit mehreren Elementen (VarArgs)
	/// </summary>
	/// <param name='args'>
	/// Elemente welche in die Warteschlange aufgenommen werden sollen
	/// </param>
	public PriorityQueue(params T[] args) : this() {
		foreach(T a in args)
			Enqueue(a);
	}
	
	
	
	/// <summary>
	/// Leert die Proiority Queue, so dass sie keine Elemente mehr enthält.
	/// </summary>
	public void Empty(){
		list.Clear();
		menge.Clear();
	}
	
	
	/// <summary>
	/// Einfügen eines Elementes in Warteschleife, wenn nicht bereits vorhanden
	/// </summary>
	/// <param name='t'>
	/// Das Element das in die Warteschlange soll
	/// </param>
	public void Enqueue(T t){
		//noch nicht vorhanden
		if(!menge.Contains(t)){
			list.Add(t, t);
			menge.Add(t);
		}
		
	}
	
	
	
	/// <summary>
	/// Das erste Element der Warteschlange.
	/// Das Element mit der kleinsten Priorität.
	/// </summary>
	public T First(){
		if(list.Count > 0)
			return list.Values[0];
		return default(T);
	}
	
	
	
	/// <summary>
	/// Entferne das erste Element der Warteschlange
	/// </summary>
	public void RemoveFirst(){
		if(list.Count > 0){
			menge.Remove(First());
			list.RemoveAt(0);
		}
	}
	
	
	
	/// <summary>
	/// Entferne das erste Element der Warteschlange und gebe es zurück.
	/// </summary>
	public T Dequeue(){
		T t = First();
		RemoveFirst();
		return t;
	}
	
	
	
}
