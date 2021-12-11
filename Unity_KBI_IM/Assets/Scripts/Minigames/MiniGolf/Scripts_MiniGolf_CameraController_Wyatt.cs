using UnityEngine;

public class Scripts_MiniGolf_CameraController_Wyatt : MonoBehaviour {
    [SerializeField] Scripts_MiniGolf_BallController_Zach target; 
    [SerializeField] GameObject upTarget;
    [SerializeField] bool useUpTarget;
    [SerializeField] bool isStatic;
    [SerializeField] float height;

    void LateUpdate() {
        if (!isStatic) {
            transform.position = new Vector3(
                target.transform.position.x,
                target.transform.position.y + height,
                target.transform.position.z
                );
		}

        if (useUpTarget) {
            transform.rotation = Quaternion.LookRotation(target.transform.position - transform.position, 
                upTarget.transform.position - target.transform.position);
		}
    }

    public void SetTarget(Scripts_MiniGolf_BallController_Zach _target) {
        target = _target;
	}
}
