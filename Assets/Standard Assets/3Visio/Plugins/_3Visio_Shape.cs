/*
Warning: Do not delete nor modify this script, possible tampering could affect the operation of 3Visio.
If you want to make permanent the properties of the meshes created with 3Visio, simply remove this component in the inspector.
*/
using UnityEngine;
using System;
[Serializable]
public class _3Visio_Shape : MonoBehaviour {
	public int sides=3;
	public string id="";
	public bool Star=false;
	public bool Gear=false;
	public float raggioEsterno=0.5f;
	public float apotema=0.5f;
	public float sideLength=0;
	public float perimeter=0;
	public float area=0;
	public string Builded="";
	public string Comments="";
	public float starAmount=0.5f;
	public GameObject parent=null;
}
