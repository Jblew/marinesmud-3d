using UnityEngine;
using System.Collections;

public class TrainController : MonoBehaviour {
	public float awayTime = 250;
	public float stayTime = 50;
	public float awayX;
	public float changeMargin;
	public float transitionSpeed;

	private float timer;
	private float timerTarget;

	private Vector3 targetPosition;
	private Vector3 lowPosition;
	private Vector3 highPosition;
	private Vector3 zeroPosition;
	private int state = 3;//0 = arriving, 1 = staying, 2 = leaving, 3 = waiting

	void Start () {
		timer = awayTime;
		lowPosition = new Vector3 (transform.position.x + awayX, transform.position.y, transform.position.z);
		highPosition = new Vector3 (transform.position.x - awayX, transform.position.y, transform.position.z);
		zeroPosition = transform.position;
		targetPosition = highPosition;
		timerTarget = awayTime;
	}
	
	// Update is called once per frame
	void Update () {
		timer -= Time.deltaTime;

		transform.position = Vector3.Lerp (transform.position, targetPosition, transitionSpeed * Time.deltaTime);
		//else transform.position += (targetPosition-zeroPosition)/15 * Time.deltaTime; //Vector3.Lerp (transform.position, targetPosition, transitionSpeed * Time.deltaTime);
		CheckTargetIntensity ();

		if (timer <= 0) {
			if(timerTarget == awayTime) {
				timerTarget = stayTime;
			}
			else {
				timerTarget = awayTime;
			}
			timer = timerTarget;
		}
	}

	void CheckTargetIntensity()
	{
		if (Mathf.Abs (Vector3.Distance(targetPosition, transform.position)) < changeMargin) 
		{
			if(targetPosition == highPosition && timer <= 0) {
				transform.position = lowPosition;
				targetPosition = zeroPosition;
			}
			else if(targetPosition == zeroPosition && timer <= 0) {
				targetPosition = highPosition;
			}
			else if(timer <= 0) {
				targetPosition = zeroPosition;
			}
		}
	}
}
