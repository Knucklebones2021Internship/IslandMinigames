using UnityEngine;

public struct OrientedPoint {
	public Vector3 position;
	public Quaternion orientation;

	public OrientedPoint(Vector3 pos, Quaternion ori) {
		position = pos;
		orientation = ori;
	}

	public Vector3 LocalToWorldPos(Vector3 localSpacePos) {
		return position + orientation * localSpacePos;
	}

	public Vector3 LocalToWorldVec(Vector3 localSpacePos) {
		return orientation * localSpacePos;
	}
}

