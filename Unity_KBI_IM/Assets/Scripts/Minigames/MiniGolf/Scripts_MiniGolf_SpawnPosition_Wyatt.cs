using UnityEngine;

public class Scripts_MiniGolf_SpawnPosition_Wyatt : MonoBehaviour {
	void OnDrawGizmos() {
		Gizmos.color = Color.red;
		Gizmos.DrawSphere(transform.position, 0.3f);
	}
}
