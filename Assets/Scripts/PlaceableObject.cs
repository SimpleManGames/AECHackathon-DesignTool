using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceableObject : MonoBehaviour {
    public bool hasBeenCreated = false;
    Vector3 pos;
    public GameObject followObj;
    //public PlaceObject po;
    RaycastHit hit;
    Renderer rend;
    // Use this for initialization
    void Start () {
        pos = transform.position;
        rend = GetComponent<Renderer>();
        //sfollowObj = po.dominantController;
	}
	
	// Update is called once per frame
	void Update () {
        //pos.y = Mathf.Clamp(pos.y, 2, 3);
        
        if (followObj != null)
        {
            gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
            if (Physics.Raycast(followObj.transform.position, followObj.transform.forward, out hit))
            {
                pos = hit.point;
                pos.y = hit.point.y + rend.bounds.size.y /2;
            }
        }
        else { gameObject.layer = LayerMask.NameToLayer("Default"); }
        transform.position = pos;
	}
}
