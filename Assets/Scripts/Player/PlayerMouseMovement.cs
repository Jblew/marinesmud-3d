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
	private SphereCollider greenCollider = null;
	private SphereCollider shoulderCollider = null;
	private SphereCollider armCollider = null;
	private SphereCollider forearmCollider = null;

	//public float shoulderLength = 0f;
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
			Debug.Log ("Got UMA rigs: rightShoulder="+rightShoulder.name+", rightArm="+rightArm.name+", rightForearm="+rightForearm.name+", rightHand="+rightHand.name);
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
			float shoulderLength = Vector3.Distance(rightShoulder.position, rightArm.position);
			armLength = Vector3.Distance(rightArm.position, rightForearm.position);
			forearmLength = Vector3.Distance(rightForearm.position, rightHand.position);

			float grabDistance = Vector3.Distance(rightArm.position, desiredRightHandPosition);
			if(grabDistance > armLength+forearmLength) {
				grabStatus = "Too far";

			}
			else {
				grabStatus = "Distance ok";

			}

			if(shoulderCollider == null) {
				shoulderCollider = rightShoulder.gameObject.AddComponent<SphereCollider>();
				shoulderCollider.isTrigger = true;
			}
			if(shoulderCollider != null) {
				//shoulderCollider.center = rightShoulder.position-transform.position;
				shoulderCollider.radius = shoulderLength;
			}

			if(armCollider == null) {
				armCollider = rightArm.gameObject.AddComponent<SphereCollider>();
				armCollider.isTrigger = true;
			}
			if(armCollider != null) {
				//armCollider.center = rightArm.position-transform.position;
				armCollider.radius = armLength;
			}

			if(forearmCollider == null) {
				forearmCollider = rightForearm.gameObject.AddComponent<SphereCollider>();
				forearmCollider.isTrigger = true;
			}
			if(forearmCollider != null) {
				//forearmCollider.center = rightForearm.position-transform.position;
				forearmCollider.radius = forearmLength;
			}

			//head.localEulerAngles = new Vector3(0f, 0f, 0f);

		}
	}
}
