using UnityEngine;
using System.Collections;

public class FireAudio : MonoBehaviour
{
	void Update ()
	{
		audio.volume = particleSystem.startColor.a * 2f;
	}
}
