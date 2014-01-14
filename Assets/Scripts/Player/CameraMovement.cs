using UnityEngine;
using System.Collections;

public class CameraMovement : MonoBehaviour {
	public float smooth = 0.5f;

	private GameObject player = null;
	private Vector3 relCameraPos;
	private float relCameraPosMag;
	private Vector3 newPos;

	void Update() {
		if (player == null) {
			player = GameObject.FindGameObjectWithTag (Tags.PLAYER);
			relCameraPos = transform.position - player.transform.position;
			relCameraPosMag = relCameraPos.magnitude - 0.5f;
		}
	}

	void FixedUpdate()
	{
		if (player != null) {
						Vector3 standardPos = player.transform.position + relCameraPos;

						Vector3 abovePos = player.transform.position + Vector3.up * relCameraPosMag;
						Vector3[] checkPoints = new Vector3[5];
						checkPoints [0] = standardPos;
						checkPoints [1] = Vector3.Lerp (standardPos, abovePos, 0.25f);
						checkPoints [2] = Vector3.Lerp (standardPos, abovePos, 0.5f);
						checkPoints [3] = Vector3.Lerp (standardPos, abovePos, 0.75f);
						checkPoints [1] = abovePos;

						for (int i = 0; i < checkPoints.Length; i++) {
								if (ViewingPosCheck (checkPoints [i])) {
										break;
								}
						}

						transform.position = Vector3.Lerp (transform.position, newPos, smooth * Time.deltaTime);
		}
	}

	bool ViewingPosCheck(Vector3 checkPos)
	{
		RaycastHit hit;

		if (Physics.Raycast (checkPos, (player.transform.position - checkPos), out hit, relCameraPosMag))
		{
			if(hit.transform != player)
			{
				return false;
			}
		}

		newPos = checkPos;
		return true;
	}
}
