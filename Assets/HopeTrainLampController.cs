using UnityEngine;
using System.Collections;

public class HopeTrainLampController : MonoBehaviour {

	public GameObject offZoneTrigger;

	// Use this for initialization
	void Start () {
		//GetComponent<Light> ().enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider other) {
		if (other.gameObject == offZoneTrigger) {
			GetComponent<Light> ().enabled = false;
			//gameObject.transform.parent.gameObject.renderer.enabled = false;
		}
	}

	void OnTriggerExit(Collider other) {
		if (other.gameObject == offZoneTrigger) {
			GetComponent<Light> ().enabled = true;
			//gameObject.transform.parent.gameObject.renderer.enabled = true;
		}
	}
}
