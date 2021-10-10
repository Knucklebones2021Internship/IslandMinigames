using UnityEngine;
using UnityEngine.InputSystem;

public class Scripts_BubbleRun_BubbleController_Wyatt : MonoBehaviour {
    [SerializeField] GameObject cam;

    Rigidbody rb;
    Vector2 input;

    bool jumpAttempted = false;

	void Awake() {
        rb = GetComponent<Rigidbody>();

        Scripts_InputManager_Wyatt.EnableAttitudeSensor();
	}

	void OnEnable() {
        Scripts_InputManager_Wyatt.touchPress.started += Jump;
	}

	void OnDisable() {
        Scripts_InputManager_Wyatt.touchPress.started -= Jump;
	}

	void Update() {
        Quaternion attitude = Scripts_InputManager_Wyatt.GetAttitudeSensorValue();

        float attitudeX = attitude.eulerAngles.x;
        float attitudeY = attitude.eulerAngles.y;

        if (attitudeX < 355f && attitudeX > 270f) {
            input.x = 1f; // right
		} else if (attitudeX > 5f && attitudeX < 90f) {
            input.x = -1f; // left
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

        float moveForce = 300;
        float jumpForce = 300f;

        if (IsGrounded()) { // you can only roll if you're on the ground!
            rb.AddForce(moveDir * moveForce * Time.fixedDeltaTime);
		}

        float maxSpeed = 50f;
        if(rb.velocity.magnitude > maxSpeed){
            float velocityY = rb.velocity.y;
            rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
            rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxSpeed);
            rb.velocity = new Vector3(rb.velocity.x, velocityY, rb.velocity.z);
        }

        if (jumpAttempted) {
            if (IsGrounded()) {
                Vector3 velocityDir = rb.velocity.normalized;
                rb.AddForce(new Vector3(velocityDir.x, jumpForce, velocityDir.z));
			} jumpAttempted = false;
		}
    }

    bool IsGrounded() {
        return Physics.Raycast(transform.position, -Vector3.up, 0.51f);
	}

    void Jump(InputAction.CallbackContext context) { 
        print("wyatt position: " + Scripts_InputManager_Wyatt.touchPosition.ReadValue<Vector2>());
        print("wyatt jump!");

        jumpAttempted = true;

        //transform.position = Vector3.zero;
    }
}
