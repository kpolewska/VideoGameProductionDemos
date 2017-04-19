using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookRotation : MonoBehaviour {

	public float rotationAngle = 25f;

	public float rotationEasing = 10f;

	public float minVerticalAngle = -75;
	public float maxVerticalAngle = 75;

	public float minHorizontalAngle = -20;
	public float maxHorizontalAngle = 20;


	private Quaternion horizontalRotation;
	private Quaternion verticalRotation;

	// Use this for initialization
	void Start () {
		//Set initial rotation to the default rotation of object
		horizontalRotation = transform.localRotation;
		verticalRotation = transform.localRotation;
	}
	
	// Update is called once per frame
	void Update () {
		float horizontalAxis = Input.GetAxis("Horizontal");
		float verticalAxis = Input.GetAxis("Vertical");

		//Add a euler angle to the desired horizontal & vertical rotations, time axis & deltaTime
		horizontalRotation *= Quaternion.Euler(0, rotationAngle*horizontalAxis*Time.deltaTime, 0);
		verticalRotation *= Quaternion.Euler(-rotationAngle*verticalAxis*Time.deltaTime, 0, 0);

		//Clamp the horizontal and vertical rotations
		//Feel free to comment away the horizontalRotation part
		horizontalRotation = ClampRotationAroundYAxis(horizontalRotation);
		verticalRotation = ClampRotationAroundXAxis(verticalRotation);

		//Uses Quaternion Slerp to add an ease in/out to the rotation movement, going from current rotation to desired rotation
		transform.localRotation = Quaternion.Slerp (transform.localRotation, horizontalRotation*verticalRotation, rotationEasing * Time.deltaTime);
	}

	//Bit of code taken from the FPS controller of Unity
	//Extract the X component of a quaternion, clamp it, then reapply to quaternion
	//Same process for all angles.
	Quaternion ClampRotationAroundXAxis(Quaternion q)
	{
		q.x /= q.w;
		q.y /= q.w;
		q.z /= q.w;
		q.w = 1.0f;

		float angleX = 2.0f * Mathf.Rad2Deg * Mathf.Atan (q.x);

		angleX = Mathf.Clamp (angleX, minVerticalAngle, maxVerticalAngle);

		q.x = Mathf.Tan (0.5f * Mathf.Deg2Rad * angleX);

		return q;
	}

	Quaternion ClampRotationAroundYAxis(Quaternion q)
	{
		q.x /= q.w;
		q.y /= q.w;
		q.z /= q.w;
		q.w = 1.0f;

		float angleY = 2.0f * Mathf.Rad2Deg * Mathf.Atan (q.y);

		angleY = Mathf.Clamp (angleY, minHorizontalAngle, maxHorizontalAngle);

		q.y = Mathf.Tan (0.5f * Mathf.Deg2Rad * angleY);

		return q;
	}
}
