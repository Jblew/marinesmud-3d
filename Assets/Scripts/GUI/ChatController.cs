using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;

//by Alex Aza: http://stackoverflow.com/questions/5923552/c-sharp-collection-with-automatic-item-removal
public class FixedSizedQueue<T> : Queue<T>
{
	private readonly int maxQueueSize;
	private readonly object syncRoot = new object();
	
	public FixedSizedQueue(int maxQueueSize)
	{
		this.maxQueueSize = maxQueueSize;
	}
	
	public new void Enqueue(T item)
	{
		lock (syncRoot)
		{
			base.Enqueue(item);
			if (Count > maxQueueSize)
				Dequeue(); // Throw away
		}
	}
}

public class ChatController : MonoBehaviour {

	public GUISkin guiSkin;
	public int maxNumOfLines = 5;

	private AlternativMUDClient alternativMUDClient = null;
	private FixedSizedQueue<string> messages;
	private int msgWidth;
	private bool typingMode = false;
	private string message = "";

	// Use this for initialization
	void Start () {
		messages =  new FixedSizedQueue<string>(maxNumOfLines);
		msgWidth = Screen.width / 3;

		messages.Enqueue ("======= CHAT =======");
		messages.Enqueue (" Press enter to start typing or exit typing.");
	}
	
	// Update is called once per frame
	void Update () {
		if (alternativMUDClient == null) {
			GameObject alternativMUDClientObject = GameObject.FindWithTag ("AlternativMUDClient");
			if (alternativMUDClientObject != null) {
				alternativMUDClient = alternativMUDClientObject.GetComponent<AlternativMUDClient> ();
				alternativMUDClient.AddListener (AlternativeMUDClasses.MSG_U3DM_CHAT_MESSAGE, OnChatMessage);
			} else {
				Debug.LogWarning ("PlayerMovement: Cannot find by tag: #AlternativMUDClient");
			}
		}
	}

	void OnGUI() {
		GUI.skin = guiSkin;

		int numOfLines = messages.Count;

		int lineHeight = 20;
		int y = Screen.height - numOfLines * lineHeight - 30;


		float alpha = 0f;
		foreach (string msg in messages) {
			alpha = Mathf.Min (1f, alpha+1f/(float)numOfLines);
			GUI.color = new Color(1f, 1f, 1f, alpha);
			GUI.Label (new Rect (10, y, msgWidth, lineHeight), msg);
			y += lineHeight;
		}

		if (typingMode) {
			if (Event.current.type == EventType.KeyDown && (Event.current.keyCode == KeyCode.Return)) {
				typingMode = false;
				if(message.Length > 0) {
					//Debug.Log ("Sending msg");
					//messages.Enqueue("[me] "+message);
					alternativMUDClient.SendMessage(AlternativeMUDClasses.CMD_U3DM_POST_CHAT_MESSAGE, "{\"message\":\""+message+"\"}");
				}
				message = "";
			}

			GUI.SetNextControlName ("ChatField");
			message = GUI.TextField (new Rect (10, y, msgWidth, lineHeight), message);

			if (GUI.GetNameOfFocusedControl () == string.Empty) {
				GUI.FocusControl ("ChatField");
			}
		}
		else {
			if (Event.current.type == EventType.KeyDown && (Event.current.keyCode == KeyCode.Return)) {
				typingMode = true;
			}
		}
	}

	public void AddMessage(string msg) {
		messages.Enqueue (msg);
	}

	void OnChatMessage(string jsonData) {

		var N = JSON.Parse (jsonData);
		messages.Enqueue (N["character"]["name"]+": "+N["message"]);
	}
}
