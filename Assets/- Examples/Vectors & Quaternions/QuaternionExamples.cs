using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuaternionExamples : MonoBehaviour {

	[Header("VECTOR 3 ROTATION EXAMPLE")]
	public bool enableVectorRotationExample = false;
	public Vector3 eulerRotation = Vector3.zero;
	public Vector3 initialForwardDirection = Vector3.forward;
	public bool useGameObjectForward = false;

	[Space(20)]
	[Header("QUATERNION ANGLE AXIS EXAMPLE")]
	public bool enableAngleAxisExample = false;
	public float angleDegree = 45.5f;
	public Vector3 rotationAxisVector = Vector3.forward; //Imagine placing a pin on a object. This becomes the new axis of rotation

	[Space(20)]
	[Header("QUATERNION SLERP EXAMPLE")]
	public bool enableSlerpExample = false;
	public Vector3 lookVector = Vector3.forward;
	public float slerpSpeed = 1;

	void OnDrawGizmos() {
		if(enableVectorRotationExample) VectorRotationExample();
		if(enableAngleAxisExample) AngleAxisExample();
		if(enableSlerpExample) SlerpQuaternionExample();
	}

	void VectorRotationExample(){
		Quaternion rotationAngle = Quaternion.Euler(eulerRotation);
		Vector3 directionVector = (useGameObjectForward)? transform.forward : initialForwardDirection;
		Vector3 rotatedVector = rotationAngle * directionVector;

		//Get Quaternion rotation angle from the rotated vector. Uses rotatedVector.Z as the forward of the object (Basically where the object will "look")
		//The second parameter is used to determine the vector upward of the object, which in this case would be rotationAngle*Vector3.up
		//Use localRotation if you want to rotate object according to its parent rotation. Otherwise, use transform.rotation
		transform.localRotation = Quaternion.LookRotation(rotatedVector, rotationAngle*Vector3.up);
	
//		//Can also be done using the method transform.rotation.SetLookRotation, but I advise against.
//		//Both have the same result, with the difference that the SetLookRotation allows you to change the up axis of the object.
//		//As you see below, it takes a lot more work to implement.
//		Quaternion setLookRotationExample = new Quaternion();
//		setLookRotationExample.SetLookRotation(rotatedVector, Vector3.up);
//		transform.rotation = setLookRotationExample;

		//Draw the Rotated Vector
		Gizmos.color = Color.white;
		Gizmos.DrawLine(transform.position, rotatedVector);
		Gizmos.DrawSphere(rotatedVector, 0.1f);

		//Draw the 3 Local axis
		Gizmos.color = Color.red; // X Right
		Gizmos.DrawLine(transform.position, transform.right);

		Gizmos.color = Color.green; // Y Up
		Gizmos.DrawLine(transform.position, transform.up);

		Gizmos.color = Color.blue; // Z Forward
		Gizmos.DrawLine(transform.position, transform.forward);

		//Draw the 3 world axis
		Gizmos.color = Color.red; // X Right
		Gizmos.DrawLine(transform.position, Vector3.right);

		Gizmos.color = Color.green; // Y Up
		Gizmos.DrawLine(transform.position, Vector3.up);

		Gizmos.color = Color.blue; // Z Forward
		Gizmos.DrawLine(transform.position, Vector3.forward);
	}

	//----------------------------------------------------------------------------------------------------------------------------
	//Angle axis helps you rotate an object by a certain amount of degrees on a specific axis
	void AngleAxisExample(){
		Quaternion rotationAngle = Quaternion.AngleAxis(angleDegree, rotationAxisVector);
		transform.localRotation = rotationAngle;

		//Draw the Rotation Axis Vector
		Gizmos.DrawLine(transform.position, rotationAxisVector);

		//Draw the 3 local axis
		Gizmos.color = Color.red; // X Right
		Gizmos.DrawLine(transform.position, (rotationAngle * Vector3.right).normalized);

		Gizmos.color = Color.green; // Y Up
		Gizmos.DrawLine(transform.position, (rotationAngle * Vector3.up).normalized);

		Gizmos.color = Color.blue; // Z Forward
		Gizmos.DrawLine(transform.position, (rotationAngle * Vector3.forward).normalized);
	}

	void SlerpQuaternionExample(){
		//Like vectors, you can lerp and slerp quaternion values
		//transform.rotation = Quaternion.Slerp(transform.localRotation, Quaternion.LookRotation(lookVector), slerpSpeed * Time.deltaTime);

		transform.rotation = Quaternion.Slerp (transform.localRotation, Quaternion.LookRotation (transform.right), slerpSpeed * Time.deltaTime);
	}
















}
