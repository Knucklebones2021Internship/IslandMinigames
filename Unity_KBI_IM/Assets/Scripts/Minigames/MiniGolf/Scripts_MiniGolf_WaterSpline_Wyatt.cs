using UnityEngine;

public class Scripts_MiniGolf_WaterSpline_Wyatt : MonoBehaviour {
	[SerializeField] GameObject splinePrefab;
	[SerializeField] GameObject lighthouse;
	[SerializeField] Transform exitPosition;
	[SerializeField] float duration;

	Scripts_MiniGolf_BallController_Zach ball;
	BezierSpline spline;

	public bool splining = false;
	float progress;

	void OnTriggerEnter(Collider other) {
		ball = other.GetComponent<Scripts_MiniGolf_BallController_Zach>(); 

		if (ball != null) {
			if (ball.gameObject == Scripts_MiniGolf_Manager_Wyatt.LocalPlayerInstance) {
				if (splining) return;
				splining = true;
				progress = 0f;

				// set up spline
				spline = Instantiate(splinePrefab, ball.transform.position, Quaternion.identity).GetComponent<BezierSpline>();
				spline.SetStartPoint(Vector3.zero);

				// temporary positions
				Vector3 lighthousePos = lighthouse.transform.position;
				Vector3 ballPos = ball.transform.position;
				Vector3 exitPos = exitPosition.position;
				lighthousePos.y = 0f;
				ballPos.y = 0f;
				exitPos.y = 0f;

				// calculate the spline end point
				Vector3 targetDir = (lighthousePos - ballPos).normalized; // use a random angle on this
				float displacement = (lighthousePos - exitPos).magnitude;
				Vector3 targetPos = lighthouse.transform.position + targetDir * displacement;
				targetPos.y = exitPosition.position.y;
				Vector3 localTargetPos = targetPos - ball.transform.position;
				spline.SetEndPoint(localTargetPos);

				#region SPLINE CONTROL POINTS
				float side = 1f; // initialze side to left

				// set the near control point
				Vector3 forward = lighthousePos - ballPos;
				Vector3 cp1 = Vector3.Cross(forward, Vector3.up); // get a vector orthogonal to the lighthouse direction
				if (Vector3.Dot(cp1, ball.rb.velocity) < 0f) side = -side; // choose which side to go based on our initial direction
				cp1 = cp1 * side; // multiply by side to flip if necessary
				cp1 = cp1 + forward * 0.5f; // move control point in direction of lighthouse for more circular curvature
				cp1 = cp1 + Vector3.down * 0.5f; // move control point down so we go under the water
				Vector3 cp1Local = cp1; // convert to local space relative to spline origin (redundant because spline origin is the same as ballPos)
				spline.SetControlPoint(1, cp1Local);

				// set the far control point
				Vector3 forward2 = lighthousePos - targetPos;
				Vector3 cp2 = Vector3.Cross(forward2, Vector3.up); // get a vector orthogonal to the lighthouse direction
				cp2 = cp2 * -side; // side must be negative this cross product is a reflection of the previous
				cp2 = cp2 + forward2 * 0.5f; // more circular curvature
				cp2 = cp2 + Vector3.down * 0.5f; // move beneath the water
				Vector3 cp2Local = cp2 + targetPos - ballPos; // convert to local space relative to spline origin
				spline.SetControlPoint(2, cp2Local);
				#endregion

				// play splash FX
			}
		}
	}

	private void Update() {
		if (!splining) return;

		progress += Time.deltaTime;
		ball.transform.localPosition = spline.GetPoint(progress);
		if (progress > 1f) {
			progress = 1f;
			ball.rb.velocity = spline.GetVelocity(progress); // perhaps clamp this
			splining = false;
		}
	}
}
