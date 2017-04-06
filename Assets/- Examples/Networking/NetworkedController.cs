using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkedController : NetworkBehaviour {

	public Vector3 randomDirection;

	// Use this for initialization
	void Start () {
		if (hasAuthority) {
			randomDirection = new Vector3 (Random.value, Random.value, Random.value);
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (hasAuthority) {
			transform.position += randomDirection.normalized * Time.deltaTime;
		}
	}
}
