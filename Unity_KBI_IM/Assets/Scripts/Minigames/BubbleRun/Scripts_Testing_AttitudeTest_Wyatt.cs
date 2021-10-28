using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scripts_Testing_AttitudeTest_Wyatt : MonoBehaviour {
	Quaternion refRot = Quaternion.identity;
	Quaternion initRot;

	private void Awake() {
        if (SystemInfo.supportsGyroscope) {
            Input.gyro.enabled = true;
            Input.gyro.updateInterval = 0.0001f;
        } initRot = LeftHandify(Input.gyro.attitude);
	}

	void Update() {
		Quaternion attitude = LeftHandify(Input.gyro.attitude);
		Quaternion newRot = attitude * initRot * Quaternion.Inverse(refRot);
		transform.rotation = newRot;

		print("--------------------------------");
		print(transform.rotation.eulerAngles);
		print(transform.localRotation.eulerAngles);
		print("--------------------------------");
	}

	Quaternion LeftHandify(Quaternion rightHandedQuaternion) {
          return new Quaternion (- rightHandedQuaternion.x,
              - rightHandedQuaternion.z,
              - rightHandedQuaternion.y,
              rightHandedQuaternion.w);
    }
}
