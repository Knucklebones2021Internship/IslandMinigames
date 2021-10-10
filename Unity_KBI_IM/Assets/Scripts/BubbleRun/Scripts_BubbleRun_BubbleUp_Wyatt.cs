using UnityEngine;

public class Scripts_BubbleRun_BubbleUp_Wyatt : MonoBehaviour {
    void Update() {
        transform.rotation = Quaternion.LookRotation(Vector3.forward);
    }
}
