using UnityEngine;
using System.Collections;

public class Util : MonoBehaviour {

	public static Transform FindChildWithName(Transform t, string name) {
		if (t.name == name) {
			return t;
		}
		else {
			for(int i = 0;i < t.childCount;i++) {
				Transform child = t.GetChild(i);
				if(child.name == name) return t;
				else {
					Transform probablyResult = FindChildWithName(child, name);
					if(probablyResult != null) {
						return probablyResult;
					}
				}
			}
			return null;
		}
	}
}
