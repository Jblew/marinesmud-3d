using UnityEngine;
using System.Collections;

public class CharacterLobbyController : MonoBehaviour {
	public float tumble;

	void Awake()
	{
		rigidbody.angularVelocity = Vector3.up * tumble;
	}
}
