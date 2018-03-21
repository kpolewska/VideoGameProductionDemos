using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkedCharacterController : NetworkBehaviour {

    //SyncVar will make sure that the value of this variable will be shared between all clients & server/host for the respective instance of this class
    [SyncVar]
    public int healthBar = 100;

    public TextMesh textMesh;

	// Use this for initialization
	void Start () {

        //Instantiate the textMesh object
        GameObject textMeshObject = Instantiate(textMesh.gameObject);
        textMeshObject.transform.SetParent(this.transform);
        textMeshObject.transform.position = transform.position;

        textMesh = textMeshObject.GetComponent<TextMesh>();

		if (hasAuthority) {
            GetComponent<Renderer>().material.color = Random.ColorHSV();
		}
	}
	
	// Update is called once per frame
	void Update () {

        textMesh.text = "Health: " + healthBar +"\n";

        textMesh.text += netId + "\n";

        if (isServer && hasAuthority)
        {
            if (isClient && hasAuthority)
            {
                //I'm a server AND client, therefore I'm the host
                textMesh.text += "I'm the Host\n";
            }
            else
            {
                //If I'm a server, but not a client, I'm a Server ONLY
                textMesh.text += "I'm the server\n";
            }
        }
        else
        {
            //If I'm the client, but not the host
            textMesh.text += "I'm a client\n";
        }

        if (hasAuthority)
        {
            textMesh.text += "I control this entity\n";

            transform.position += new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")) * Time.deltaTime;

            CmdSetHealth(healthBar);
        }
    }

    [Command]
    void CmdSetHealth(int value)
    {
        healthBar = value;
    }
}
