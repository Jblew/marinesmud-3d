using UnityEngine;
using System.Collections;

public class ShakeObject : MonoBehaviour {

	private Vector3 beforeShakePosition = Vector3.zero;
	public float shakeAmount;
	public bool axisX;
	public bool axisY;
	public bool axisZ;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (beforeShakePosition == Vector3.zero) beforeShakePosition = transform.localPosition;
		Vector3 newPosition = beforeShakePosition + Random.insideUnitSphere * shakeAmount;
		transform.localPosition = new Vector3((axisX? newPosition.x : beforeShakePosition.x), (axisY? newPosition.y : beforeShakePosition.y), (axisZ? newPosition.z : beforeShakePosition.z));
	}
}
