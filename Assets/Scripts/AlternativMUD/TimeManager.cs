using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using SimpleJSON;

public class TimeManager : MonoBehaviour {
	public static long timeCorrectionTicks;
	public int year = 0;
	public int month = 0;
	public int day;
	public int hour;
	public int minute;
	public int second;
	public string dateStr;
	public string timeCorrectionStr;
	public string startedAtTimeInGameStr;
	public delegate void TimeHasChanged(long currentTimeTicks);

	private static readonly DateTime Jan1st1970 = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
	private static List<TimeHasChanged> eventListeners = new List<TimeHasChanged>();

	// Use this for initialization
	void Start () {
		GetComponent<AlternativMUDClient> ().AddListener (AlternativeMUDClasses.MSG_U3DM_SCENE_ENTER_SUCCEEDED, SceneEnterSucceeded);
	}

	private void SceneEnterSucceeded(string jsonData) {
		var N = JSON.Parse (jsonData);
		setTimeInGame (long.Parse(N["timeInGame"]));
	}

	// Update is called once per frame
	private long i = 0;
	void Update () {
		if (Debug.isDebugBuild) {
			if(i % 10 == 0) {
				long currentTicks = getCurrentTicks ();
				DateTime date = new DateTime (currentTicks);
				dateStr = date.ToString ("yyyy-MM-ddTHH:mm:ssZ");
			}
			i++;
		}
	}

	private void setTimeInGame(long timeInGame) {
		long ticksInGame = new DateTime((timeInGame * TimeSpan.TicksPerMillisecond) + Jan1st1970.Ticks).ToLocalTime().Ticks;
		timeCorrectionTicks = ticksInGame-DateTime.Now.Ticks;
		timeCorrectionStr = new DateTime(timeCorrectionTicks).ToString("yyyy-MM-ddTHH:mm:ssZ");
		startedAtTimeInGameStr = new DateTime(ticksInGame).ToString("yyyy-MM-ddTHH:mm:ssZ");
	}

	public static long getCurrentTicks() {
		return DateTime.Now.Ticks + timeCorrectionTicks;
	}

	public static void AddListener(TimeHasChanged eventListener) {
		eventListeners.Add (eventListener);
	}
}
