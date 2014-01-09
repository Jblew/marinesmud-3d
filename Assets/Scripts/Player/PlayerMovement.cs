using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {
	[System.Serializable]
	public class  Anim {
		public AnimationClip walk;
		public AnimationClip stand_Lshot;
		public AnimationClip stand_Rshot;
		public AnimationClip seat_Lshot;
		public AnimationClip seat_Fshot;
		public AnimationClip stand_Fshot;
		public AnimationClip idle;
		public AnimationClip hit;
		public AnimationClip death;
		public AnimationClip run;
	}
	
	
	public Anim anim;
	public Animation  aniBody;

	public AudioClip shoutingClip;
	public float turnSmoothing = 15f;
	public float speedDampTime = 0.1f;

	public float mouseTurnSpeed;
	public float walkSpeed;

	public GUILoginPanel guiLoginPanelScript;

	private Vector3 movementTargetVector = Vector3.zero;
	private Vector3 moveRotation = Vector3.zero;

	private HashIDs hash;
	private AnimationClip currentAnimation;
	
	void Awake()
	{
		hash = GameObject.FindGameObjectWithTag (Tags.GAME_CONTROLLER).GetComponent<HashIDs> ();
		currentAnimation = anim.walk;
	}
	
	void FixedUpdate()
	{
		float h = Input.GetAxis ("Horizontal");
		float v = Input.GetAxis ("Vertical");
		bool sneak = Input.GetButton ("Sneak");
		if(!guiLoginPanelScript.enabled) MovementManagement (h, v, sneak);

	}

	void Start () {
		for(int i = 0;i<4; i++) {
			aniBody.clip = anim.idle;
			aniBody.Play ();
		}
	}
	
	void Update() {
		aniBody.CrossFade (currentAnimation.name, 0.1f);

		moveRotation = new Vector3(0,Input.GetAxis("Mouse X"),0);
		if(!guiLoginPanelScript.enabled) transform.Rotate (moveRotation* mouseTurnSpeed);
	}
	
	void MovementManagement(float horizontal, float vertical, bool sneaking) {
		if (horizontal != 0f || vertical != 0f) {
			rigidbody.velocity = transform.rotation * Vector3.forward * mouseTurnSpeed * walkSpeed;
			currentAnimation = anim.walk;
		}
		else {
			rigidbody.velocity = Vector3.zero;
			currentAnimation = anim.idle;
			movementTargetVector = Vector3.zero;
		}
	}
	
	void AudioManagement(bool shout)
	{
		if (!audio.isPlaying) {
			audio.Play ();
		}
		
		if (shout) {
			AudioSource.PlayClipAtPoint(shoutingClip, transform.position);
		}
	}

	public Vector3 GetMovementTargetVector() {
		return movementTargetVector;
	}
}
