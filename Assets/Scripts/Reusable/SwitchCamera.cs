using UnityEngine;
using System.Collections;

public class SwitchCamera : MonoBehaviour {
	public Camera secondaryCamera;

	private GameObject player;
	private Camera mainCamera;

	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag (Tags.PLAYER);
	}
	
	void OnTriggerEnter(Collider other) {
		if (other.gameObject == player) {
			mainCamera = Camera.main;
			Camera.main.enabled = false;
			secondaryCamera.enabled = true;
		}
	}
	
	void OnTriggerExit(Collider other) {
		if (other.gameObject == player) {
			mainCamera.enabled = true;
			secondaryCamera.enabled = false;
		}
	}
}
