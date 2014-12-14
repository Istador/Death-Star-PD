using UnityEngine; //Abhängigkeit: Unity.GameObject

/*
 * Interface für Objekte die Nachrichten des Nachrichtensystems empfangen können
*/
public interface MessageReceiver {
	
	/// <summary>
	/// Methode um Nachrichten zu verarbeiten
	/// </summary>
	/// <returns>
	/// konnte die Nachricht erfolgreich verarbeitet werden?
	/// </returns>
	/// <param name='msg'>
	/// Die Nachricht die verarbeitet werden soll
	/// </param>
	bool HandleMessage(Telegram msg);
	
}

public class MessageReceiverWrapper : MessageReceiver {

	/// <summary>
	/// mcht aus einem GameObjejkt einen MessageReceiver, falls notwendig.
	/// </summary>
	/// <param name="obj">Object.</param>
	public static MessageReceiver wrap(GameObject obj){
		MessageReceiver mr = obj.GetComponent<GeneralObject>();

		//wenn es kein eigenes MessageReceiver Skript besitzt
		if(mr == null) mr = new MessageReceiverWrapper(obj);

		return mr;
	}

	GameObject obj;

	private MessageReceiverWrapper(GameObject obj){
		this.obj = obj;
	}

	public bool HandleMessage(Telegram msg){
		if(obj != null)
			//Sende über das Unity-Nachrichten-System
			obj.SendMessage(msg.message, msg.extraInfo, SendMessageOptions.DontRequireReceiver);
		return true;
	}

}