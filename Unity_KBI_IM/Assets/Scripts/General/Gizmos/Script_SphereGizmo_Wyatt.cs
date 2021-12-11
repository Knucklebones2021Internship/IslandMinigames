using UnityEngine;

public class Script_SphereGizmo_Wyatt : MonoBehaviour {
	private void Start() {
		
	}
	void OnDrawGizmos() {
		Gizmos.color = Color.red;
		Gizmos.DrawSphere(transform.position, transform.localScale.x);
	}
}
