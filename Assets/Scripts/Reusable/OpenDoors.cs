using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;

public class OpenDoors : MonoBehaviour {
	public string nextSceneName;
	public string message;
	public string failMessage;

	private delegate void ExecuteInUpdate();
	private GameObject player = null;
	private bool changingScene = false;
	private bool sceneChangeEnabled = false;
	private Queue<ExecuteInUpdate> executeInUpdate = new Queue<ExecuteInUpdate>();
	private AlternativMUDClient alternativMUDClient = null;
	private GUIBigLabelFader labelFader = null;
	private string failMsgServer = "unknown reason";
	private bool failed = false;

	void Start() {
		if (alternativMUDClient == null) {
			alternativMUDClient = GameObject.FindWithTag("AlternativMUDClient").GetComponent<AlternativMUDClient>();
		}

		if (labelFader == null) {
			labelFader = GameObject.FindWithTag("GUIBigLabel").GetComponent<GUIBigLabelFader>();
		}
		if(labelFader == null) Debug.LogWarning("Could not find #GUIBigLabel");

		if (alternativMUDClient != null) {
			alternativMUDClient.AddListener (AlternativeMUDClasses.MSG_U3DM_SCENE_ENTER_FAILED, SceneEnterFailed);
			alternativMUDClient.AddListener (AlternativeMUDClasses.MSG_U3DM_SCENE_ENTER_SUCCEEDED, SceneEnterSucceeded);
		}
		else Debug.LogWarning("Could not find #AlternativMUDClient");
	}

	void Update() {
		if (player == null) {
			player = GameObject.FindGameObjectWithTag (Tags.PLAYER);	
		}

		if (sceneChangeEnabled) {
			if(labelFader != null) {
				if(failed) labelFader.Show (failMessage+" ("+failMsgServer+")");
				else labelFader.Show (message, 0.5f);
			}

			if (!changingScene &&  Input.GetButton ("Jump")) {
				Debug.Log ("Changing scene!");
				changingScene = true;
				if(alternativMUDClient != null) alternativMUDClient.SendMessage (AlternativeMUDClasses.CMD_U3DM_CHANGE_SCENE, "{\"sceneName\":\"" + nextSceneName + "\"}");
				else Debug.LogWarning("Cannot change scene: alternativMUDClient is null");
				Debug.Log ("Sent request!");
			}
		}

		if (executeInUpdate.Count != 0) {
			executeInUpdate.Dequeue()();
		}
	}

	void OnTriggerEnter(Collider other) {
		if (player != null && other.gameObject == player) {
			sceneChangeEnabled = true;
			failed = false;
		}
	}

	void OnTriggerExit(Collider other) {
		if (player != null && other.gameObject == player) {
			changingScene = false;
			sceneChangeEnabled = false;
		}
	}

	void SceneEnterFailed(string jsonData) {
		var N = JSON.Parse (jsonData);
		failMsgServer = N ["message"];
		failed = true;
	}

	void SceneEnterSucceeded(string jsonData) {
		if (changingScene) {
			executeInUpdate.Enqueue (ChangeScene);
		}
	}

	void ChangeScene() {
		Debug.Log ("Trying to load level "+nextSceneName);
		Application.LoadLevel (nextSceneName);
	}
}
