using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowObject : MonoBehaviour {

	public Transform objectToFollow;
	public Transform objectToRotateWith;
	
	// Update is called once per frame
	void Update () {
		transform.position = objectToFollow.position;
		transform.rotation = objectToRotateWith.rotation;

	}
}
