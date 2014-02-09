using UnityEditor;  
using UnityEngine;
using System;
using _3VisioShapesClass;
public class _3VisioShapes: EditorWindow{ 
	static _3Visio_Shape _3Visio;
	static GameObject VisioShape;
	bool Star=false;
	bool Gear=false;
	static bool objectSelected=false;
	static float starAmount=0.5f;
	static int sides=3;
	[MenuItem("GameObject/3Visio/Shapes &S", false, 111)]
    public static void InitWindow(){
		_3VisioShapes window =(_3VisioShapes)EditorWindow.GetWindow (typeof (_3VisioShapes));
		window.title = "3Visio Shapes";
		window.position=new Rect(7,140,410,240);
    }
	[MenuItem("GameObject/3Visio/Help &h", false, 111)]
    public static void help(){
		string path="file:///"+Application.dataPath+"/3Visio/Help/help.html";
		Debug.Log(path);
		Application.OpenURL(path);
    }
	void OnGUI () {
		if(!Selection.activeGameObject){
			VisioShape=null;
			_3Visio=null;
			objectSelected=false;
		}else{
			_3Visio=Selection.activeGameObject.GetComponent<_3Visio_Shape>();
			if(_3Visio){
				VisioShape=Selection.activeGameObject;
				sides=_3Visio.sides;
				objectSelected=true;
			}else objectSelected=false;
		}
		if(objectSelected){
			if(Selection.activeGameObject){
				GUILayout.BeginHorizontal();	
					GUILayout.Label("Object Selected: "+Selection.activeGameObject.name);
					GUILayout.Label("ID: "+_3Visio.id);
				GUILayout.EndHorizontal();
				GUILayout.Label("Radius: "+ _3Visio.raggioEsterno.ToString()); 
				GUILayout.Label("Apotheme: "+_3Visio.apotema.ToString()); 
				GUILayout.Label("Side Length: "+_3Visio.sideLength.ToString()); 
				GUILayout.Label("Perimeter: "+_3Visio.perimeter.ToString()); 
				GUILayout.Label("Area: "+_3Visio.area.ToString()); 
				GUILayout.Label(_3Visio.Builded);
				GUILayout.Label("Comments: ");
				_3Visio.Comments = GUILayout.TextArea(_3Visio.Comments, 200);
				if(GUI.changed){
					return;
				}
				sides=EditorGUILayout.IntSlider("Sides: ", sides, 3,40);
				if(GUI.changed){
					Undo.SetSnapshotTarget(_3Visio, "Sides"); 
					Undo.CreateSnapshot();
					Undo.RegisterSnapshot();
					_3Visio.sides=sides;
					Modify();
					return;
				}
				Star=_3Visio.Star;
				Star = EditorGUILayout.Toggle ("Star: ", Star);
				if(GUI.changed){
					_3Visio.Star=Star;
					if(Star){
						Gear=false;
						_3Visio.Gear=Gear;
					}
					GUI.changed=false;
					Modify();
					return;
				}
				if(Star){
					if(starAmount!=_3Visio.starAmount){
						starAmount=_3Visio.starAmount;
					}
					starAmount=EditorGUILayout.Slider("Star Length: ", starAmount, 0.1f,0.99f); 			
					if(GUI.changed){
						Undo.SetSnapshotTarget(_3Visio, "Star Length"); 
						Undo.CreateSnapshot();
						Undo.RegisterSnapshot();
						_3Visio.starAmount=starAmount;
						Modify();
						return;
					}
				}
				Gear=_3Visio.Gear;
				Gear = EditorGUILayout.Toggle ("Gear: ", Gear);
				if(GUI.changed){
					_3Visio.Gear=Gear;
					if(Gear){
						Star=false;
						_3Visio.Star=Star;
					}
					GUI.changed=false;
					Modify();
					return;
				}
				if(Gear){
					if(starAmount!=_3Visio.starAmount){
						starAmount=_3Visio.starAmount;
					}
					starAmount=EditorGUILayout.Slider("Gear Length: ", starAmount, 0.1f,0.99f);
					if(GUI.changed){
						Undo.SetSnapshotTarget(_3Visio, "Gear Length"); 
						Undo.CreateSnapshot();
						Undo.RegisterSnapshot();
						_3Visio.starAmount=starAmount;
						Modify();
						return;
					}
				}
			}
		}else{
			if(GUILayout.Button("Build a new 3Visio Shape")){
				GUIBuildPlane();
			}
		}
	}
	void Update(){
		Repaint();
	}
	void GUIBuildPlane(){
		VisioShape=new GameObject("3Visio Shape");
		string id=VisioShape.GetInstanceID().ToString();
		_3Visio=VisioShape.AddComponent<_3Visio_Shape>();
		_3Visio.id=id;
		_3Visio.parent=VisioShape;
		_3Visio.hideFlags=HideFlags.HideInInspector;
		_3Visio.Builded=Display(DateTime.Now, "Creation date: ");
		VisioShape.AddComponent<MeshFilter>();
		MeshRenderer meshRenderer=VisioShape.AddComponent<MeshRenderer>();
		meshRenderer.renderer.sharedMaterial=new Material(Shader.Find("Diffuse"));
		Modify();
	}
	static string Display(DateTime inputDt, string prefix){
		return _3VisioShape.Display(inputDt, prefix);
    }
	void Modify(){
		Mesh mesh=VisioShape.GetComponent<MeshFilter>().sharedMesh;
		if(!mesh){
			mesh=new Mesh();
			VisioShape.GetComponent<MeshFilter>().sharedMesh=mesh;
		}
		_3VisioShape.build(sides, ref _3Visio.raggioEsterno, ref _3Visio.apotema, ref _3Visio.sideLength, ref _3Visio.area, ref _3Visio.perimeter, Star, Gear, starAmount, ref mesh);
	}
}
