using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// this is the ball controller script
/// </summary>

// we first require that the ball have a rigidbody
[RequireComponent(typeof(Rigidbody))]
public class Scripts_MiniGolf_BallController_Zach : MonoBehaviour 
{
    [Tooltip("A multiplier for applying power to the ball.")]
    public float power = 10f;
    [Tooltip("The maximum force magnitude. This is used for clamping the vector of the finger drag, which is used for applying the force on the ball.")]
    public float maxForceMagnitude = 5f;
    [Tooltip("The velocity at which the ball is considered stopped. At or below this threshold, the ball will manually stop.")]
    public float stopVelocity = 0.1f;

    private Rigidbody rb;
    private LineRenderer line;
    private Camera mainCam;

    private Vector3 dragStartPos;
    private Touch touch;
    private bool isIdle;
    private bool isAiming = false;

    private void Awake() {
        rb = GetComponent<Rigidbody>();
        line = GetComponent<LineRenderer>();
        mainCam = Camera.main;

        line.positionCount = 2;
        line.enabled = false;
    }

	private void Update() {
        if (rb.velocity.magnitude <= stopVelocity) {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            isIdle = true;
        } else isIdle = false;

        line.SetPosition(0, transform.position);

        if (Input.touchCount > 0) {
            touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began && isIdle && !isAiming)
                DragStart(new Vector3(touch.position.x, touch.position.y, mainCam.transform.position.y));

            if (touch.phase == TouchPhase.Moved)
                if (isAiming) Dragging(new Vector3(touch.position.x, touch.position.y, mainCam.transform.position.y));

            if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
                if (isAiming) DragRelease(new Vector3(touch.position.x, touch.position.y, mainCam.transform.position.y));
        }
    }

    private void DragStart(Vector3 screenPoint) {
        dragStartPos = mainCam.ScreenToWorldPoint(screenPoint);
        isAiming = true;
    }

    private void Dragging(Vector3 screenPoint) {
        Vector3 draggingPos = mainCam.ScreenToWorldPoint(screenPoint);

        Vector3 offset = draggingPos - dragStartPos;
        line.SetPosition(1, transform.position + offset);
        line.enabled = true;
    }

    private void DragRelease(Vector3 screenPoint) {
        isAiming = false;
        line.enabled = false;

        Vector3 dragReleasePos = mainCam.ScreenToWorldPoint(screenPoint);
        Vector3 force = dragStartPos - dragReleasePos;
        Vector3 clampedForce = Vector3.ClampMagnitude(force, maxForceMagnitude);

        // add an instant force to the ball, in the direction opposite the drag and with a multiplier of power
        rb.AddForce(clampedForce * power, ForceMode.Impulse);
    }
}