using UnityEngine;

public class Scripts_MiniGolf_CameraController_Wyatt : MonoBehaviour {
    [SerializeField] GameObject target; 
    [SerializeField] GameObject lighthouse;
    [SerializeField] float height;

    void LateUpdate() {
        transform.position = new Vector3(
            target.transform.position.x,
            target.transform.position.y + height,
            target.transform.position.z
            );
        transform.rotation = Quaternion.LookRotation(target.transform.position - transform.position, 
            lighthouse.transform.position - target.transform.position);
    }
}