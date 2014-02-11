using UnityEngine;
using System.Collections;

public class DontDestroyObject : MonoBehaviour {
	// Use this for initialization
	void Start () {
		//DontDestroyOnLoad (gameObject);
		disableDestroy (gameObject);
	}

	void disableDestroy(GameObject gameObject) {
		DontDestroyOnLoad (gameObject);
		for(int i = 0;i < gameObject.transform.childCount;i++) {
			disableDestroy (gameObject.transform.GetChild(i).gameObject);
		}
	}
}
