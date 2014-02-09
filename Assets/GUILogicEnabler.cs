using UnityEngine;
using System.Collections;

public class GUILogicEnabler : MonoBehaviour {
	public GameObject secondaryGameLogic;
	
	void Start () {
		GameObject alternativMUDClient = GameObject.FindWithTag ("AlternativMUDClient");
		if(alternativMUDClient == null) {
			secondaryGameLogic.SetActive(true);
		}
	}
}
