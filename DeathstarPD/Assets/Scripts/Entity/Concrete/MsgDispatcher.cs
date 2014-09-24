using UnityEngine;
using System.Collections;

/// 
/// Der Message-Dispatcher wird einmal in jedem Level benötigt (z.B. auf
/// dem Spieler lagernd), um beim Nachrichtensystem die Methode aufzurufen die 
/// zeitverzögerte Nachrichten verschickt.
/// 
/// Schöner als in LateUpdate (nach den Update-Methoden aller Objekte) wäre dies 
/// in einer EarlyUpdate Methode (vor den Update-Methoden alle Objekte), aber die 
/// gibt es in Unity nicht. Dann würden die Nachrichten vor dem ganzem Update kram 
/// ausgeführt werden, und dadurch verursachte Zustandswechsel müssten nicht einen 
/// ganzen Frame warten bis sie auswirkungen haben können.
/// 
public class MsgDispatcher : MonoBehaviour {
	
	/// <summary>
	/// Liefert zeitverzögerte Nachrichten aus
	/// </summary>
	void LateUpdate(){
		MessageDispatcher.I.DispatchDelayedMessages();
	}

	//Destruktor
	~MsgDispatcher(){
		//Message Dispatcher leeren, 
		//damit keine alten Nachrichten in neuen Levels ausgeliefert werden.
		//(was scheitern würde, weil die gameObjects nicht mehr existieren)
		MessageDispatcher.I.EmptyQueue();
	}
}
