using UnityEngine;

public class Scripts_MiniGolf_CameraController_Wyatt : MonoBehaviour {
    [SerializeField] Scripts_MiniGolf_BallController_Zach target; 
    [SerializeField] GameObject upTarget;
    [SerializeField] Transform spawnTransform;
    [SerializeField] bool useUpTarget;
    [SerializeField] bool useUpTargetForStaticForward;
    [SerializeField] bool isStatic;
    [SerializeField] float height;

    Vector3 cameraUpVector = Vector3.forward;

	void Start() {
        if (useUpTargetForStaticForward) {
            cameraUpVector = upTarget.transform.position - spawnTransform.position;
            cameraUpVector.y = 0f; // make sure the angle is top down
		}
	}

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
		} else {
            transform.rotation = Quaternion.LookRotation(target.transform.position - transform.position,
                cameraUpVector);
                //Vector3.forward);
		}
    }

    public void SetTarget(Scripts_MiniGolf_BallController_Zach _target) {
        target = _target;
	}

    public Scripts_MiniGolf_BallController_Zach GetTarget() {
        return target;
	}
}
