using UnityEngine;
using System.Collections;

public class VariableOrientedLamp : MonoBehaviour {
	private long i = 0;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		if(i % 10 == 0) {
			VariablesManager mgr = GameObject.FindWithTag("AlternativMUDClient").GetComponent<VariablesManager>();
			if(mgr != null) {
				if(mgr.variables.ContainsKey("global.power") && mgr.variables["global.power"] == "1") {
					GetComponent<Light>().enabled = true;
				}
				else {
					GetComponent<Light>().enabled = false;
				}
			}
		}
		i++;
	}
}
