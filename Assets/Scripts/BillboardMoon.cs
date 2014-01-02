using UnityEngine;
using System.Collections;

public class BillboardMoon : MonoBehaviour
{
	public GameObject cam;
	
	
	void Update () 
	{
		transform.LookAt(cam.transform);
	}
}
