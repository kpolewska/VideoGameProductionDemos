using UnityEngine;
using UnityEngine.AI;
using System.Collections;

[RequireComponent (typeof (NavMeshAgent))]
public class NavmeshAgentMove : MonoBehaviour {

	public Transform target;
	NavMeshAgent agent;

	float walkSpeed;
	public float jumpSpeed = 0.5f;
	public float dropSpeed = 1f;

	void Start () {
		agent = GetComponent<NavMeshAgent>();
		walkSpeed = agent.speed;
	}

	void Update () {
		// Update destination if the target moves one unit

		NavMeshHit hit;
		if (NavMesh.SamplePosition (target.position, out hit, 1, NavMesh.AllAreas)) {
			agent.SetDestination (target.position);
		} else {
			agent.ResetPath ();
		}



		if (agent.isOnOffMeshLink) {
			agent.autoTraverseOffMeshLink = true;

			if (agent.currentOffMeshLinkData.linkType == OffMeshLinkType.LinkTypeJumpAcross) 
			{
				agent.speed = jumpSpeed;
			} else if (agent.currentOffMeshLinkData.linkType == OffMeshLinkType.LinkTypeDropDown) {
				agent.speed = dropSpeed;
			}
		} else {
			agent.autoTraverseOffMeshLink = false;
			agent.speed = walkSpeed;
		}
	}
}