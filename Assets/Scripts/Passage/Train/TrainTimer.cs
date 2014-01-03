using UnityEngine;
using System.Collections;

public class TrainTimer : MonoBehaviour {
	public float timeInSeconds;

	private float timer;

	// Use this for initialization
	void Start () {
		timer = timeInSeconds;
	}
	
	// Update is called once per frame
	void Update () {
		timer -= Time.deltaTime;

		if (timer > 0){
			int hours = ((int)Mathf.Floor(timer/3600));
			int minutes = ((int)Mathf.Floor((timer-hours*3600)/60));
			int seconds = ((int)Mathf.Floor (timer-hours*3600-minutes*60));
			GetComponent<TextMesh>().text = "Train in "+hours.ToString("F0")+":"+minutes.ToString("F0")+":"+seconds.ToString("F0");
		} else {
			timer = timeInSeconds;
		}
	}
}
