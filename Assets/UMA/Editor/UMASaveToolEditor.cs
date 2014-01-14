using UnityEngine;
using UnityEditor;
using System.Collections;
using UMA;


[CustomEditor(typeof(UMASaveTool))]
[CanEditMultipleObjects]
public class UMASaveToolEditor : Editor {
	
	public SerializedProperty avatarName;
	public SerializedProperty serializedAvatar;

	
    void OnEnable () {
        avatarName = serializedObject.FindProperty ("avatarName");
    }
	
	
	public override void OnInspectorGUI(){	
		serializedObject.Update();
		
		GUILayout.Label ("Avatar Name", EditorStyles.boldLabel);
		avatarName.stringValue = EditorGUILayout.TextArea(avatarName.stringValue);   
		
		GUILayout.Space(20);
		
		GUILayout.BeginHorizontal();
		if(GUILayout.Button("Save Avatar Txt")){
			UMASaveTool umaSaveTool = (UMASaveTool)target;    
			GameObject gameObject = (GameObject)umaSaveTool.gameObject;
			UMADynamicAvatar umaDynamicAvatar = gameObject.GetComponent("UMADynamicAvatar") as UMADynamicAvatar;

			if(umaDynamicAvatar){
				var path = EditorUtility.SaveFilePanel("Save serialized Avatar", "Assets", avatarName.stringValue + ".txt", "txt");
				if(path.Length != 0) 
				{
					var asset = ScriptableObject.CreateInstance<UMATextRecipe>();
					asset.Save(umaDynamicAvatar.umaData, umaDynamicAvatar.context);
					System.IO.File.WriteAllText(path, asset.recipeString);
					ScriptableObject.Destroy(asset);
				}
			}
		}

		if(GUILayout.Button("Save Avatar Asset")){
			UMASaveTool umaSaveTool = (UMASaveTool)target;    
			GameObject gameObject = (GameObject)umaSaveTool.gameObject;
			UMADynamicAvatar umaDynamicAvatar = gameObject.GetComponent("UMADynamicAvatar") as UMADynamicAvatar;

			if(umaDynamicAvatar){
				var path = EditorUtility.SaveFilePanelInProject("Save serialized Avatar", avatarName.stringValue + ".asset", "asset", "Message 2");
				if(path.Length != 0) 
				{
					var asset = ScriptableObject.CreateInstance<UMATextRecipe>();
					asset.Save(umaDynamicAvatar.umaData, umaDynamicAvatar.context);
					AssetDatabase.CreateAsset(asset, path);
					AssetDatabase.SaveAssets();
				}
			}
		}
		
		GUILayout.EndHorizontal();
		
		GUILayout.Space(20);

		
		serializedObject.ApplyModifiedProperties();
	}
	
}
