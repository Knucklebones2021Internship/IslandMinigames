using UnityEngine;

[DefaultExecutionOrder(-50)]
public class Scripts_BubbleRun_Manager_Wyatt : MonoBehaviour {
	void Awake() {
        if (Scripts_InputManager_Wyatt.Instance == null) {
            Instantiate(Resources.Load("GlobalManager", typeof(GameObject)));
		}

        Screen.orientation = ScreenOrientation.LandscapeLeft;
	}
}
