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
    [Tooltip("The uniform scale factor for the aim circle.")]
    public float aimCircleScale = 3f;

    public GameObject ballHitFX;
    public GameObject waterFX;

    public Rigidbody rb;
    private LineRenderer line;
    private Camera mainCam;
    private GameObject aimCircle;

    private Vector3 dragStartPos;
    private Touch touch;
    private bool isIdle;
    private bool isAiming = false;
    private bool mouseClicked = false;

    Scripts_MiniGolf_CameraController_Wyatt localCameraController;
    Scripts_MiniGolf_WaterSpline_Wyatt splineScript;

    private bool waterEffectsPlayable = true; // being set to false is equivalent to the water effects having already played once

    private Quaternion globalUpDirection = new Quaternion(0.707106829f, 0, 0, 0.707106829f);

    //bool playing = true;

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

        localCameraController = GameObject.Find("MinigolfCamera").GetComponent<Scripts_MiniGolf_CameraController_Wyatt>();
        if (photonView.IsMine || !PhotonNetwork.IsConnected) {
            localCameraController.SetTarget(this);
		}

        splineScript = GameObject.Find("Water").GetComponent<Scripts_MiniGolf_WaterSpline_Wyatt>();
    }

	void Start() {
        if (photonView.IsMine) {
            Scripts_MiniGolf_Manager_Wyatt.LocalPlayerInstance = this.gameObject;
        }
	}

	private void Update() {
        // if our client does not own this ball, prevent us from controlling it
        if (!photonView.IsMine && PhotonNetwork.IsConnected) return;

        //if (!playing) return;

        if (rb.velocity.magnitude <= stopVelocity)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            isIdle = true;

            aimCircle.transform.rotation = globalUpDirection;
            aimCircle.SetActive(true);
        }
        else
        {
            isIdle = false;
            aimCircle.SetActive(false);
        }

        line.SetPosition(0, transform.position);

        // if the water effects have already been played upon entering the spline
        // and the water spline is complete, allow water effects to be played
        if (!waterEffectsPlayable && !splineScript.splining)
            waterEffectsPlayable = true;

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

    /// <summary>
    /// This function is called by Scripts_Hole_MiniGolf_BrianLin.cs when a ball enters a hole
    /// </summary>
    public void CompleteHole() { // TODO: probably need to check the hole index for correct or incorrect
        if (photonView.IsMine) {
            // tell all other clients that this ball is complete
            photonView.RPC("CompleteBall", RpcTarget.All, photonView.ViewID);
            //playing = false;
		}
	}

    [PunRPC]
    void CompleteBall(int photonID) {
        // remove this ball from the local potential spectate list
        Scripts_MiniGolf_BallController_Zach ball = Scripts_MiniGolf_Manager_Wyatt.RemovePlayerFromLocalSpectateList(photonID);

        if (ball == null) return; // if the ball did not exist, terminate (something went wrong, maybe this should be an assertion)

        // if our camera is currently looking at this ball
        if (localCameraController.GetTarget() == ball) {
            // grab a new candidate from the spectate list
            Scripts_MiniGolf_BallController_Zach candidate = Scripts_MiniGolf_Manager_Wyatt.GetLocalSpectateCandidate(0);

            if (candidate == null) { // if there is no candidate
                // enter post game UI
                print("enter post game UI");
			} else { // if a candidate does exist
                // focus our camera on the candidate
                localCameraController.SetTarget(candidate);

                // enable specate UI if not enabled already
                print("enable spectate UI");
			}
		}

        // turn off this ball
        ball.gameObject.SetActive(false);
    }

    private void OnCollisionEnter(Collision collision)
    {
        // if the ball is not in the water, aka the water effects are playable,
        // then hit effects can be played

        if (waterEffectsPlayable)
            PlayHitEffects(collision);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Water"))
            if (waterEffectsPlayable)
            {
                PlayWaterEffects(other);

                waterEffectsPlayable = false;
            }
    }

    private void PlayHitEffects(Collision collision)
    {
        // spawn position is point of impact
        Vector3 spawnPosition = collision.contacts[0].point;
        Quaternion spawnRotation = globalUpDirection;

        // Debug.Log("Relative velocity magnitude: " + collision.relativeVelocity.magnitude);

        // we want to scale the particle effect (gameobject) according to the hit velocity
        // we scale the particle effect (gameobject) according to the hit velocity
        // in this case, we can use relative velocity from the collision info to represent the hit velocity,
        // since the walls are static and do not have a velocity
        GameObject effectGameObject = Instantiate(ballHitFX, spawnPosition, spawnRotation);
        effectGameObject.transform.localScale *= HitFXScaleFactor(collision.relativeVelocity.magnitude);

        // now that the particle effect (gameobject) is scaled, we can play the particle effect itself
        // note that the particle system's PlayOnAwake is toggled off, so that it could be scaled upon instantiation before playing
        ParticleSystem effect = effectGameObject.GetComponent<ParticleSystem>();
        effect.Play();

        float effectDuration = 2f;
        Destroy(effectGameObject, effectDuration);
    }

    private void PlayWaterEffects(Collider collider)
    {
        // spawn position is point of impact
        // ClosestPointOnBounds returns the closest point to the bounding box of the attached collider
        // since the collider in this case is attached to a quad, ClosestPointOnBounds returns exactly the point of impact
        Vector3 spawnPosition = collider.ClosestPointOnBounds(transform.position);
        Quaternion spawnRotation = Quaternion.identity;

        GameObject effect = Instantiate(waterFX, spawnPosition, spawnRotation);

        float effectDuration = 3f;
        Destroy(effect, effectDuration);
    }

    // my original solution to scaling was using Mathf.Clamp01,
    // but i thought this one, using inverse lerp + lerp, would give a better scaling effect,
    // by remapping values between 0.1 and 1 instead of "throwing away" values outside of 0 and 1, per se, with Clamp01
    private float HitFXScaleFactor(float relativeVelocity)
    {
        float maxRelativeVelocity = 15f; // from testing the hardest i can shoot the ball

        // the idea here is that relative velocities in (0, 15) will be used to scale the particle FX scale down to (0.1, 1),
        // using inverse lerp + lerp make this a linear transformation
        // however, to make smaller relative velocities transform to larger scales for visibility, we use a non-linear transformation,
        // in this case, the square root function (other functions considered were sine, log, and higher roots)
        // we first extrapolate the percentage that the relative velocity is between 0 and the max relative velocity
        // then we transform it non-linearly with the square root function
        // finally, we use this transformed percentage to map the relative velocity to a scale factor between 0.1 and 1
        // 0.1 was chosen as the lower end of lerp instead of 0 to ensure visibility for tiny relative velocities

        float percentage = Mathf.InverseLerp(0, maxRelativeVelocity, relativeVelocity);
        float transformedPercentage = Mathf.Sqrt(percentage);
        float scaleFactor = Mathf.Lerp(0.1f, 1, transformedPercentage);

        // Debug.Log("Scale factor: " + scaleFactor);

        return scaleFactor;
    }
}