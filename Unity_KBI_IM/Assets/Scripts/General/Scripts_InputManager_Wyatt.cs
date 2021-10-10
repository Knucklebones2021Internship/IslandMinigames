using UnityEngine;

public class Scripts_InputManager_Wyatt : MonoBehaviour {
	public static Scripts_InputManager_Wyatt Instance { get; private set; }

	void Awake() {
		if (Instance == null) {
			Instance = this;
			DontDestroyOnLoad(gameObject);
		} else {
			Destroy(gameObject);
		}
	}
}
