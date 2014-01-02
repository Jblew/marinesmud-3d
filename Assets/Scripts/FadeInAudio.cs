using UnityEngine;
using System.Collections;

public class FadeInAudio : MonoBehaviour
{
	public float fade = 0.0f;
	
	
	void Awake()
	{	
		AudioListener.volume = 0;
	}
	
	
	void Update ()
	{
		AudioListener.volume = fade;
	}
}
