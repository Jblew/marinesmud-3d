using UnityEngine;
using System.Collections;

public class RotateCameraByMouseY : MonoBehaviour {
	public float mouseTurnSpeed;

	private GUILoginPanel guiLoginPanelScript = null;
	private Vector3 moveRotation;

	void Start() {
		GameObject alternativMUDClient = GameObject.FindWithTag ("AlternativMUDClient");
		if (alternativMUDClient != null) {
			guiLoginPanelScript = alternativMUDClient.GetComponent<GUILoginPanel>();
		} else {
			Debug.LogWarning ("RotateCameraByMouseY: Cannot find by tag: #AlternativMUDClient");
		}
	}

	// Update is called once per frame
	void Update () {
		moveRotation = new Vector3(-Input.GetAxis("Mouse Y"), 0f, 0f);
		if (guiLoginPanelScript == null) {
			transform.Rotate (moveRotation* mouseTurnSpeed);
		}
		else if(!guiLoginPanelScript.enabled) transform.Rotate (moveRotation* mouseTurnSpeed);
	}
}
