using UnityEngine;
using System.Collections;

public class PlayerMouseMovement : MonoBehaviour {
	public float mouseTurnSpeed = 1f;
	public Transform desiredRightHandPosition = null;
	
	private GUILoginPanel guiLoginPanelScript = null;
	
	private Vector3 movementTargetVector = Vector3.zero;
	private Vector3 moveRotation = Vector3.zero;
	private Rigidbody myRigidbody = null;
	private Transform head = null;
	private Transform rightArm = null;
	private Transform rightForearm = null;
	private Transform rightHand = null;
	
	void Start () {
		GameObject alternativMUDClient = GameObject.FindWithTag ("AlternativMUDClient");
		if (alternativMUDClient != null) {
			guiLoginPanelScript = alternativMUDClient.GetComponent<GUILoginPanel>();
		} else {
			Debug.LogWarning ("PlayerMovement: Cannot find by tag: #AlternativMUDClient");
		}

		head = Util.FindChildWithName(transform, "Head");
		rightArm = Util.FindChildWithName(transform, "RightArm");
		rightForearm = Util.FindChildWithName(transform, "RightForeArm");
		rightHand = Util.FindChildWithName(transform, "RightHand");
		if(head != null && rightArm && rightForearm != null && rightHand != null) {
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
		if(head != null && rightArm && rightForearm != null && rightHand != null) {
			//neck.eulerAngles = new Vector3(0f, 0f, -90f);
			/***==TOP==**/
			float distanceFromArmToDesiredPosition = Vector3.Distance(rightArm.position, desiredRightHandPosition.position);
			head.localEulerAngles = new Vector3(0f, 0f, 0f);
		}
	}
}
