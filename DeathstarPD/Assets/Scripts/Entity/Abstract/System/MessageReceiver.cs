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
