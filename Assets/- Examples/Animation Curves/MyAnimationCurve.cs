using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyAnimationCurve : MonoBehaviour {

	public AnimationCurve myCurve;
	public float jumpHeight = 3;
	public float jumpTime = 2;

	float currentJumpTime;

	bool canJump = true;
	bool isJumping = false;
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown ("Jump")) {
			if (canJump)
				StartCoroutine (Jump());
		}

		if (isJumping) {
			currentJumpTime += Time.deltaTime / jumpTime;

			transform.position = Vector3.up * myCurve.Evaluate (currentJumpTime) * jumpHeight;
		}
	}

	IEnumerator Jump(){
		
		currentJumpTime = 0;

		canJump = false;
		isJumping = true;

		yield return new WaitForSeconds (jumpTime);

		canJump = true;
		isJumping = false;

		yield return null;
	}
}
