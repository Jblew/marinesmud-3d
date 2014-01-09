using UnityEngine;
using System.Collections;

public class HopeTrainController : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		transform.Translate (new Vector3(0f, 0f, 10f*Time.deltaTime));
	}
}
