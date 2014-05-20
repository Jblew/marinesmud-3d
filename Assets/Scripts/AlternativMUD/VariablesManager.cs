using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;

public class VariablesManager : MonoBehaviour {
	public Dictionary<string, string> variables = new Dictionary<string, string>();
	private AlternativMUDClient alternativMudClient;

	// Use this for initialization
	void Start () {
		alternativMudClient = GetComponent<AlternativMUDClient> ();
		alternativMudClient.AddListener (AlternativeMUDClasses.MSG_U3DM_SCENE_ENTER_SUCCEEDED, SceneEnterSucceeded);
		alternativMudClient.AddListener (AlternativeMUDClasses.MSG_U3DM_VARIABLE_CHANGED, VariableChanged);
	}

	void SceneEnterSucceeded(string jsonData) {
		var N = JSON.Parse (jsonData);
		foreach(string key in N["variables"].AsObject.Keys) {
			string value = N["variables"][key];
			variables[key] = value;
		}
	}
	void VariableChanged(string jsonData) {
		var N = JSON.Parse (jsonData);
		variables [N ["key"]] = N ["value"];
	}

	public string get(string key, string defaultValue) {
		if (variables.ContainsKey (key)) {
						return variables [key];
				} else
						return defaultValue;
	}
}
