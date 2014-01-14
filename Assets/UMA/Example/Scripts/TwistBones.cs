﻿using UnityEngine;
using System.Collections;

public class TwistBones : MonoBehaviour {
	public float twistValue;
	
	public Transform[] twistBone;
	public Transform[] refBone;
	
	private float[] originalRefRotation;
	private Vector3 rotated;
	
	// Use this for initialization
	void Awake () {
		originalRefRotation = new float[twistBone.Length];
		for(int i = 0; i < twistBone.Length; i++){
			rotated = refBone[i].localRotation * Vector3.up;
			originalRefRotation[i] = Mathf.Atan2(rotated.z, rotated.y) * Mathf.Rad2Deg;
		}
	}
	
	// Update is called once per frame
	void LateUpdate () {
		for(int i = 0; i < twistBone.Length; i++){
			rotated = refBone[i].localRotation * Vector3.up;
			twistBone[i].localEulerAngles = Vector3.zero;
			twistBone[i].Rotate(Vector3.right * twistValue * Mathf.DeltaAngle(originalRefRotation[i], Mathf.Atan2(rotated.z, rotated.y) * Mathf.Rad2Deg));
		}
	}
}