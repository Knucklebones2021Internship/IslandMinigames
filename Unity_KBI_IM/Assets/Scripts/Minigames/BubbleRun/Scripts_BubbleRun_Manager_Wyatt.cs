using UnityEngine;

[DefaultExecutionOrder(-50)]
public class Scripts_BubbleRun_Manager_Wyatt : Scripts_BaseManager_Wyatt {
	protected override void Awake() {
		base.Awake();

        Screen.orientation = ScreenOrientation.LandscapeLeft;
	}
}
