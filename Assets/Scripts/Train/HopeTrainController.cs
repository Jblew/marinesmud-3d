using UnityEngine;
using System.Collections;

public class HopeTrainController : MonoBehaviour {

	private GameObject player = null;
	private bool holdPlayer = true;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (player == null) {
			player = GameObject.FindGameObjectWithTag ("Player");
		}

		transform.Translate (new Vector3(0f, 0f, 20f*Time.deltaTime));
		if(holdPlayer) player.transform.Translate (new Vector3(0f, 0f, 20f*Time.deltaTime));
		//rigidbody.AddForce (new Vector3(1f, 1f, 100000f*Time.deltaTime));
	}

	void OnTriggerExit(Collider other) {
		if (other.gameObject == player) {
			holdPlayer = false;
		}
	}

	void OnTriggerEnter(Collider other) {
		if (other.gameObject == player) {
			holdPlayer = true;
		}
	}
}
