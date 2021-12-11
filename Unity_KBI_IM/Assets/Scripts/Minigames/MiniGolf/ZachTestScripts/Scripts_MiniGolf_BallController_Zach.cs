using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// this is the ball controller script
/// </summary>

// we first require that the ball have a rigidbody
[RequireComponent(typeof(Rigidbody))]
public class Scripts_MiniGolf_BallController_Zach : MonoBehaviourPun
{
    [Tooltip("A multiplier for applying power to the ball.")]
    public float power = 10f;
    [Tooltip("The maximum force magnitude. This is used for clamping the vector of the finger drag, which is used for applying the force on the ball.")]
    public float maxForceMagnitude = 5f;
    [Tooltip("The velocity at which the ball is considered stopped. At or below this threshold, the ball will manually stop.")]
    public float stopVelocity = 0.1f;
    [SerializeField] [Tooltip("The uniform scale factor for the aim circle.")]
    private float aimCircleScale = 3f;

    public Rigidbody rb;
    private LineRenderer line;
    private Camera mainCam;
    private GameObject aimCircle;

    private Vector3 dragStartPos;
    private Touch touch;
    private bool isIdle;
    private bool isAiming = false;
    private bool mouseClicked = false;

    [System.Serializable]
    public struct WwiseGolfEvents
    {
        public AK.Wwise.Event BallInWater;
        public AK.Wwise.Event BallRollsBack;
        public AK.Wwise.Event HitOffRamp;
        public AK.Wwise.Event WindmillGust;
        public AK.Wwise.Event LaserBlock;
        public AK.Wwise.Event ReboundBlock;
        public AK.Wwise.Event HoleInOne;
        public AK.Wwise.Event Par;
        public AK.Wwise.Event OverPar;
        public AK.Wwise.Event WrongAnswer;
        public AK.Wwise.Event TimeUp;
    }
    [SerializeField] private WwiseGolfEvents wwiseGolfEvents;


    private void Awake() {
        rb = GetComponent<Rigidbody>();
        line = GetComponent<LineRenderer>();
        mainCam = Camera.main;
        aimCircle = transform.GetChild(0).gameObject;

        line.positionCount = 2;
        line.enabled = false;

        aimCircle.transform.localScale *= aimCircleScale;
        aimCircle.SetActive(true);

        Scripts_MiniGolf_CameraController_Wyatt cam = GameObject.Find("MinigolfCamera").GetComponent<Scripts_MiniGolf_CameraController_Wyatt>();
        if (photonView.IsMine || !PhotonNetwork.IsConnected) {
            cam.SetTarget(this);
		}
    }

	void Start() {
        if (photonView.IsMine) {
            Scripts_MiniGolf_Manager_Wyatt.LocalPlayerInstance = this.gameObject;
        }
	}

	private void Update() {
        // if our client does not own this ball, prevent us from controlling it
        if (!photonView.IsMine && PhotonNetwork.IsConnected) return;

        if (rb.velocity.magnitude <= stopVelocity)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            isIdle = true;

            aimCircle.transform.rotation = new Quaternion(0.707106829f, 0, 0, 0.707106829f);
            aimCircle.SetActive(true);
        }
        else
        {
            isIdle = false;
            aimCircle.SetActive(false);
        }

        line.SetPosition(0, transform.position);

		#region MOBILE SUPPORT
		if (Input.touchCount > 0) {
            touch = Input.GetTouch(0);

            Debug.Log("Touch position: " + touch.position);

            if (touch.phase == TouchPhase.Began && isIdle && !isAiming)
                DragStart(new Vector3(touch.position.x, touch.position.y, mainCam.transform.position.y));

            if (touch.phase == TouchPhase.Moved)
                if (isAiming) Dragging(new Vector3(touch.position.x, touch.position.y, mainCam.transform.position.y));

            if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
                if (isAiming) DragRelease(new Vector3(touch.position.x, touch.position.y, mainCam.transform.position.y));
        }
		#endregion

		#region MOUSE SUPPORT
        if (Input.GetKeyDown(KeyCode.Space)) { // kill velocity for debug purposes
            rb.velocity = Vector3.zero;
		}

		Vector2 mousePosition = Input.mousePosition;

        if (Input.GetMouseButtonUp(0)) { // un-clicked
            mouseClicked = false;
            if (isAiming) {
                DragRelease(new Vector3(mousePosition.x, mousePosition.y, mainCam.transform.position.y));
			}
        } else if (mouseClicked) { // mouse moved
            if (isAiming) {
                Dragging(new Vector3(mousePosition.x, mousePosition.y, mainCam.transform.position.y));
            }
		} else if (Input.GetMouseButtonDown(0)) { // clicked
            mouseClicked = true;
            if (isIdle && !isAiming) {
                DragStart(new Vector3(mousePosition.x, mousePosition.y, mainCam.transform.position.y));
			}
		}
		#endregion
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