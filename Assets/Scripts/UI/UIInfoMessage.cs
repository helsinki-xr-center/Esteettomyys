
/**
 * Author: Nomi Lakkala
 * 
 * <summary>UI message handler that handles communication between different message producers and the consumer.
 * For example:
 * <example>
 * <code>
 * new UIInfoMessage("Hello!").Deliver();
 * </code>
 * </example>
 * </summary>
 */
public class UIInfoMessage
{
	public enum MessageType
	{
	Info,
	Success,
	Error
	}


	public delegate void UIInfoMessageDelegate(UIInfoMessage message);
	private static event UIInfoMessageDelegate StaticEvent;

	public string message;
	public MessageType messageType;

	public UIInfoMessage(string message, MessageType type)
	{
		this.message = message;
		this.messageType = type;
	}

	public UIInfoMessage(string message) : this(message, MessageType.Info)
	{ }

	/**
	 * <summary>Delivers this message to the consumers.</summary>
	 */
	public void Deliver(){
		StaticEvent?.Invoke(this);
	}

	/**
	 * <summary>Adds a consumer for this message type. Remember to call <see cref="RemoveListener"/> before the consumer is destroyed.</summary>
	 */
	public static void AddListener(UIInfoMessageDelegate del) => StaticEvent += del;
	/**
	 * <summary>Removes a consumer from this message type.</summary>
	 */
	public static void RemoveListener(UIInfoMessageDelegate del) => StaticEvent -= del;
}
