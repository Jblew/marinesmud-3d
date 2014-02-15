using UnityEngine;
using System.Collections;
using System;

public class TrainController : MonoBehaviour {
	public int awayTime = 200;
	public int stayTime = 50;
	public int leavingTime = 50;
	public float awayX;
	public float transitionSpeed;
	
	private Vector3 targetPosition;
	private Vector3 awayPosition;
	private Vector3 leavingPosition;
	private Vector3 stayPosition;
	public int timer;

	/*Train: LEAVING <-- STAY <-- AWAY*/
	void Start () {
		awayPosition = new Vector3 (transform.position.x + awayX, transform.position.y, transform.position.z);
		leavingPosition = new Vector3 (transform.position.x - awayX, transform.position.y, transform.position.z);
		stayPosition = transform.position;
		targetPosition = leavingPosition;
		transform.position = awayPosition;
	}
	
	// Update is called once per frame
	void Update () {
		int cycleTime = awayTime + stayTime + leavingTime;
		timer = (int)(((TimeManager.getCurrentTicks () / TimeSpan.TicksPerSecond)-(long)leavingTime) % (long)cycleTime);//add leaving time, because train should leave when timer on display is zero

		Vector3 lastTargetPosition = targetPosition;
		if (timer < awayTime) {
			targetPosition = awayPosition;
		} else if(timer < awayTime + stayTime) {
			targetPosition = stayPosition;
		} else {
			targetPosition = leavingPosition;
		}
		if (lastTargetPosition == leavingPosition && targetPosition == awayPosition) {
			transform.position = awayPosition;
		}
		else {
			transform.position = Vector3.Lerp (transform.position, targetPosition, transitionSpeed * Time.deltaTime);
		}
	}
}
