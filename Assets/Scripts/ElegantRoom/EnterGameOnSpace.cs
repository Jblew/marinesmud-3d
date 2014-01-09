using UnityEngine;
using System.Collections;

public class EnterGameOnSpace : MonoBehaviour {
	void Update () {
		if (Input.GetButton ("Jump")) {
			gameObject.guiText.text = "Invisible strength carries you...";
			Application.LoadLevel("Passage");
		}
	}
}
