using UnityEngine;
using System.Collections;

public class OpenDoors : MonoBehaviour {
	public GUIBigLabelFader labelFader;
	public string nextSceneName;

	private GameObject player;

	void Start() {
		player = GameObject.FindGameObjectWithTag (Tags.PLAYER);
	}

	void OnTriggerEnter(Collider other) {
		if(other.gameObject == player) labelFader.Show ("Press space to enter that room.");
	}

	void OnTriggerExit(Collider other) {
		if (other.gameObject == player) {
			labelFader.Hide ();
		}
	}
}
