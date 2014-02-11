using UnityEngine;
using System.Collections;

public class GUIBigLabelFader : MonoBehaviour {
	public GUIText guiTextComponent;
	public GUITexture guiTextureComponent;
	public float transitionSpeed = 2f;

	private bool showLabel = false;
	private float lowAlpha = 0f;
	private float highAlpha = 1f;
	private string text = "";
	private float timer = 0f;
	
	void Update () {
		if (timer < 0) {
			showLabel = false;
			//Debug.Log ("!showLabel");
			timer = 0;
		} else if(timer > 0) {
			timer -= Time.deltaTime;
		}

		if (showLabel) {
			guiTextComponent.text = text;
			guiTextComponent.color = new Color(guiTextComponent.color.r, guiTextComponent.color.g, guiTextComponent.color.b, Mathf.Lerp (guiTextComponent.color.a, highAlpha, transitionSpeed * Time.deltaTime));
			//guiTextureComponent.color = new Color(guiTextureComponent.color.r, guiTextureComponent.color.g, guiTextureComponent.color.b, Mathf.Lerp (guiTextureComponent.color.a, highAlpha, transitionSpeed * Time.deltaTime));
			//guiTextureComponent.enabled = true;
		} else {
			if(guiTextComponent.color.a > 0.05f) {
				guiTextComponent.text = text;
				guiTextComponent.color = new Color(guiTextComponent.color.r, guiTextComponent.color.g, guiTextComponent.color.b, Mathf.Lerp (guiTextComponent.color.a, lowAlpha, transitionSpeed * Time.deltaTime));
				//guiTextureComponent.color = new Color(guiTextureComponent.color.r, guiTextureComponent.color.g, guiTextureComponent.color.b, Mathf.Lerp (guiTextureComponent.color.a, lowAlpha, transitionSpeed * Time.deltaTime));
				//guiTextureComponent.enabled = true;
			}
			else {
				guiTextComponent.text = "";
				//guiTextureComponent.enabled = false;
			}
		}
	}

	public void Show(string text_, float time) {
		text = text_;
		showLabel = true;
		timer = time;
	}

	public void Show(string text_) {
		text = text_;
		showLabel = true;
		timer = 5f;
	}

	//public void Hide() {
	//	showLabel = false;
	//}
}
