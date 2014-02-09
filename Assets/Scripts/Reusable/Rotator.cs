using UnityEngine;
using System.Collections;

public class Rotator : MonoBehaviour {
	public float speedX = 0f;
	public float speedY = 0f;
	public float speedZ = 0f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		transform.Rotate (new Vector3(speedX, speedY, speedZ) * Time.deltaTime);
	}
}
