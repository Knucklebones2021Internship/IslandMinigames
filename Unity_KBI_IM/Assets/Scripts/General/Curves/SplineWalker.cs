using UnityEngine;

public class SplineWalker : MonoBehaviour {
	public enum SplineWalkerMode { Once, Loop, PingPong }

	public BezierSpline spline;
	public SplineWalkerMode mode;
	public float moveSpeed;

	private float progress;
	private bool walking = false, walkComplete = false;
	private bool goingForward = true;

	private float dt = 0.005f;

	private void Awake() {
		dt *= moveSpeed;
	}

	public void SetSpeed(float speed) {
		dt = 0.005f * moveSpeed * speed;
	}

	private void FixedUpdate() {
		if (!walking) return;

		float vel = spline.GetVelocity(progress).magnitude;
		if (vel == 0f) vel += 0.0001f;
		float adjustedDeltaTime = dt / vel;

		if (goingForward) {
			progress += adjustedDeltaTime;
			if (progress > 1f) {
				if (mode == SplineWalkerMode.Once) {
					progress = 1f;

					walkComplete = true;
				} else if (mode == SplineWalkerMode.Loop) {
					progress -= 1f;
				} else {
					progress = 2f - progress;
					goingForward = false;
				}
			}
		} else {
			progress -= adjustedDeltaTime;
			if (progress < 0f) {
				progress = -progress;
				goingForward = true;
			}
		}

		transform.localPosition = spline.GetPoint(progress);
	}

	public void SetWalking(bool active) => walking = active;
	public bool GetWalkComplete() => walkComplete;

	public void ResetProgress() {
		walkComplete = false;
		progress = 0f;
	}
}