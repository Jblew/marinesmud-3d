using UnityEngine;
using System.Collections;

public class DontDestroyObject : MonoBehaviour {
	// Use this for initialization
	void Start () {
		DontDestroyOnLoad (gameObject);
	}
}
