using UnityEngine;
using UnityEngine.InputSystem;

public class Scripts_BubbleRun_BubbleController_Wyatt : MonoBehaviour {
    [SerializeField] GameObject cam;

    Rigidbody rb;

    Vector2 input;

    void pw(object message) {
        print("wyatt + " + message.ToString());
	}

	private void Awake() {
        rb = GetComponent<Rigidbody>();

        Screen.orientation = ScreenOrientation.LandscapeLeft;

        if (AttitudeSensor.current != null) {
            InputSystem.EnableDevice(AttitudeSensor.current);
        }
	}

	private void Update() {
        // y corresponds to forward
            // flat is at zero degrees
            // 360 -> 270 is forward
            // 0 -> 90 is backward

        Quaternion attitude = AttitudeSensor.current.attitude.ReadValue();
        pw("x: " + attitude.eulerAngles.x + "\ny: " + attitude.eulerAngles.y + "\nz: " + attitude.eulerAngles.z);

        float attitudeX = attitude.eulerAngles.x;
        float attitudeY = attitude.eulerAngles.y;

        //if (Keyboard.current[Key.W].wasPressedThisFrame) {
        //    input.y += 1;
		//}
        //if (Keyboard.current[Key.S].wasPressedThisFrame) {
        //    input.y -= 1;
		//}
        //if (Keyboard.current[Key.A].wasPressedThisFrame) {
        //    input.x -= 1;
		//}
        //if (Keyboard.current[Key.D].wasPressedThisFrame) {
        //    input.x += 1;
		//}

        if (attitudeX < 355f && attitudeX > 270f) {
            input.x = 1f; // forward
		} else if (attitudeX > 5f && attitudeX < 90f) {
            input.x = -1f; // backward
		} else {
            input.x = 0f; // stop
		}

        if (attitudeY < 355f && attitudeY > 270f) {
            input.y = 1f; // forward
		} else if (attitudeY > 5f && attitudeY < 90f) {
            input.y = -1f; // backward
		} else {
            input.y = 0f; // stop
		}

        input = input.normalized;
	}

	void FixedUpdate() {
        Vector3 movement = new Vector3(input.x, 0f, input.y);

        Vector3 cameraDir = transform.position - cam.transform.position;
        cameraDir.y = 0f;

        Vector3 moveDir = new Vector3(movement.x, 0f, movement.z);
        pw("movement: " + moveDir);

        float force = 60;
        float speed = 7f;
        //rb.AddForce(moveDir * force * Time.fixedDeltaTime);
        rb.velocity = moveDir * speed;
    }
}
