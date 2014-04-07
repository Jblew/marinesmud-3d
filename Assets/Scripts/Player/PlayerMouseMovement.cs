using UnityEngine;
using System.Collections;

public class PlayerMouseMovement : MonoBehaviour {
	public float mouseTurnSpeed = 1f;
	private Vector3 desiredRightHandPosition = Vector3.zero;
	
	private GUILoginPanel guiLoginPanelScript = null;
	
	private Vector3 movementTargetVector = Vector3.zero;
	private Vector3 moveRotation = Vector3.zero;
	private Rigidbody myRigidbody = null;
	private Transform head = null;
	private Transform rightShoulder = null;
	private Transform rightArm = null;
	private Transform rightForearm = null;
	private Transform rightHand = null;
	private Transform pointer1 = null;

	public float shoulderLength = 0f;
	public float armLength = 0f;
	public float forearmLength = 0f;
	public string grabStatus = "Unknown";
	
	void Start () {
		GameObject alternativMUDClient = GameObject.FindWithTag ("AlternativMUDClient");
		if (alternativMUDClient != null) {
			guiLoginPanelScript = alternativMUDClient.GetComponent<GUILoginPanel>();
		} else {
			Debug.LogWarning ("PlayerMovement: Cannot find by tag: #AlternativMUDClient");
		}
		GameObject _pointer1 = GameObject.FindGameObjectWithTag ("Pointer1");
		if (_pointer1 != null) pointer1 = _pointer1.transform;

		head = Util.FindChildWithName(transform, "Head");
		rightShoulder = Util.FindChildWithName(transform, "RightShoulder");
		rightArm = Util.FindChildWithName(transform, "RightArm");
		rightForearm = Util.FindChildWithName(transform, "RightForeArm");
		rightHand = Util.FindChildWithName(transform, "RightHand");
		if(head != null && rightShoulder != null && rightArm != null && rightForearm != null && rightHand != null) {
			Debug.Log ("Got UMA rigs");
		}
		else {
			Debug.Log ("Could not get UMA rigs!");
		}
	}
	
	void Update() {
		moveRotation = new Vector3(0,Input.GetAxis("Mouse X"),0);
		if(guiLoginPanelScript == null) transform.Rotate (moveRotation* mouseTurnSpeed);
		else if(!guiLoginPanelScript.enabled) transform.Rotate (moveRotation* mouseTurnSpeed);
	}

	void LateUpdate() {
		if(pointer1 != null) desiredRightHandPosition = pointer1.position;
		if(head != null && rightShoulder != null && rightArm != null && rightForearm != null && rightHand != null) {
			//neck.eulerAngles = new Vector3(0f, 0f, -90f);
			/***==TOP==**/
			shoulderLength = Vector3.Distance(rightShoulder.position, rightArm.position);
			armLength = Vector3.Distance(rightArm.position, rightForearm.position);
			forearmLength = Vector3.Distance(rightForearm.position, rightHand.position);

			float grabDistance = Vector3.Distance(rightShoulder.position, desiredRightHandPosition);
			if(grabDistance > shoulderLength+armLength+forearmLength) {
				grabStatus = "Too far";

			}
			else {
				grabStatus = "Distance ok";
			}

			//head.localEulerAngles = new Vector3(0f, 0f, 0f);

		}
	}
}
