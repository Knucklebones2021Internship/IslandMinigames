using UnityEngine;

public class Scripts_MiniGolf_WaterSpline_Wyatt : MonoBehaviour {
	[SerializeField] GameObject splinePrefab;
	[SerializeField] GameObject lighthouse;
	[SerializeField] Transform exitPosition;
	[SerializeField] float duration;

	Scripts_MiniGolf_BallController_Zach ball;
	BezierSpline spline;

	bool splining = false;
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

				Vector3 lighthousePos = lighthouse.transform.position;
				Vector3 ballPos = ball.transform.position;
				Vector3 exitPos = exitPosition.position;

				lighthousePos.y = 0f;
				ballPos.y = 0f;
				exitPos.y = 0f;

				Vector3 targetDir = (new Vector3(lighthousePos.x, 0f, lighthousePos.z) 
					- new Vector3(ballPos.x, 0f, ballPos.z)).normalized; // use a random angle on this

				float displacement = (lighthousePos - exitPos).magnitude;

				Vector3 targetPos = lighthouse.transform.position + targetDir * displacement;
				targetPos.y = exitPosition.position.y;

				Vector3 localTargetPos = targetPos - ball.transform.position;

				spline.SetEndPoint(localTargetPos);

				// set the two control point

				// play splash FX
				// set ball velocity to spline velocity at the end
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
