using UnityEngine;

public class Scripts_BubbleRun_BubbleController_Wyatt : MonoBehaviour {
    [SerializeField] GameObject cam;
    [SerializeField] GameObject cameraFollow;

    Rigidbody rb;
    Vector2 input;

    bool jumpAttempted = false;
    bool screenPressed = false;

    bool moving = false;
    bool grounded = false;

	void Awake() {
        rb = GetComponent<Rigidbody>();

        if (SystemInfo.supportsGyroscope) {
            Input.gyro.enabled = true;
            Input.gyro.updateInterval = 0.0001f;
        }
	}

    void Update() {
        //Quaternion referenceRotation = Quaternion.identity;
        //Quaternion deviceRotation = new Quaternion(0.5f, 0.5f, -0.5f, 0.5f) * Input.gyro.attitude * new Quaternion(0, 0, 1, 0);
        //Quaternion eliminationOfXY = Quaternion.Inverse(
        //    Quaternion.FromToRotation(referenceRotation * Vector3.forward, deviceRotation * Vector3.forward)
        //);
        //Quaternion rotationZ = eliminationOfXY * deviceRotation; 

        //float roll = rotationZ.eulerAngles.z;

        //print("ref: " + referenceRotation.eulerAngles);
        //print("dev: " + deviceRotation.eulerAngles);
        //print("elim: " + eliminationOfXY.eulerAngles);
        //print("z: " + rotationZ.eulerAngles);

        //print(roll);

        input = Vector2.zero;
        if (Input.GetKeyDown(KeyCode.W)) {
            input.y += 1f;
        } else if (Input.GetKeyDown(KeyCode.S)) {
            input.y -= 1f;
        } else if (Input.GetKeyDown(KeyCode.A)) {
            input.x -= 1f;
		} else if (Input.GetKeyDown(KeyCode.D)) {
            input.x += 1f;
		} 

        /*
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
		}*/

        input = input.normalized;

        // poll for jump
        if (Input.touchCount > 0) {
            if (!screenPressed) {
                jumpAttempted = true;
                screenPressed = true;
			}
        } else screenPressed = false;

        if (Input.GetKeyDown(KeyCode.Space)) { jumpAttempted = true; }
    }

	void FixedUpdate() {
        Vector3 movement = new Vector3(input.x, 0f, input.y);

        Vector3 cameraDir = transform.position - cam.transform.position;
        cameraDir.y = 0f;

        Vector3 moveDir = new Vector3(movement.x, 0f, movement.z);

        float moveForce = 800f * 10f; 
        float jumpForce = 300f;


        bool newGrounded = IsGrounded();
        if (!grounded && newGrounded) {
            // TODO CAREY: play bouncing sound
		} grounded = newGrounded;

        if (grounded) { // you can only roll if you're on the ground!
            rb.AddForce(moveDir * moveForce * Time.fixedDeltaTime);
		}

        float maxSpeed = 30f;
        print(rb.velocity.magnitude);
        //if(rb.velocity.magnitude > maxSpeed){
        //    float velocityY = rb.velocity.y;
        //    rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        //    rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxSpeed);
        //    rb.velocity = new Vector3(rb.velocity.x, velocityY, rb.velocity.z);
        //}

        if (rb.velocity.magnitude < 0.1f) {
            // CAREY TODO: stop rolling sound
            moving = false;
		} else {
            if (!moving) {
                // CAREY TODO: play rolling sound

                moving = true;
			}
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
