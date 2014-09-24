using System;
using UnityEngine;

/**
 * Nachrichtenformat für das Nachrichtensystem
 * 
 * Quelle:
 * Mat Buckland - Programming Game AI by Example
*/
public class Telegram : IComparable<Telegram> {
	
	
	
	/// <summary>
	/// Absender der Nachricht
	/// </summary>
	public readonly object sender;
	
	
	
	/// <summary>
	/// Empfänger der Nachricht
	/// </summary>
	public readonly MessageReceiver receiver;
	
	
	
	/// <summary>
	/// die Nachricht selbst
	/// </summary>
	public readonly string message;
	
	
	
	/// <summary>
	/// Gewünschter Zeitpunkt der Nachrichten-Auslieferung
	/// </summary>
	public readonly float dispatchTime;
	
	
	
	/// <summary>
	/// Zusätzliche Daten der Nachricht
	/// </summary>
	public readonly object extraInfo;
	
	
	
	/// <summary>
	/// Initializes a new instance of the <see cref="Telegram"/> class.
	/// </summary>
	/// <param name='sender'>Absender der Nachricht</param>
	/// <param name='receiver'>Empfänger der Nachricht</param>
	/// <param name='message'>die Nachricht selbst</param>
	/// <param name='dispatchTime'>Gewünschter Zeitpunkt der Nachrichten-Auslieferung</param>
	/// <param name='extraInfo'>Zusätzliche Daten der Nachricht</param>
	public Telegram(object sender, MessageReceiver receiver, string message, float dispatchTime, object extraInfo){
		this.sender = sender;
		this.receiver = receiver;
		this.message = message;
		this.dispatchTime = dispatchTime;
		this.extraInfo = extraInfo;
	}
	
	
	
	/// <summary>
	/// Initializes a new instance of the <see cref="Telegram"/> class.
	/// </summary>
	/// <param name='receiver'>Empfänger der Nachricht</param>
	/// <param name='message'>die Nachricht selbst</param>
	/// <param name='dispatchTime'>Gewünschter Zeitpunkt der Nachrichten-Auslieferung</param>
	/// <param name='extraInfo'>Zusätzliche Daten der Nachricht</param>
	public Telegram(MessageReceiver receiver, string message, float dispatchTime, object extraInfo)
		: this(null, receiver, message, dispatchTime, extraInfo){}
	
	/// <summary>
	/// Initializes a new instance of the <see cref="Telegram"/> class.
	/// </summary>
	/// <param name='receiver'>Empfänger der Nachricht</param>
	/// <param name='message'>die Nachricht selbst</param>
	/// <param name='extraInfo'>Zusätzliche Daten der Nachricht</param>
	public Telegram(MessageReceiver receiver, string message, object extraInfo)
		: this(receiver, message, Time.time, extraInfo){}
	
	/// <summary>
	/// Initializes a new instance of the <see cref="Telegram"/> class.
	/// </summary>
	/// <param name='receiver'>Empfänger der Nachricht</param>
	/// <param name='message'>die Nachricht selbst</param>
	/// <param name='dispatchTime'>Gewünschter Zeitpunkt der Nachrichten-Auslieferung</param>
	public Telegram(MessageReceiver receiver, string message, float dispatchTime)
		: this(receiver, message, dispatchTime, null){}
	
	
	
	/// <summary>
	/// Initializes a new instance of the <see cref="Telegram"/> class.
	/// </summary>
	/// <param name='sender'>Absender der Nachricht</param>
	/// <param name='receiver'>Empfänger der Nachricht</param>
	/// <param name='message'>die Nachricht selbst</param>
	/// <param name='extraInfo'>Zusätzliche Daten der Nachricht</param>
	public Telegram(object sender, MessageReceiver receiver, string message, object extraInfo)
		: this(sender, receiver, message, Time.time, extraInfo){}
	
	/// <summary>
	/// Initializes a new instance of the <see cref="Telegram"/> class.
	/// </summary>
	/// <param name='sender'>Absender der Nachricht</param>
	/// <param name='receiver'>Empfänger der Nachricht</param>
	/// <param name='message'>die Nachricht selbst</param>
	public Telegram(object sender, MessageReceiver receiver, string message)
		: this(sender, receiver, message, null){}
	
	
	
	/// <summary>
	/// Initializes a new instance of the <see cref="Telegram"/> class.
	/// </summary>
	/// <param name='sender'>Absender der Nachricht</param>
	/// <param name='receiver'>Empfänger der Nachricht</param>
	/// <param name='message'>die Nachricht selbst</param>
	/// <param name='dispatchTime'>Gewünschter Zeitpunkt der Nachrichten-Auslieferung</param>
	public Telegram(object sender, MessageReceiver receiver, string message, float dispatchTime)
		: this(sender, receiver, message, dispatchTime, null){}
	
	
	
	/// <summary>
	/// Initializes a new instance of the <see cref="Telegram"/> class.
	/// </summary>
	/// <param name='receiver'>Empfänger der Nachricht</param>
	/// <param name='message'>die Nachricht selbst</param>
	public Telegram(MessageReceiver receiver, string message) : this(null, receiver, message){}
	
	
	
	/// <summary>
	/// Vergleicht zwei Nachrichten anhand deren geplanten Auslieferungszeitpunkt.
	/// </summary>
	/// <returns><code>
	/// this.dispatchTime.CompareTo(other.dispatchTime);
	/// </code></returns>
	/// <param name='other'>
	/// die andere Nachricht mit der Verglichen wird
	/// </param>
	public int CompareTo(Telegram other){
		//auslieferungszeit vergleichen
		int r = this.dispatchTime.CompareTo(other.dispatchTime);
		//wenn es nicht gleich ist, Ergebnis ausgeben
		if(r != 0) return r;
		//wenn Zeitpunkt gleich ist HashCode vergleichen
		return this.GetHashCode().CompareTo(other.GetHashCode());
	}
	
	
	
}
