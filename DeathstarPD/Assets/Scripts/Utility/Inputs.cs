using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

/**
 * Klasse um registrierten Buttons eine Aktion in Form einer anonymen Funktion zuzuweisen,
 * welche bei aktivierung des jeweiligen Buttons ausgeführt werden soll.
 * 
 * Muss einmal pro Level auf einem gameObject gezogen werden.
 */
public class Inputs : MonoBehaviour {

	//Interface nach außen
	public interface IInputs {
		void Unregister(string name);
		void Unregister(KeyCode name);
		void Register(string name, Action<bool> action);
		void Register(KeyCode name, Action<bool> action);
		void Register(string name, Action action);
		void Register(KeyCode name, Action action);
	}

	//Innere Klasse
	private class STInputs : IInputs {

		private Dictionary<string, bool> states = new Dictionary<string, bool>();
		private Dictionary<string, HashSet<Action<bool>>> actions = new Dictionary<string, HashSet<Action<bool>>>();

		private Dictionary<KeyCode, bool> keystates = new Dictionary<KeyCode, bool>();
		private Dictionary<KeyCode, HashSet<Action<bool>>> keyactions = new Dictionary<KeyCode, HashSet<Action<bool>>>();



		public void Unregister(string name){
			actions.Remove(name);
			states.Remove(name);
		}

		public void Unregister(KeyCode key){
			keyactions.Remove(key);
			keystates.Remove(key);
		}



		public void Register(string name, Action action){
			Register(name, (bool b)=>{if(b)action();});
		}

		public void Register(KeyCode key, Action action){
			Register(key, (bool b)=>{if(b)action();});
		}



		public void Register(string name, Action<bool> action){
			states[name] = false;
			if(!actions.ContainsKey(name))
				actions[name] = new HashSet<Action<bool>>();
			actions[name].Add(action);
		}

		public void Register(KeyCode key, Action<bool> action){
			keystates[key] = false;
			if(!keyactions.ContainsKey(key))
				keyactions[key] = new HashSet<Action<bool>>();
			keyactions[key].Add(action);
		}


		
		public void Clear(){
			states.Clear();
			actions.Clear();
			keystates.Clear();
			keyactions.Clear();
		}

		public void Update(){
			//für alle überwachten Input Buttons
			foreach(string name in actions.Keys){
				bool oldstate = states[name];
				bool newstate = Input.GetButton(name);
				//bei einer Änderung
				if(oldstate ^ newstate){
					states[name] = newstate; //change state
					foreach(Action<bool> action in actions[name])
						action(newstate); //call method
				}
			}

			//für alle überwachten Input Keys
			foreach(KeyCode key in keyactions.Keys){
				bool oldstate = keystates[key];
				bool newstate = Input.GetKey(key);
				//bei einer Änderung
				if(oldstate ^ newstate){
					keystates[key] = newstate; //change state
					foreach(Action<bool> action in keyactions[key])
						action(newstate); //call method
				}
			}
		}


		/**
	 	* Singleton
		*/
		private static STInputs instance;
		private STInputs(){}
		public static STInputs Instance{get{
				if(instance==null) instance = new STInputs();
				return instance;
			}}
		public static STInputs I{get{return Instance;}}

	}
	// Ende der inneren Klasse


	//wird einmal pro Game-Loop aufgerufen
	private void Update(){
		STInputs.I.Update();
	}

	//Destruktor
	private void OnDestroy(){
		//registrierte buttons leeren
		STInputs.I.Clear();
	}

	public static IInputs I{get{return STInputs.I;}}
}
