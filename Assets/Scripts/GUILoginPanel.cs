using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;

public class GUILoginPanel : MonoBehaviour
{
	public AlternativMUDClient alternativeMUDClientScript;
	public bool loginFailed;
	public bool loginSucceeded;
	public bool finished = false;
	public string correctLogin = "";
	public string correctPassword = "";
	public string selectedCharacter = "";

	private delegate void ExecuteInUpdate();
	private string login = "";
	private string password = "";
	private Queue<ExecuteInUpdate> executeInUpdate = new Queue<ExecuteInUpdate>();

	void Awake ()
	{
		alternativeMUDClientScript.AddListener (AlternativeMUDClasses.MSG_LOGIN_SUCCEEDED, OnLoginSucceeded);
		alternativeMUDClientScript.AddListener (AlternativeMUDClasses.MSG_LOGIN_FAILED, OnLoginFailed);
		alternativeMUDClientScript.AddListener (AlternativeMUDClasses.MSG_AUTH_CHARACTERS, OnCharactersList);
	}

	void OnGUI ()
	{
		int boxWidth = 220;
		int boxHeight = 150;
		int boxX = Screen.width / 2 - boxWidth / 2;
		int boxY = Screen.height / 2 - boxHeight / 2;

		GUI.Box (new Rect (boxX, boxY, boxWidth, boxHeight), "Log in");
		GUI.Label (new Rect (boxX + 10, boxY + 30, boxWidth / 2 - 10, 30), "Login");
		GUI.Label (new Rect (boxX + 10, boxY + 60, boxWidth / 2 - 10, 30), "Password");

		GUI.SetNextControlName ("LoginField");
		login = GUI.TextField (new Rect (boxX + boxWidth / 2 + 10, boxY + 30, boxWidth / 2 - 20, 20), login);

		GUI.SetNextControlName ("PasswordField");
		password = GUI.PasswordField (new Rect (boxX + boxWidth / 2 + 10, boxY + 60, boxWidth / 2 - 20, 20), password, '*');
					
		if (loginFailed) {
			GUI.contentColor = Color.red;
			GUI.Label (new Rect (boxX + 10, boxY + 95, boxWidth / 2 - 10, 40), "Failed");
		}

		GUI.contentColor = Color.white;
		if (GUI.Button (new Rect (boxX + boxWidth / 2 + 10, boxY + 90, boxWidth / 2 - 20, 30), "Log in")) {
			alternativeMUDClientScript.SendMessage(AlternativeMUDClasses.CMD_PERFORM_LOGIN, "{\"login\":\""+login+"\",\"password\":\""+password+"\"}");
		}
	
		if (GUI.GetNameOfFocusedControl () == string.Empty) {
			GUI.FocusControl ("LoginField");
		}
	}

	public void OnLoginSucceeded (string jsonData)
	{
		loginSucceeded = true;
		loginFailed = false;
		correctLogin = login;
		correctPassword = password;
		Debug.Log ("Login Succeeded!");
		alternativeMUDClientScript.SendMessage(AlternativeMUDClasses.CMD_AUTH_GET_CHARACTERS, "{}");
	}

	public void OnLoginFailed (string jsonData)
	{
		loginSucceeded = false;
		loginFailed = true;
	}

	public void OnCharactersList (string jsonData)
	{
		var characters = JSON.Parse(jsonData);
		selectedCharacter = characters ["characters"] [0] ["name"];
		Debug.Log ("Selected character: "+selectedCharacter);
		finished = true;
		executeInUpdate.Enqueue (StartMultiplayer);
	}

	private void StartMultiplayer() {
		this.enabled = false;
	}

	public void Update() {
		if (executeInUpdate.Count != 0) {
			executeInUpdate.Dequeue()();
		}
	}
}
