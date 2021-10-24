using UnityEngine;

public class Scripts_BubbleRun_BubbleController_Wyatt : MonoBehaviour {
    [SerializeField] GameObject cam;
    [SerializeField] GameObject cameraFollow;

    Rigidbody rb;
    Vector2 input;

    bool jumpAttempted = false;
    bool screenPressed = false;

	void Awake() {
        rb = GetComponent<Rigidbody>();

        if (SystemInfo.supportsGyroscope) {
            Input.gyro.enabled = true;
        }
	}

    void Update() {
        Vector3 attitude = Input.gyro.attitude.eulerAngles;

        float attitudeX = attitude.x;
        float attitudeY = attitude.y;
        float offset = 30f;

        if (attitudeX < 360f - offset && attitudeX > 180f) {
            input.x = 1f; // right
		} else if (attitudeX > 0f + offset && attitudeX < 180f) {
            input.x = -1f; // left
		} else {
            input.x = 0f; // stop
		}

        if (attitudeY < 360f - offset && attitudeY > 180f) {
            input.y = 1f; // forward
		} else if (attitudeY > 0f + offset && attitudeY < 180f) {
            input.y = -1f; // backward
		} else {
            input.y = 0f; // stop
		}

        input = input.normalized;

        // poll for jump
        if (Input.touchCount > 0) {
            if (!screenPressed) {
                jumpAttempted = true;
                screenPressed = true;
			}
        } else screenPressed = false;
    }

	void FixedUpdate() {
        Vector3 movement = new Vector3(input.x, 0f, input.y);

        Vector3 cameraDir = transform.position - cam.transform.position;
        cameraDir.y = 0f;

        Vector3 moveDir = new Vector3(movement.x, 0f, movement.z);

        float moveForce = 800f; 
        float jumpForce = 300f;

        if (IsGrounded()) { // you can only roll if you're on the ground!
            rb.AddForce(moveDir * moveForce * Time.fixedDeltaTime);
		}

        float maxSpeed = 100f;
        print(rb.velocity.magnitude);
        if(rb.velocity.magnitude > maxSpeed){
            print("clamping speed");
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

        cameraFollow.transform.position = transform.position;
    }

    bool IsGrounded() {
        return Physics.Raycast(transform.position, -Vector3.up, 0.51f);
	}
}
