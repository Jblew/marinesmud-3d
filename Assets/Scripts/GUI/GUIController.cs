using UnityEngine;
using System.Collections;

[ExecuteInEditMode()] 
public class GUIController : MonoBehaviour {
	public GUILoginPanel guiLoginPanel;
	public Texture2D texture = null;
	public float angle = 0;
	public float lastAngle = 0;
	public Vector2 size = new Vector2(128, 128);
	public bool soundOn = true;

	private Vector2 pos = new Vector2(0, 0);
	private Rect rect;
	private Vector2 pivot;
	private GameObject player = null;
	private long i = 0;

	void Start() {
		player = GameObject.FindGameObjectWithTag (Tags.PLAYER);
		if (guiLoginPanel == null) {
			Debug.LogWarning ("GUIController.guiLoginPanel is null");
		}
	}

	void OnGUI() {
		if(player != null) {
			angle = -player.transform.eulerAngles.y;

			pos = new Vector2(Screen.width / 2, Screen.height);
			rect = new Rect(pos.x - size.x * 0.5f, pos.y - size.y * 0.5f, size.x, size.y);
			pivot = new Vector2(rect.xMin + rect.width * 0.5f, rect.yMin + rect.height * 0.5f);

			Matrix4x4 matrixBackup = GUI.matrix;
			GUIUtility.RotateAroundPivot(angle, pivot);
			GUI.DrawTexture(rect, texture);
			GUI.matrix = matrixBackup;
		}
		else player = GameObject.FindGameObjectWithTag (Tags.PLAYER);

		if (soundOn) {
			if (GUI.Button (new Rect (Screen.width-80, Screen.height - 30, 75, 25), "Mute")) {
				soundOn = false;
				AudioListener.volume = 0;
			}
		} else {
			if (GUI.Button (new Rect (Screen.width-80, Screen.height - 30, 75, 25), "Unmute")) {
				soundOn = true;
				AudioListener.volume = 100;
			}
		}

		if (i % 50 == 0 && guiLoginPanel != null) {
			if(guiLoginPanel.correctLogin != null) transform.FindChild("UsernameText").gameObject.GetComponent<GUIText>().text = guiLoginPanel.correctLogin;
			if(guiLoginPanel.selectedCharacter != null) transform.FindChild("CharacterNameText").gameObject.GetComponent<GUIText>().text = guiLoginPanel.selectedCharacter["name"];
		}



		i++;
	}
}
