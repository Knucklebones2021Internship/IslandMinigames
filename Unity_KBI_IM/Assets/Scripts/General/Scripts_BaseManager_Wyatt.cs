using UnityEngine;

[DefaultExecutionOrder(-50)]
public class Scripts_BaseManager_Wyatt : MonoBehaviour {
	protected virtual void Awake() {
        if (Scripts_InputManager_Wyatt.Instance == null) {
            Instantiate(Resources.Load("GlobalManager", typeof(GameObject)));
		}
	}
}
