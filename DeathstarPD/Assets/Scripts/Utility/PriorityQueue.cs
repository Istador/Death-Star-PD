using System;
using System.Collections;
using System.Collections.Generic;

/*
 * Priorisierte Warteschlange ohne doppelte Elemente
 * 
 * basiert auf den Vergleichsoperator CompareTo des Interfaces IComparable
*/
public class PriorityQueue<T> : ICollection<T> where T: IComparable<T> {
	
	
	
	/// <summary>
	/// Heap, in dem das kleinste Element vorne ist
	/// </summary>
	private Heap<T> heap;
	
	
	
	/// <summary>
	/// Menge, um doppelte Elemente auszuschließen
	/// </summary>
	private HashSet<T> menge;
	
	
	
	/// <summary>
	/// Konstruktor: Leere Warteschlange
	/// </summary>
	public PriorityQueue(){
		heap = new MinHeap<T>();
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
		Clear();
	}

	/// <summary>
	/// Leert die Proiority Queue, so dass sie keine Elemente mehr enthält.
	/// </summary>
	public void Clear(){
		heap.Clear();
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
			heap.Add(t);
			menge.Add(t);
		}
		
	}
	
	
	
	/// <summary>
	/// Das erste Element der Warteschlange.
	/// Das Element mit der kleinsten Priorität.
	/// </summary>
	public T First{get{
		if(!heap.IsEmpty)
			return heap.First;
		return default(T);
	}}
	
	
	
	/// <summary>
	/// Entferne das erste Element der Warteschlange
	/// </summary>
	public bool RemoveFirst(){
		if(!heap.IsEmpty){
			menge.Remove(First);
			heap.RemoveFirst();
			return true;
		}
		return false;
	}
	
	
	
	/// <summary>
	/// Entferne das erste Element der Warteschlange und gebe es zurück.
	/// </summary>
	public T Dequeue(){
		T t = First;
		RemoveFirst();
		return t;
	}


	// IEnumerator
	public IEnumerator<T> GetEnumerator(){ return heap.GetEnumerator(); }
	IEnumerator IEnumerable.GetEnumerator(){ return heap.GetEnumerator(); }

	// ICollection
	public int Count { get{return heap.Count;} }
	public bool IsReadOnly { get{return false;} }
	public void Add(T t){ Enqueue(t); }
	public bool Contains(T t){ return menge.Contains(t); }
	public bool Remove(T t){
		if(t.Equals(First)) return RemoveFirst();
		else throw new InvalidOperationException("Remove not possible");
	}
	public void CopyTo(T[] arr, int index){ heap.CopyTo(arr, index); }
	
}
