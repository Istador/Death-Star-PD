using UnityEngine; //Abhängigkeit: Unity.Time

/**
 * Nachrichtensystem
 * zur Übertragung zeitverzögerter Nachrichten
 * nicht Zeitverzögerte Nachrichten werden sofort ausgeliefert
 * 
 * Quelle:
 * Mat Buckland - Programming Game AI by Example
*/
public class MessageDispatcher {


	
	/// <summary>
	/// Priorisierte Warteschlange mit Mengencharakteristik (keine Doppelten)
	/// </summary>
	private PriorityQueue<Telegram> pq = new PriorityQueue<Telegram>();
	
	
	
	/// <summary>
	/// Nachricht an Empfänger ausliefern
	/// </summary>
	/// <param name='msg'>
	/// Die Nachricht die ausgeliefert wird
	/// </param>
	private void Discharge(Telegram msg){
		//Nachricht an MessageReceiver Methode übergeben
		if(msg.receiver != null)
			msg.receiver.HandleMessage(msg);
	}
	
	
	
	/// <summary>
	/// Nachricht an Nachrichtensystem zum Ausliefern übergeben
	/// </summary>
	/// <param name='msg'>
	/// Die Nachricht die ausgeliefert werden soll
	/// </param>
	public void Dispatch(Telegram msg){
		//sofort ausliefern
		if(msg.dispatchTime <= Time.time)
			Discharge(msg);
		//später ausliefern
		else
			pq.Enqueue(msg); //in Warteschlange tun
	}
	
	
	
	/// <summary>
	/// Nachricht an Nachrichtensystem zum Ausliefern übergeben
	/// </summary>
	/// <param name='msg'>
	/// Die Nachricht die ausgeliefert werden soll
	/// </param>
	public void Dispatch(object sender, MessageReceiver receiver, string msg, float delay = 0.0f, object extraInfo = null){
		Dispatch(new Telegram(sender, receiver, msg, Time.time+delay, extraInfo));
	}
	
	
	/// <summary>
	/// Nachricht an Nachrichtensystem zum Ausliefern übergeben
	/// </summary>
	/// <param name='msg'>
	/// Die Nachricht die ausgeliefert werden soll
	/// </param>
	public void Dispatch(MessageReceiver receiver, string msg, float delay = 0.0f, object extraInfo = null){
		Dispatch(new Telegram(null, receiver, msg, Time.time+delay, extraInfo));
	}
	
	
	
	/// <summary>
	/// Methode die kontinuierlich aufgerufen wird um verzögerte Nachrichten 
	/// auszuliefern
	/// </summary>
	public void DispatchDelayedMessages(){
		//aktuelle Zeit
		float now = Time.time;
		
		//Nachricht mit frühestem auslieferzeitpunkt
		Telegram t = pq.First;
		
		//wenn das Telegram jetzt ausgeliefert werden soll
		while(t != null && t.dispatchTime <= now && t.dispatchTime >= 0.0f){
			//ausliefern
			Discharge(t);
			//aus Warteschlange entfernen
			pq.RemoveFirst();
			//nächste Nachricht
			t = pq.First;
		}
	}
	
	
	
	/// <summary>
	/// Löscht alle noch verbleibenden Nachrichten, z.B. bei einem Level Neustart
	/// </summary>
	public void EmptyQueue(){
		pq.Clear();
	}
	
	
	/**
	 * Singleton
	*/
	private static MessageDispatcher instance;
	private MessageDispatcher(){}
	public static MessageDispatcher Instance{get{
			if(instance==null) instance = new MessageDispatcher();
			return instance;
	}}
	public static MessageDispatcher I{get{return Instance;}}
	
}