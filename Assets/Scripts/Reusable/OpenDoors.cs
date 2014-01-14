using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;

public class OpenDoors : MonoBehaviour {
	public AlternativMUDClient alternativMUDClient;
	public GUIBigLabelFader labelFader;
	public string nextSceneName;
	public string message;
	public string failMessage;

	private delegate void ExecuteInUpdate();
	private GameObject player = null;
	private bool changingScene = false;
	private bool sceneChangeEnabled = false;
	private Queue<ExecuteInUpdate> executeInUpdate = new Queue<ExecuteInUpdate>();

	void Start() {
		alternativMUDClient.AddListener (AlternativeMUDClasses.MSG_U3DM_SCENE_ENTER_FAILED, SceneEnterFailed);
		alternativMUDClient.AddListener (AlternativeMUDClasses.MSG_U3DM_SCENE_ENTER_SUCCEEDED, SceneEnterSucceeded);
	}

	void Update() {
		if (player == null) {
			player = GameObject.FindGameObjectWithTag (Tags.PLAYER);	
		}

		if (sceneChangeEnabled) {
			labelFader.Show (message, 0.5f);

			if (!changingScene &&  Input.GetButton ("Jump")) {
				Debug.Log ("Changing scene!");
				changingScene = true;
				alternativMUDClient.SendMessage (AlternativeMUDClasses.CMD_U3DM_CHANGE_SCENE, "{\"sceneName\":\"" + nextSceneName + "\"}");
			}
		}

		if (executeInUpdate.Count != 0) {
			executeInUpdate.Dequeue()();
		}
	}

	void OnTriggerEnter(Collider other) {
		if (player != null && other.gameObject == player) {
			sceneChangeEnabled = true;
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
		labelFader.Show (failMessage+" ("+N["message"]+")");

	}

	void SceneEnterSucceeded(string jsonData) {
		if (changingScene) {
			executeInUpdate.Enqueue (ChangeScene);
		}
	}

	void ChangeScene() {
		Application.LoadLevel (nextSceneName);
	}
}
