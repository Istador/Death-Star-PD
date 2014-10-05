using System;
using System.Collections;
using System.Collections.Generic;

/*
 * Quelle: http://stackoverflow.com/questions/102398/priority-queue-in-net
 * leicht verändert
*/

public abstract class Heap<T> : IEnumerable<T> where T: IComparable<T> {
	private const int InitialCapacity = 0;
	private const int GrowFactor = 2;
	private const int MinGrow = 1;
	
	private int _capacity = InitialCapacity;
	private T[] _heap = new T[InitialCapacity];
	private int _tail = 0;

	public int Count { get { return _tail; } }
	public int Capacity { get { return _capacity; } }
	public bool IsEmpty { get {return Count == 0;} }
	public T First { get {return GetMin();}}

	protected abstract bool Dominates(T x, T y);

	protected Heap(){}

	protected Heap(IEnumerable<T> collection){
		if(collection == null) throw new ArgumentNullException("collection");
		
		foreach(var item in collection){
			if(Count == Capacity) Grow();
			_heap[_tail++] = item;
		}
		
		for(int i = Parent(_tail - 1); i >= 0; i--)
			BubbleDown(i);
	}

	//Heap zurücksetzen
	public void Clear(){
		_heap = new T[_capacity];
		_tail = 0;
	}

	public void Add(T item){
		if(Count == Capacity) Grow();
		_heap[_tail++] = item;
		BubbleUp(_tail - 1);
	}

	private void BubbleUp(int i){
		if(i == 0 || Dominates(_heap[Parent(i)], _heap[i])) 
			return; //correct domination (or root)
		Swap(i, Parent(i));
		BubbleUp(Parent(i));
	}

	public T GetMin(){
		if(Count == 0) throw new InvalidOperationException("Heap is empty");
		return _heap[0];
	}

	public T RemoveFirst(){
		return ExtractMin();
	}

	public T ExtractMin(){
		if(Count == 0) throw new InvalidOperationException("Heap is empty");
		T ret = _heap[0];
		_tail--;
		Swap(_tail, 0);
		BubbleDown(0);
		return ret;
	}

	private void BubbleDown(int i){
		int dominatingNode = Dominating(i);
		Swap(i, dominatingNode);
		BubbleDown(dominatingNode);
	}

	private int Dominating(int i){
		int dominatingNode = i;
		dominatingNode = GetDominating(LeftChild(i), dominatingNode);
		dominatingNode = GetDominating(RightChild(i), dominatingNode);
		return dominatingNode;
	}

	private int GetDominating(int newNode, int dominatingNode){
		if (newNode < _tail && !Dominates(_heap[dominatingNode], _heap[newNode]))
			return newNode;
		else
			return dominatingNode;
	}

	private void Swap(int i, int j){
		if(i == j) return;
		T tmp = _heap[i];
		_heap[i] = _heap[j];
		_heap[j] = tmp;
	}
	
	private static int Parent(int i){ return (i + 1)/2 - 1; }
	private static int LeftChild(int i){ return (i + 1)*2 - 1; }
	private static int RightChild(int i){ return LeftChild(i) + 1; }

	private void Grow(){
		int newCapacity = _capacity*GrowFactor + MinGrow;
		var newHeap = new T[newCapacity];
		Array.Copy(_heap, newHeap, _capacity);
		_heap = newHeap;
		_capacity = newCapacity;
	}

	public IEnumerator<T> GetEnumerator(){ return ((IEnumerable<T>)_heap).GetEnumerator(); }
	IEnumerator IEnumerable.GetEnumerator(){ return _heap.GetEnumerator(); }

	public void CopyTo(T[] arr, int index){ _heap.CopyTo(arr, index); }

}



public class MaxHeap<T> : Heap<T> where T: IComparable<T>
{
	public MaxHeap() : base() {}
	public MaxHeap(IEnumerable<T> collection) : base(collection){}
	
	protected override bool Dominates(T x, T y){
		return x.CompareTo(y) >= 0;
	}
}



public class MinHeap<T> : Heap<T> where T: IComparable<T>
{
	public MinHeap() : base() {}
	public MinHeap(IEnumerable<T> collection) : base(collection){}

	
	protected override bool Dominates(T x, T y){
		return x.CompareTo(y) <= 0;
	}
}
