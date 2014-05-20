using UnityEngine;
using System.Collections;

public class TriangleController {
	
}

public interface Bone {
	Vector3 getStartPoint( );
	Vector3 getEulerAngles( );
	float getLength();
}

public class RealBone : Bone {
	private Transform t;
	private float length;

	public RealBone(Transform t_, float length_) {
		t = t_;
		length = length_;
	}

	public Transform getBoneTransform() {
		return t;
	}

	public Vector3 getStartPoint() {
		return t.position;
	}

	public Vector3 getEulerAngles() {
		return t.eulerAngles;
	}

	public float getLength() {
		return length;
	}
}

public interface VirtualBone : Bone {

}

public class Triangle : VirtualBone {
	private Transform t1;
	private Transform t2;
	private float length1;
	private float length2;

	public Vector3 getStartPoint() {
		return t1.position;
	}

	public Vector3 getEndPoint() {
		return Vector3.zero; //calculate
	}
	
	public Vector3 getEulerAngles() {
		return Vector3.zero;//calculate
	}
	
	public float getLength() {
		return 0f;//calculate
	}
}

