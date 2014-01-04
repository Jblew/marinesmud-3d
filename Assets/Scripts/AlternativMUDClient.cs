using UnityEngine;
using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using SimpleJSON;

public class AlternativeMUDClasses {
	public static string CMD_PERFORM_LOGIN = "net.alternativmud.system.nebus.server.StandardBusSubscriber$PerformLogin";
	public static string MSG_LOGIN_SUCCEEDED = "net.alternativmud.system.nebus.server.StandardBusSubscriber$LoginSucceeded";
	public static string MSG_LOGIN_FAILED = "net.alternativmud.system.nebus.server.StandardBusSubscriber$LoginFailed";
	public static string CMD_GET_STATUS = "net.alternativmud.system.nebus.server.StandardBusSubscriber$GetStatus";
	public static string MSG_STATUS = "net.alternativmud.system.nebus.server.StandardBusSubscriber$Status";

	public static string CMD_AUTH_GET_CHARACTERS = "net.alternativmud.system.nebus.server.AuthenticatedBusSubscriber$GetCharacters";
	public static string MSG_AUTH_CHARACTERS = "net.alternativmud.system.nebus.server.AuthenticatedBusSubscriber$Characters";
	public static string CMD_AUTH_START_GAMEPLAY = "net.alternativmud.system.nebus.server.AuthenticatedBusSubscriber$StartGameplay";
	public static string MSG_AUTH_GAMEPLAY_STARTED = "net.alternativmud.system.nebus.server.AuthenticatedBusSubscriber$GameplayStarted";
	public static string MSG_AUTH_GAMEPLAY_START_FAILED = "net.alternativmud.system.nebus.server.AuthenticatedBusSubscriber$GameplayStartFailed";
	public static string CMD_AUTH_GET_TIME_MACHINE = "net.alternativmud.system.nebus.server.AuthenticatedBusSubscriber$GetTimeMachine";
	public static string MSG_AUTH_TIME_MACHINE = "net.alternativmud.logic.time.TimeMachine";
	public static string CMD_AUTH_ENTER_UNITY3D_MODE = "net.alternativmud.system.nebus.server.AuthenticatedBusSubscriber$EnterUnity3DMode";
	public static string MSG_AUTH_UNITY3D_MODE_ENTER_FAILED = "net.alternativmud.system.nebus.server.AuthenticatedBusSubscriber$Unity3DModeEnterFailed";

	public static string MSG_U3DM_SCENE_ENTER_SUCCEEDED = "net.alternativmud.system.unityserver.Unity3DModeSubscriber$SceneEnterSucceeded";
}

class ConnectionMaintainer {
	public bool running = false;
	public bool connected = false;
	public bool connectionFailed = false;

	public IDictionary<string, List<AlternativMUDClient.OnMessage>> eventListeners = new Dictionary<string, List<AlternativMUDClient.OnMessage>>();

	private string hostname;
	private int port;
	private UTF8Encoding utfEncoding = new UTF8Encoding();
	private Queue<string []> msgQueue = new Queue<string []>();

	public ConnectionMaintainer(string _hostname, int _port) {
		hostname = _hostname;
		port = _port;
	}

	public static int SwapEndianness(int value)
	{
		var b1 = (value >> 0) & 0xff;
		var b2 = (value >> 8) & 0xff;
		var b3 = (value >> 16) & 0xff;
		var b4 = (value >> 24) & 0xff;
		
		return b1 << 24 | b2 << 16 | b3 << 8 | b4 << 0;
	}

	private byte [] BuildMessage(string className, string json)
	{

		byte [] classNameBytes = utfEncoding.GetBytes (className);
		byte [] jsonBytes = utfEncoding.GetBytes (json);
		int length = 4 + classNameBytes.Length + jsonBytes.Length;

		byte [] output = new byte[length+4];
		if (BitConverter.IsLittleEndian) {
			Buffer.BlockCopy (BitConverter.GetBytes ((Int32)SwapEndianness (length)), 0, output, 0, 4);
			Buffer.BlockCopy (BitConverter.GetBytes ((Int32)SwapEndianness (classNameBytes.Length)), 0, output, 4, 4);
			Buffer.BlockCopy (classNameBytes, 0, output, 8, classNameBytes.Length);
			Buffer.BlockCopy (jsonBytes, 0, output, 8 + classNameBytes.Length, jsonBytes.Length);
		} else {
			Buffer.BlockCopy (BitConverter.GetBytes ((Int32)length), 0, output, 0, 4);
			Buffer.BlockCopy (BitConverter.GetBytes ((Int32)classNameBytes.Length), 0, output, 4, 4);
			Buffer.BlockCopy (classNameBytes, 0, output, 8, classNameBytes.Length);
			Buffer.BlockCopy (jsonBytes, 0, output, 8 + classNameBytes.Length, jsonBytes.Length);
		}

		return output;
	}

	public void Run() {
		running = true;

		while(running) {
			Debug.Log ("Trying to connect to server on "+hostname+":"+port);
			TcpClient client = new TcpClient(hostname,port);
			try{
				NetworkStream s = client.GetStream();
				//StreamReader sr = new StreamReader(s);
				//BinaryWriter sw = new BinaryWriter(s);
				//sw.AutoFlush = true;
				Debug.Log ("Got AlternativMUD eBus stream");

				byte [] dataToWrite = BuildMessage(AlternativeMUDClasses.CMD_GET_STATUS, "{}");
				//Debug.Log ("dataToWrite: "+BitConverter.ToString(dataToWrite).Replace("-", string.Empty));

				s.Write(dataToWrite, 0, dataToWrite.Length);
				s.Flush();

				while(running){
					if(s.DataAvailable) {
						byte [] lengthBytes = {(byte)s.ReadByte(), (byte)s.ReadByte(), (byte)s.ReadByte(), (byte)s.ReadByte()};

						int length = (BitConverter.IsLittleEndian? SwapEndianness(BitConverter.ToInt32(lengthBytes, 0)) : BitConverter.ToInt32(lengthBytes, 0));
						//Debug.Log ("leftToRead="+leftToRead);

						byte [] data = new byte[length];
						int dataOffset = 0;
						int readBytes = s.Read(data, 0, length);
						if(readBytes != length) Debug.Log ("Read incomplete message (readBytes["+readBytes+"] != leftToRead["+length+"])");

						int classNameLength = (BitConverter.IsLittleEndian? SwapEndianness(BitConverter.ToInt32(data, dataOffset)) : BitConverter.ToInt32(data, dataOffset));//4 = length of this field
						dataOffset += 4;

						string className = utfEncoding.GetString(data, dataOffset, classNameLength);
						dataOffset += classNameLength;

						processMessage(className, utfEncoding.GetString(data, dataOffset, length - classNameLength - 4));
					}
					else if(msgQueue.Count != 0) {
						string [] msgEntry = msgQueue.Dequeue();
						//Debug.Log ("Dequeued msg entry "+msgEntry[0]+"; json: "+msgEntry[1]);
						if(msgEntry != null) {
							byte [] msgToWrite = BuildMessage(msgEntry[0], msgEntry[1]);
							s.Write(msgToWrite, 0, msgToWrite.Length);
							s.Flush();
							//Debug.Log ("Written msg entry");
						}
					}
					else {
						Thread.Sleep(10);
					}
					//Thread.Sleep (1000);
				}
				s.Close();
				connected = false;
			}finally{
				client.Close();
				connected = false;
			}
			Debug.Log("Connection closed, reconnecting in 5 seconds...");
			connectionFailed = true;
			Thread.Sleep(5000);
		}
	}

	private void processMessage(string className, string jsonData) {
		if (eventListeners.ContainsKey (className)) {
			List<AlternativMUDClient.OnMessage> listeners = eventListeners[className];
			foreach (AlternativMUDClient.OnMessage listener in listeners) {
				listener(jsonData);
			}
		}

		if(className == AlternativeMUDClasses.MSG_LOGIN_SUCCEEDED) {
		}
		else if(className == AlternativeMUDClasses.MSG_LOGIN_FAILED) {
		}
		else if(className == AlternativeMUDClasses.MSG_STATUS) {
			var N = JSON.Parse(jsonData);
			Debug.Log ("Got AlternativMUD status: title=" + N["title"] +", subtitle="+N["subtitle"]);
			connected = true;
			connectionFailed = false;
		}
		else if(className == AlternativeMUDClasses.MSG_AUTH_CHARACTERS) {
		}
		else if(className == AlternativeMUDClasses.MSG_AUTH_GAMEPLAY_STARTED) {
		}
		else if(className == AlternativeMUDClasses.MSG_AUTH_GAMEPLAY_START_FAILED) {
		}
		else if(className == AlternativeMUDClasses.MSG_AUTH_TIME_MACHINE) {
		}
		else {
			//Debug.Log ("Unknown type of AlternativMUD message: "+className);
		}

	}

	public void SendMessage(string className, string jsonData) {
		string [] queueEntry = new string[2];
		queueEntry [0] = className;
		queueEntry [1] = jsonData;
		msgQueue.Enqueue (queueEntry);
		//Debug.Log ("Added "+className+" to msg queue");
	}
}

public class AlternativMUDClient : MonoBehaviour {
	public delegate void OnMessage(string jsonData);

	public string hostname;
	public int port;
	public AudioClip connectionEstabilishedSound;
	public AudioClip connectionFailedSound;
	public TextMesh statusTextMesh;
	public InfoTextFX infoTextFx;

	private GUILoginPanel guiLoginPanelScript;
	private bool connected = false;
	private bool connectionFailed = false;
	private bool authDataCorrect = false;
	private string login;
	private string password;
	private bool failure;

	private static ConnectionMaintainer connectionMaintainer = null;

	// Use this for initialization
	void Start () {
		DontDestroyOnLoad (gameObject);

		guiLoginPanelScript = GetComponent<GUILoginPanel> ();

		if (connectionMaintainer == null) {
			connectionMaintainer = new ConnectionMaintainer(hostname, port);
		}

		if (connectionMaintainer.running == false) {
			Thread t = new Thread(new ThreadStart(connectionMaintainer.Run));
			t.Start();
		}
	}

	void Update() {
		if (!connected && connectionMaintainer.connected) {
			AudioSource.PlayClipAtPoint(connectionEstabilishedSound, Camera.main.transform.position);
			statusTextMesh.text = "Connected";
			infoTextFx.fxOn = false;

			if(!authDataCorrect) {
				guiLoginPanelScript.enabled = true;
			}
		}
		if (!connectionFailed && connectionMaintainer.connectionFailed) {
			AudioSource.PlayClipAtPoint(connectionFailedSound, Camera.main.transform.position);
			statusTextMesh.text = "Connection failed";
			infoTextFx.fxOn = true;
		}
		connected = connectionMaintainer.connected;
		connectionFailed = connectionMaintainer.connectionFailed;
	}

	public void AddListener(string className, OnMessage eventListener) {
		if(connectionMaintainer == null) {
			connectionMaintainer = new ConnectionMaintainer(hostname, port);
		}

		if(!connectionMaintainer.eventListeners.ContainsKey(className)) {
			connectionMaintainer.eventListeners[className] = new List<OnMessage>();
		}

		connectionMaintainer.eventListeners [className].Add (eventListener);
	}

	public void SendMessage(string className, string json) {
		if (connectionMaintainer == null)
						Debug.Log ("Trying to send message ("+className+"), while connectionMaintainer is null!");
		else connectionMaintainer.SendMessage (className, json);
	}
}
