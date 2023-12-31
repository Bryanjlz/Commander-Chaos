using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MyTools {
	public static Quaternion SmoothDampQuaternion(Quaternion current, Quaternion target, ref Vector3 currentVelocity, float smoothTime) {
		Vector3 c = current.eulerAngles;
		Vector3 t = target.eulerAngles;
		return Quaternion.Euler(
		  Mathf.SmoothDampAngle(c.x, t.x, ref currentVelocity.x, smoothTime),
		  Mathf.SmoothDampAngle(c.y, t.y, ref currentVelocity.y, smoothTime),
		  Mathf.SmoothDampAngle(c.z, t.z, ref currentVelocity.z, smoothTime)
		);
	}

	public static float negativeCube(float num) {
		return 1 - (num * num * num);
	}
}
