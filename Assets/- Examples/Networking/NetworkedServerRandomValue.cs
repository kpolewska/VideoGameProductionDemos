using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class NetworkedServerRandomValue : NetworkBehaviour {

	[SyncVar]
	public float serverValue = 0;

	public Text myText;
	
	// Update is called once per frame
	void Start () {
		if (isServer) {
			serverValue = Random.value;
			myText.text = serverValue.ToString();
		}
	}

	void Update(){
		if (isServer) {
			if (Input.anyKeyDown) {
				serverValue = Random.value;


			}
		}

		myText.text = serverValue.ToString();
	}
}
