using UnityEngine;

public class Scripts_BubbleRun_Gate_Wyatt : MonoBehaviour {
	[SerializeField] GameObject door;
	[SerializeField] bool validDoor;

	void Awake() {
		door.GetComponent<MeshCollider>().enabled = !validDoor;
	}
}
