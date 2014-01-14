using UnityEngine;
using System.Collections;
using UMA;
using SimpleJSON;

public class UmaSave : MonoBehaviour {

	public UMADynamicAvatar dynamicAvatar;
	public bool saveInstance;
	public string text;
	//public string hmm = //"{"height":128,"headSize":128,"headWidth":128,"neckThickness":128,"armLength":128,"forearmLength":128,"armWidth":128,"forearmWidth":128,"handsSize":128,"feetSize":128,"legSeparation":128,"upperMuscle":128,"lowerMuscle":128,"upperWeight":128,"lowerWeight":128,"legsSize":128,"belly":128,"waist":128,"gluteusSize":128,"earsSize":128,"earsPosition":128,"earsRotation":128,"noseSize":128,"noseCurve":128,"noseWidth":128,"noseInclination":128,"nosePosition":128,"nosePronounced":128,"noseFlatten":128,"chinSize":128,"chinPronounced":128,"chinPosition":128,"mandibleSize":128,"jawsSize":128,"jawsPosition":128,"cheekSize":128,"cheekPosition":128,"lowCheekPronounced":128,"lowCheekPosition":128,"foreheadSize":128,"foreheadPosition":128,"lipsSize":128,"mouthSize":128,"eyeRotation":128,"eyeSize":128,"breastSize":128}";

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		if (saveInstance) {
			//string dnaJson = UMADna.SaveInstance(dynamicAvatar.umaData.GetDna<UMADnaHumanoid>());

			//var N = JSON.Parse("{}");
			//N["packedSlotDataList"] = new JSONArray();
			//for(int i = 0;i < dynamicAvatar.umaData.umaRecipe.slotDataList.Length;i++) {
			//	N["packedSlotDataList"][i] = dynamicAvatar.umaData.umaRecipe.slotDataList[i];
			//}
			//N["race"] = dynamicAvatar.umaData.umaRecipe.raceData.name;
			//N["umaDna"] = new JSONArray();
			//N["packedDna"]["dnaType"] = "UMADnaHumanoid";
			//N["packedDna"]["packedDna"] = dnaJson;

			//N["slots"] = dynamicAvatar.umaData.umaRecipe.slotDataList[0].
			//text = N.ToString();
			text = LitJson.JsonMapper.ToJson(UMAPackedRecipeBase.PackRecipe(dynamicAvatar.umaData));

			saveInstance = false;
		}
	}

	public static string Save(UMADynamicAvatar avatarToSave) {
		return LitJson.JsonMapper.ToJson(UMAPackedRecipeBase.PackRecipe(avatarToSave.umaData));
	}
}
