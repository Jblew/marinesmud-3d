using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using System.Net.Sockets;
using System;
using System.Net;

public class AlternativMUDUDPClient : MonoBehaviour {
	public float packetsPerSecond;
	public GameObject player;
	public GameObject serverPositionMarker;
	public int sentPackets;
	public int receivedPackets;

	private AlternativMUDClient alternativMUDClientScript;
	private delegate void ExecuteInUpdate();
	private Queue<ExecuteInUpdate> executeInUpdate = new Queue<ExecuteInUpdate>();
	private float minPacketInterval;
	private float packetTimer = 0f;
	private UdpClient udpClient = null;
	private byte characterID = 0;
	
	void Start () {
		minPacketInterval = 1f / packetsPerSecond;
		sentPackets = 0;
		receivedPackets = 0;

		alternativMUDClientScript = GetComponent<AlternativMUDClient> ();
		alternativMUDClientScript.AddListener (AlternativeMUDClasses.MSG_U3DM_SCENE_ENTER_SUCCEEDED, SceneEnterSuccess);
		Debug.Log ("Added listener on MSG_U3DM_SCENE_ENTER_SUCCEEDED");
	}

	void SceneEnterSuccess(string jsonData) {
		Debug.Log ("SceneEnterSuccess");
		Debug.Log ("SceneEnterSuccess: "+jsonData);
		executeInUpdate.Enqueue (SceneEnterSuccessSync);

		var N = JSON.Parse (jsonData);
		characterID = (byte)N ["characterID"].AsInt;

		if (udpClient != null) {
			udpClient.Close();
		}
		udpClient = new UdpClient (1100);
		udpClient.Connect(alternativMUDClientScript.hostname, N["port"].AsInt);
	}

	void SceneEnterSuccessSync() {
		GUILoginPanel loginPanel = GetComponent<GUILoginPanel> ();
		if (loginPanel != null) {
			loginPanel.enabled = false;
		}
	}
	
	void Update () {
		packetTimer += Time.deltaTime;
		if (packetTimer > minPacketInterval) {
			if(udpClient != null) {
				byte [] sendBytes = new byte[25];
				sendBytes[0] = characterID;
				if(BitConverter.IsLittleEndian) {
					byte [] dataArr = BitConverter.GetBytes ((Single)player.transform.position.x);
					Array.Reverse(dataArr);
					Buffer.BlockCopy (dataArr, 0, sendBytes, 1, 4);

					dataArr = BitConverter.GetBytes ((Single)player.transform.position.y);
					Array.Reverse(dataArr);
					Buffer.BlockCopy (dataArr, 0, sendBytes, 5, 4);
					
					dataArr = BitConverter.GetBytes ((Single)player.transform.position.z);
					Array.Reverse(dataArr);
					Buffer.BlockCopy (dataArr, 0, sendBytes, 9, 4);
					
					dataArr = BitConverter.GetBytes ((Single)player.transform.eulerAngles.x);
					Array.Reverse(dataArr);
					Buffer.BlockCopy (dataArr, 0, sendBytes, 13, 4);
					
					dataArr = BitConverter.GetBytes ((Single)player.transform.eulerAngles.y);
					Array.Reverse(dataArr);
					Buffer.BlockCopy (dataArr, 0, sendBytes, 17, 4);
					
					dataArr = BitConverter.GetBytes ((Single)player.transform.eulerAngles.z);
					Array.Reverse(dataArr);
					Buffer.BlockCopy (dataArr, 0, sendBytes, 21, 4);
					

				}
				else {
					Buffer.BlockCopy (BitConverter.GetBytes ((Single)player.transform.position.x), 0, sendBytes, 1, 4);
					Buffer.BlockCopy (BitConverter.GetBytes ((Single)player.transform.position.y), 0, sendBytes, 5, 4);
					Buffer.BlockCopy (BitConverter.GetBytes ((Single)player.transform.position.z), 0, sendBytes, 9, 4);
					Buffer.BlockCopy (BitConverter.GetBytes ((Single)player.transform.eulerAngles.x), 0, sendBytes, 13, 4);
					Buffer.BlockCopy (BitConverter.GetBytes ((Single)player.transform.eulerAngles.y), 0, sendBytes, 17, 4);
					Buffer.BlockCopy (BitConverter.GetBytes ((Single)player.transform.eulerAngles.z), 0, sendBytes, 21, 4);
				}
				udpClient.Send(sendBytes, sendBytes.Length);
				sentPackets++;
			}
			packetTimer = 0f;
		}
		if (udpClient != null) {
			if(udpClient.Available > 0) {
				IPEndPoint remoteIpEndPoint = new IPEndPoint(IPAddress.Any, 0);
				Byte[] receiveBytes = udpClient.Receive(ref remoteIpEndPoint);
				int offset = 0;
				while(receiveBytes.Length-offset >= 25) {
					byte rCharacterID = receiveBytes[offset]; offset+=1;
					float posX = ReadSingleBigEndian(receiveBytes, offset); offset+=4;
					float posY = ReadSingleBigEndian(receiveBytes, offset); offset+=4;
					float posZ = ReadSingleBigEndian(receiveBytes, offset); offset+=4;
					float rotX = ReadSingleBigEndian(receiveBytes, offset); offset+=4;
					float rotY = ReadSingleBigEndian(receiveBytes, offset); offset+=4;
					float rotZ = ReadSingleBigEndian(receiveBytes, offset); offset+=4;
					serverPositionMarker.transform.position = new Vector3(posX, posY, posZ);
					serverPositionMarker.transform.eulerAngles = new Vector3(rotX, rotY, rotZ);
				}
				receivedPackets++;
				//serverPositionMarker.transform.position
			}
		}

		if (executeInUpdate.Count != 0) {
			executeInUpdate.Dequeue()();
		}
	}

	public static float ReadSingleBigEndian(byte[] data, int offset)
	{
		return ReadSingle(data, offset, false);
	}
	public static float ReadSingleLittleEndian(byte[] data, int offset)
	{
		return ReadSingle(data, offset, true);
	}
	private static float ReadSingle(byte[] data, int offset, bool littleEndian)
	{
		if (BitConverter.IsLittleEndian != littleEndian)
		{   // other-endian; reverse this portion of the data (4 bytes)
			byte tmp = data[offset];
			data[offset] = data[offset + 3];
			data[offset + 3] = tmp;
			tmp = data[offset + 1];
			data[offset + 1] = data[offset + 2];
			data[offset + 2] = tmp;
		}
		return BitConverter.ToSingle(data, offset);
	}
}
