using UnityEngine;
using System.Collections;

public class RotateCameraByMouseY : MonoBehaviour {
	public float mouseTurnSpeed;
	public GUILoginPanel guiLoginPanelScript;

	private Vector3 moveRotation;
	
	// Update is called once per frame
	void Update () {
		moveRotation = new Vector3(-Input.GetAxis("Mouse Y"), 0f, 0f);
		if(!guiLoginPanelScript.enabled) transform.Rotate (moveRotation* mouseTurnSpeed);
	}
}
