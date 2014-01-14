using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using System.Net.Sockets;
using System;
using System.Net;

public class AlternativMUDUDPClient : MonoBehaviour {
	public GameObject enemyPrefab;
	public float packetsPerSecond;
	public int sentPackets;
	public int receivedPackets;

	private GameObject playerChildren = null;
	private AlternativMUDClient alternativMUDClientScript;
	private delegate void ExecuteInUpdate();
	private Queue<ExecuteInUpdate> executeInUpdate = new Queue<ExecuteInUpdate>();
	private Dictionary<Byte, GameObject> enemies = new Dictionary<Byte, GameObject>();
	private float minPacketInterval;
	private float packetTimer = 0f;
	private UdpClient udpClient = null;
	private byte myID = 0;
	
	void Start () {
		minPacketInterval = 1f / packetsPerSecond;
		sentPackets = 0;
		receivedPackets = 0;

		alternativMUDClientScript = GetComponent<AlternativMUDClient> ();
		alternativMUDClientScript.AddListener (AlternativeMUDClasses.MSG_U3DM_SCENE_ENTER_SUCCEEDED, SceneEnterSuccess);
		alternativMUDClientScript.AddListener (AlternativeMUDClasses.MSG_U3DM_ENEMY_LEFT, EnemyLeft);
		alternativMUDClientScript.AddListener (AlternativeMUDClasses.MSG_U3DM_ENEMY_ARRIVED, EnemyArrived);
		Debug.Log ("Added listener on MSG_U3DM_SCENE_ENTER_SUCCEEDED");
	}

	void SceneEnterSuccess(string jsonData) {
		Debug.Log ("SceneEnterSuccess");
		Debug.Log ("SceneEnterSuccess: "+jsonData);
		executeInUpdate.Enqueue (SceneEnterSuccessSync);

		var N = JSON.Parse (jsonData);
		myID = (byte)N ["characterID"].AsInt;

		//Debug.Log (N["enemies"]);
		enemies.Clear ();
		foreach(string key in N["enemies"].AsObject.Keys) {
			JSONNode enemy = N["enemies"][key];
			Debug.Log ("Found enemy "+key+":"+enemy.ToString());
			byte characterID = (byte) int.Parse(key);
			executeInUpdate.Enqueue(delegate() {
				//if(characterID != myID) enemies.Add(characterID, Instantiate(enemyPrefab, Vector3.zero, Quaternion.identity) as GameObject);
				if(characterID != myID) {
					//enemies.Add(characterID, Instantiate(enemyPrefab, Vector3.zero, Quaternion.identity) as GameObject);
					UMADynamicAvatar avatar = LoadUMA(enemy["umaPackedRecipe"], null, "Enemy", false);
					enemies.Add (characterID, avatar.gameObject);
				}
			});

		}

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

		if (playerChildren == null) {
			playerChildren = GameObject.FindWithTag ("PlayerChildren");
			if(playerChildren == null) Debug.LogError("Could not find #PlayerChildren"); 
		}

		LoadUMA (loginPanel.selectedCharacter["umaPackedRecipe"], playerChildren.transform, "Player", true);
	}

	void EnemyArrived (string jsonData) {
		var N = JSON.Parse (jsonData);
		byte characterID = (byte)N["characterID"].AsInt;
		executeInUpdate.Enqueue(delegate() {
			if(characterID != myID) {
				//enemies.Add(characterID, Instantiate(enemyPrefab, Vector3.zero, Quaternion.identity) as GameObject);
				UMADynamicAvatar avatar = LoadUMA(N["character"]["umaPackedRecipe"], null, "Enemy", false);
				enemies.Add (characterID, avatar.gameObject);
			}
		});
	}

	void EnemyLeft(string jsonData) {
		var N = JSON.Parse (jsonData);
		byte characterID = (byte)N["characterID"].AsInt;
		executeInUpdate.Enqueue(delegate() {
			if(enemies.ContainsKey(characterID)) {
				GameObject enemy = enemies[characterID];
				Destroy (enemy);
				enemies.Remove(characterID);
			}
		});
	}
	
	void Update () {
		if (playerChildren == null) {
			playerChildren = GameObject.FindGameObjectWithTag ("PlayerChildren");
		}

		packetTimer += Time.deltaTime;
		if (packetTimer > minPacketInterval) {
			if(udpClient != null) {
				byte [] sendBytes = new byte[25];
				sendBytes[0] = myID;
				if(BitConverter.IsLittleEndian) {
					byte [] dataArr = BitConverter.GetBytes ((Single)playerChildren.transform.position.x);
					Array.Reverse(dataArr);
					Buffer.BlockCopy (dataArr, 0, sendBytes, 1, 4);

					dataArr = BitConverter.GetBytes ((Single)playerChildren.transform.position.y);
					Array.Reverse(dataArr);
					Buffer.BlockCopy (dataArr, 0, sendBytes, 5, 4);
					
					dataArr = BitConverter.GetBytes ((Single)playerChildren.transform.position.z);
					Array.Reverse(dataArr);
					Buffer.BlockCopy (dataArr, 0, sendBytes, 9, 4);
					
					dataArr = BitConverter.GetBytes ((Single)playerChildren.transform.eulerAngles.x);
					Array.Reverse(dataArr);
					Buffer.BlockCopy (dataArr, 0, sendBytes, 13, 4);
					
					dataArr = BitConverter.GetBytes ((Single)playerChildren.transform.eulerAngles.y);
					Array.Reverse(dataArr);
					Buffer.BlockCopy (dataArr, 0, sendBytes, 17, 4);
					
					dataArr = BitConverter.GetBytes ((Single)playerChildren.transform.eulerAngles.z);
					Array.Reverse(dataArr);
					Buffer.BlockCopy (dataArr, 0, sendBytes, 21, 4);
					

				}
				else {
					Buffer.BlockCopy (BitConverter.GetBytes ((Single)playerChildren.transform.position.x), 0, sendBytes, 1, 4);
					Buffer.BlockCopy (BitConverter.GetBytes ((Single)playerChildren.transform.position.y), 0, sendBytes, 5, 4);
					Buffer.BlockCopy (BitConverter.GetBytes ((Single)playerChildren.transform.position.z), 0, sendBytes, 9, 4);
					Buffer.BlockCopy (BitConverter.GetBytes ((Single)playerChildren.transform.eulerAngles.x), 0, sendBytes, 13, 4);
					Buffer.BlockCopy (BitConverter.GetBytes ((Single)playerChildren.transform.eulerAngles.y), 0, sendBytes, 17, 4);
					Buffer.BlockCopy (BitConverter.GetBytes ((Single)playerChildren.transform.eulerAngles.z), 0, sendBytes, 21, 4);
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
					if(rCharacterID != 0 && rCharacterID != myID) {
						float posX = ReadSingleBigEndian(receiveBytes, offset); offset+=4;
						float posY = ReadSingleBigEndian(receiveBytes, offset); offset+=4;
						float posZ = ReadSingleBigEndian(receiveBytes, offset); offset+=4;
						float rotX = ReadSingleBigEndian(receiveBytes, offset); offset+=4;
						float rotY = ReadSingleBigEndian(receiveBytes, offset); offset+=4;
						float rotZ = ReadSingleBigEndian(receiveBytes, offset); offset+=4;
						//serverPositionMarker.transform.position = new Vector3(posX, posY, posZ);
						//serverPositionMarker.transform.eulerAngles = new Vector3(rotX, rotY, rotZ);
						if(enemies.ContainsKey(rCharacterID)) {
							GameObject enemy = enemies[rCharacterID];
							enemy.transform.position = new Vector3(posX, posY, posZ);
							enemy.transform.eulerAngles = new Vector3(rotX, rotY, rotZ);
						}
						/*else {
							Quaternion rotation = Quaternion.identity;
							rotation.eulerAngles = new Vector3(rotX, rotY, rotZ);
							enemies.Add(rCharacterID, Instantiate(enemyPrefab, new Vector3(posX, posY, posZ), rotation) as GameObject);
						}*/
					}
					else offset += 24;
				}
				receivedPackets++;	
				//serverPositionMarker.transform.position
			}
		}

		if (executeInUpdate.Count != 0) {
			executeInUpdate.Dequeue()();
		}
	}

	private UMADynamicAvatar LoadUMA(string jsonPackedRecipe, Transform child, string tag, bool addPlayerMouseMovement) {
		GameObject umaObjContext = GameObject.FindWithTag ("UMAContext");
		if (umaObjContext != null) {
			UMAContext context = umaObjContext.GetComponent<UMAContext>();
			UMADynamicAvatar avatar = UmaLoad.Load (jsonPackedRecipe, context, transform);
			avatar.gameObject.transform.position = playerChildren.transform.position;
			GameObject realPlayerObject = avatar.gameObject.transform.GetChild(0).transform.GetChild(0).gameObject;
			realPlayerObject.tag = tag;
			if(child != null) child.parent = realPlayerObject.transform;
			if(addPlayerMouseMovement) {
				PlayerMouseMovement playerMouseMovement = realPlayerObject.AddComponent<PlayerMouseMovement>();
			}
			return avatar;
		} else {
			Debug.LogError ("Cannot find #UMAContext");
		}
		return null;
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
