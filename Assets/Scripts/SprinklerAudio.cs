using UnityEngine;
using System.Collections;

public class SprinklerAudio : MonoBehaviour
{
	public float fadeSpeed;
	public AudioSource audioStart;
	public AudioSource audioLoop;
	
	
	void Update ()
	{
		if(Input.GetMouseButtonDown(0))
		{
			audioStart.Play();
			StopCoroutine("FadeOut");
			audioLoop.volume = 1f;
			audioLoop.PlayDelayed(audioStart.clip.length);
		}
		if(Input.GetMouseButtonUp(0))
		{
			audioStart.Stop();
			StartCoroutine("FadeOut");
		}
	}
	
	
	IEnumerator FadeOut ()
	{
		while(audioLoop.volume > 0.0f)
		{
			audioLoop.volume -= fadeSpeed * Time.deltaTime;
			yield return null;
		}
	}
}
