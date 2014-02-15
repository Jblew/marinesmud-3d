using UnityEngine;
using System.Collections;
using System;

public class TrainTimer : MonoBehaviour {
	public int timeInSeconds = 300;

	//private float timer;

	// Use this for initialization
	void Start () {
		//timer = timeInSeconds;
		//TimeManager.AddListener (TimeHasChanged);
	}

	private long i = 0;
	void Update () {
		//timer -= Time.deltaTime;

		//if (timer > 0){
		if (i % 15 == 0) {
			int timer = timeInSeconds - (int)((TimeManager.getCurrentTicks () / TimeSpan.TicksPerSecond) % (long)timeInSeconds);
			int hours = ((int)Mathf.Floor (timer / 3600));
			int minutes = ((int)Mathf.Floor ((timer - hours * 3600) / 60));
			int seconds = ((int)Mathf.Floor (timer - hours * 3600 - minutes * 60));
			GetComponent<TextMesh> ().text = "Train in " + hours.ToString ("F0") + ":" + minutes.ToString ("F0") + ":" + seconds.ToString ("F0");
		}
		i++;
		//} else {
		//	timer = timeInSeconds;
		//}
	}

	//void TimeHasChanged(long timeTicks) {
	//	timer = timeTicks / TimeSpan.TicksPerSecond;
	//}
}
