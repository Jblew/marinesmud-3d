using UnityEngine;
using System.Collections;
using UMA;
using LitJson;

public class UmaLoad : MonoBehaviour {
	public string text;
	public bool load = false;
	public UMAContext context;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (load) {
			UMAPackedRecipeBase.UMAPackRecipe loadedRecipe = LitJson.JsonMapper.ToObject<UMAPackedRecipeBase.UMAPackRecipe>(text);

			var newGO = new GameObject("Generated Character");
			newGO.transform.parent = transform;
			UMADynamicAvatar umaDynamicAvatar = newGO.AddComponent<UMADynamicAvatar>();
			umaDynamicAvatar.Initialize();
			UMAData umaData = umaDynamicAvatar.umaData;
			UMAPackedRecipeBase.UnpackRecipe(umaData, loadedRecipe, context);
			umaDynamicAvatar.UpdateNewRace();
			umaDynamicAvatar.umaData.myRenderer.enabled = false;

			load = false;
		}
	}

	public static UMADynamicAvatar Load(string jsonData, UMAContext umaContext, Transform desiredTransform) {
		UMAPackedRecipeBase.UMAPackRecipe loadedRecipe = LitJson.JsonMapper.ToObject<UMAPackedRecipeBase.UMAPackRecipe>(jsonData);
		
		var newGO = new GameObject("Generated Character");
		newGO.transform.parent = desiredTransform;
		UMADynamicAvatar umaDynamicAvatar = newGO.AddComponent<UMADynamicAvatar>();
		umaDynamicAvatar.Initialize();
		UMAData umaData = umaDynamicAvatar.umaData;
		UMAPackedRecipeBase.UnpackRecipe(umaData, loadedRecipe, umaContext);
		umaDynamicAvatar.UpdateNewRace();
		umaDynamicAvatar.umaData.myRenderer.enabled = false;
		//umaDynamicAvatar.tag = "Player";

		return umaDynamicAvatar;
	}

}
