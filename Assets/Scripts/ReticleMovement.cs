using UnityEngine;
using System.Collections;

public class ReticleMovement : MonoBehaviour
{
	void Update ()
	{
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit[] hits;
		
		hits = Physics.RaycastAll(ray);
		
		foreach(RaycastHit h in hits)
		{
			if(h.collider.name == "ground")
				transform.position = new Vector3(h.point.x, 5f, h.point.z);
		}
	}
}
