using UnityEngine;
using System.Collections;

public class PlayerMouseMovement : MonoBehaviour {
	public float mouseTurnSpeed = 1f;
	
	private GUILoginPanel guiLoginPanelScript = null;
	
	private Vector3 movementTargetVector = Vector3.zero;
	private Vector3 moveRotation = Vector3.zero;
	private Rigidbody myRigidbody = null;
	
	void Start () {
		GameObject alternativMUDClient = GameObject.FindWithTag ("AlternativMUDClient");
		if (alternativMUDClient != null) {
			guiLoginPanelScript = alternativMUDClient.GetComponent<GUILoginPanel>();
		} else {
			Debug.LogWarning ("PlayerMovement: Cannot find by tag: #AlternativMUDClient");
		}
		
		/*for(int i = 0;i<4; i++) {
			aniBody.clip = anim.idle;
			aniBody.Play ();
		}*/
	}
	
	void Update() {
		//aniBody.CrossFade (currentAnimation.name, 0.1f);
		
		moveRotation = new Vector3(0,Input.GetAxis("Mouse X"),0);
		if(guiLoginPanelScript == null) transform.Rotate (moveRotation* mouseTurnSpeed);
		else if(!guiLoginPanelScript.enabled) transform.Rotate (moveRotation* mouseTurnSpeed);
	}
}
