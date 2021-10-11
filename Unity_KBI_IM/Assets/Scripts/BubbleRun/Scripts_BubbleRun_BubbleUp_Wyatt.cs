using UnityEngine;

public class Scripts_BubbleRun_BubbleUp_Wyatt : MonoBehaviour {
    void FixedUpdate() {
        transform.rotation = Quaternion.LookRotation(Vector3.forward);
    }
}
