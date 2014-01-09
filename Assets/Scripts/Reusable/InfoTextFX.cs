using UnityEngine;
using System.Collections;

public class InfoTextFX : MonoBehaviour {
	public float transitionSpeed = 2f;
	public Vector3 deltaPosition;
	public Vector3 offPosition;
	public float changeMargin;
	public bool fxOn;

	private Vector3 targetPosition;
	private Vector3 lowPosition;
	private Vector3 highPosition;

	void Awake()
	{
		lowPosition = transform.position;
		highPosition = lowPosition + deltaPosition;
		targetPosition = highPosition;
	}
	
	void Update()
	{
		if (fxOn) {
			transform.position = Vector3.Lerp (transform.position, targetPosition, transitionSpeed * Time.deltaTime);
			CheckTargetIntensity ();
		} else {
			transform.position = Vector3.Lerp(transform.position, offPosition, transitionSpeed * Time.deltaTime);
		}
	}
	
	void CheckTargetIntensity()
	{
		if (Mathf.Abs (Vector3.Distance(targetPosition, transform.position)) < changeMargin) 
		{
			if(targetPosition == highPosition) targetPosition = lowPosition;
			else targetPosition = highPosition;
		}
	}
}
