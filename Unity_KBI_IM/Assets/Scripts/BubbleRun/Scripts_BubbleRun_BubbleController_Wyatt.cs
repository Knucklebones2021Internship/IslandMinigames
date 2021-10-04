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

        pw(AttitudeSensor.current);

        if (AttitudeSensor.current != null) {
            InputSystem.EnableDevice(AttitudeSensor.current);
        }
	}

	private void Update() {
        pw(AttitudeSensor.current.attitude.ReadValue());

        if (Keyboard.current[Key.W].wasPressedThisFrame) {
            input.y += 1;
		}
        if (Keyboard.current[Key.S].wasPressedThisFrame) {
            input.y -= 1;
		}
        if (Keyboard.current[Key.A].wasPressedThisFrame) {
            input.x -= 1;
		}
        if (Keyboard.current[Key.D].wasPressedThisFrame) {
            input.x += 1;
		}

        input = input.normalized;
	}

	void FixedUpdate() {
        Vector3 movement = new Vector3(input.x, 0f, input.y);

        Vector3 cameraDir = transform.position - cam.transform.position;

        Vector3 moveDir = new Vector3(movement.x * cameraDir.x, 0f, movement.y * cameraDir.y);

        float force = 10;
        rb.AddForce(moveDir * force * Time.fixedDeltaTime);
        
    }
}
