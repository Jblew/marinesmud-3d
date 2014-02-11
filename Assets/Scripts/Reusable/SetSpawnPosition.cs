using UnityEngine;
using System.Collections;

public class SetSpawnPosition : MonoBehaviour {
	public Vector3 position;
	public Vector3 rotation;

	// Use this for initialization
	void Start () {
		GameObject player = GameObject.FindWithTag ("Player");
		if (player == null) {
			Debug.LogWarning ("Could nt find #Player to set spawn position");
		}
		else {
			player.transform.position = position;
			player.transform.eulerAngles = rotation;
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
