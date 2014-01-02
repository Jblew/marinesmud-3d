using UnityEngine;
using System.Collections;

public class BarrelLid : MonoBehaviour
{
	public float dropletForce;
	
	public Rigidbody lid;
	private ParticleSystem[] sprinklers;
	private ParticleSystem.CollisionEvent[][] collisionEvents;	//arr[numPartSys][colEveOfSys]
	private int safeCollisionEventSize;
	
	
	void Awake ()
	{
		sprinklers = GetComponentsInChildren<ParticleSystem>();
	}
	
	
	void Start ()
	{
		collisionEvents = new ParticleSystem.CollisionEvent[sprinklers.Length][];
	}
	
	
	void OnParticleCollision(GameObject other)
	{
		if(other.tag == "Barrel")
		{
			
			for(int i = 0; i < collisionEvents.Length; i++)
			{
				collisionEvents[i] = new ParticleSystem.CollisionEvent[sprinklers[i].safeCollisionEventSize];
			}
			
			for(int i = 0; i < collisionEvents.Length; i++)
			{
				sprinklers[i].GetCollisionEvents(gameObject, collisionEvents[i]);
			}
			
			for(int i = 0; i < collisionEvents.Length; i++)
			{
				for(int j = 0; j < collisionEvents[i].Length; j++)
				{
					lid.AddForceAtPosition(Vector3.down * dropletForce, collisionEvents[i][j].intersection);
				}
			}
		}
	}
}
