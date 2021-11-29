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

    #region raycasting method [full credit: https://www.youtube.com/watch?v=LjLBHSU_yic]
    /*
    // shotPower is a multiplier for the force applied on the ball
    // stopVelocity is the velocity at which the ball is considered to be stopped
    [SerializeField] private float shotPower;
    [SerializeField] private float stopVelocity = 0.05f;

    [SerializeField] private LineRenderer lineRenderer;

    private bool isIdle;
    private bool isAiming;

    private Rigidbody rb;
    
        /// <summary>
        /// initialization
        /// </summary>
        private void Awake()
        {
            // initialize the rigidbody to the object's rigidbody
            rb = GetComponent<Rigidbody>();

            // aiming and the line renderer are initially "turned off"
            isAiming = false;
            lineRenderer.enabled = false;
        }

        /// <summary>
        /// physics calculations
        /// </summary>
        private void FixedUpdate()
        {
            // if the ball's velocity is less than or equal to
            // what is considered its stop velocity,
            // then we will stop the ball
            if (rb.velocity.magnitude <= stopVelocity)
                Stop();

            // if the ball is idle and a touch is detected
            // aiming is "turned on"
            if (isIdle)
                if (Input.touchCount > 0)
                    isAiming = true;

            // this is where we show the aim for the ball
            ProcessAim();
        }

        *//*private void OnMouseDown()
        {
            if (isIdle)
                isAiming = true;
        }*//*

        /// <summary>
        /// here we stop the ball
        /// </summary>
        private void Stop()
        {
            // set linear and angular velocity to zero
            // and mark the ball as "idle"
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            isIdle = true;
        }

        /// <summary>
        /// this is where we process touches meant for aiming the ball
        /// </summary>
        private void ProcessAim()
        {
            // if the ball is not idle, we cannot aim
            // if aiming is "turned off", we cannot aim
            // thus, we do not show the aim for the ball
            if (!isAiming || !isIdle)
                return;

            // this returns the point in world space where
            // the user has clicked the screen
            Vector3? worldPoint = CastMouseClickRay();

            // Debug.Log("World Point/Ray: " + worldPoint);

            // if the world point obtained was null,
            // then we have no aim to process/show
            if (!worldPoint.HasValue)
                return;

            // now, use that world point to draw the
            // line renderer representing the aim
            DrawLine(worldPoint.Value);

            // finally, if a touch is detected
            // and a finger was lifted from the screen
            // then shoot towards this world point
            if (Input.touchCount > 0)
                if (Input.touches[0].phase == TouchPhase.Ended)
                    Shoot(worldPoint.Value);
        }

        /// <summary>
        /// here we convert the touch position in screen space to
        /// a touch position in world space and return the point
        /// in world space that represents our aim
        /// </summary>
        /// <returns>a nullable Vector3</returns>
        private Vector3? CastMouseClickRay()
        {
            #region old code from video
            // this is code originally from the video i learned from
            // i have since converted all of the mouse input to touch input

            *//*Vector3 screenMousePositionFar = new Vector3(
                Input.mousePosition.x,
                Input.mousePosition.y,
                Camera.main.farClipPlane);
            Vector3 screenMousePositionNear = new Vector3(
                Input.mousePosition.x,
                Input.mousePosition.y,
                Camera.main.nearClipPlane);*//*
            #endregion

            // if a touch is detected
            if (Input.touchCount > 0)
            {
                // these are the touch positions in screen space,
                // with the z-component being the camera's far clip plane
                Vector3 screenTouchPositionFar = new Vector3(
                    Input.GetTouch(0).position.x,
                    Input.GetTouch(0).position.y,
                    Camera.main.farClipPlane);
                Vector3 screenTouchPositionNear = new Vector3(
                    Input.GetTouch(0).position.x,
                    Input.GetTouch(0).position.y,
                    Camera.main.nearClipPlane);

                // here we convert the touch positions in screen space to world space
                Vector3 worldTouchPositionFar = Camera.main.ScreenToWorldPoint(screenTouchPositionFar);
                Vector3 worldTouchPositionNear = Camera.main.ScreenToWorldPoint(screenTouchPositionNear);

                RaycastHit hit;

                // cast a ray with
                // - an origin at the touch position in world space relative to the near clipping plane
                // - and a direction equal to the touch position in world space between the near and far clipping planes
                // if the ray comes in contact with a collider, return the point of contact
                // otherwise, return null
                if (Physics.Raycast(worldTouchPositionNear, worldTouchPositionFar - worldTouchPositionNear,
                                    out hit, float.PositiveInfinity))
                    return hit.point;
                else
                    return null;
            }

            // if no touch is detected, return null
            return null;
        }

        /// <summary>
        /// this is where we draw the line renderer showing the aim for the ball
        /// </summary>
        /// <param name="worldPoint"></param>
        private void DrawLine(Vector3 worldPoint)
        {
            // create an array of points,
            // the first being the position of the ball
            // the second being the world point of the touch
            Vector3[] positions = { transform.position, -worldPoint };
            // originally:
            // Vector3[] positions = { transform.position, worldPoint };
            // this drew the line in the same direction as the user pulls/aims

            // set the positions of the vertices of the line to be
            // these two positions
            // and "turn on" the line renderer to show the aim
            lineRenderer.SetPositions(positions);
            lineRenderer.enabled = true;
        }

        /// <summary>
        /// this is where we shoot the ball
        /// we take as input the world point that stores our aim
        /// </summary>
        /// <param name="worldPoint"></param>
        private void Shoot(Vector3 worldPoint)
        {
            // first "turn off" aiming and the line renderer
            // since we are now shooting the ball and do not
            // need to show the aim
            isAiming = false;
            lineRenderer.enabled = false;

            // we want to only apply a horizontal force to the ball
            // so in this variable, we store the world point's x- and z- components
            // and set the y-component to the ball's y-component
            Vector3 horizontalWorldPoint = new Vector3(worldPoint.x, transform.position.y, worldPoint.z);

            // the direction of our force will be the vector between
            // the ball's position and the horizontal world point
            Vector3 direction = (horizontalWorldPoint - transform.position).normalized;

            // the strength, or magnitude, will be the distance between
            // the ball and the horizontal world point
            float strength = Vector3.Distance(transform.position, horizontalWorldPoint);

            // finally, we apply a force to the ball with a multiplier, shotPower
            rb.AddForce(-direction * strength * shotPower);
            // originally:
            // rb.AddForce(direction * strength * shotPower);
            // this shot the ball in the same direction as the user pulls/aims

            // and now the ball is not idle
            isIdle = false;
        }*/
    #endregion
}