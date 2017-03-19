using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceObject : MonoBehaviour {
    public GameObject objToPlace;
    public GameObject dominantController;
    public GameObject lastObjPlaced;
    public GameObject objHolder;
    public bool placingObj = false;
    GameObject tempObj;
    bool tickBuffer = false;
    float objBuffer = .5f;
    List<GameObject> ObjsInRoom = new List<GameObject>();
	// Use this for initialization
	void Start () {
        objToPlace = null;
	}

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            UndoPlacement();
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            ClearRoom();
        }
        if (Input.GetKeyDown(KeyCode.C))
        {

        }
        if (Input.GetKeyDown(KeyCode.V))
        {

        }
        if (tickBuffer)
        {
            objBuffer -= Time.deltaTime;
            if (objBuffer <= 0)
            {
                tickBuffer = false;
                objBuffer = .5f;
            }
        }
        if (objToPlace != null)
        {
            if (!placingObj)
            {
                if (objToPlace.GetComponent<PlaceableObject>().hasBeenCreated)
                {
                    ReselectObject(objToPlace);
                }
                else { CreateNewObject(objToPlace); }
            }
        }
        if (placingObj)
        {
            if (dominantController.GetComponent<ControllerHandler>().canTrigger)
            {
                if (dominantController.GetComponent<ControllerHandler>().triggerButtonDown)
                {
                    if (objToPlace != null || lastObjPlaced==tempObj)
                    {
                        if (!tickBuffer) { DeselectObject(tempObj); }
                    }
                }
            }
        }
    }
    public void ReselectObject(GameObject obj)
    {
        tempObj = obj;
        tempObj.GetComponent<PlaceableObject>().followObj = dominantController;
        placingObj = true;
        tickBuffer = true;
    }
    void CreateNewObject(GameObject obj)
    {
        tempObj = Instantiate(obj, dominantController.transform.position,Quaternion.identity) as GameObject;
        tempObj.transform.parent = objHolder.transform;
        //tempObj.transform.localPosition = new Vector3(0, 7, 0);
        placingObj = true;
        tempObj.GetComponent<PlaceableObject>().hasBeenCreated = true;
        tempObj.GetComponent<PlaceableObject>().followObj = dominantController;
        tickBuffer = true;
        ObjsInRoom.Add(tempObj);
    }
    void DeselectObject(GameObject obj)
    {
        obj.GetComponent<PlaceableObject>().followObj = null;
        lastObjPlaced = obj;
        objToPlace = null;
        placingObj = false;
    }
    public IEnumerator UndoPlacement()
    {
        ReselectObject(lastObjPlaced);
        yield return new WaitForSeconds(.1f);
    }
    void DeleteObj(GameObject obj)
    {
        ObjsInRoom.Remove(obj);
        Destroy(obj);
    }
    public IEnumerator ClearRoom()
    {
        for(int i = 0; i < ObjsInRoom.Count; i++)
        {
            Destroy(ObjsInRoom[i].gameObject);
        }
        yield return new WaitForSeconds(.1f);
    }
    public void RotateObjLeft()
    {
        if (placingObj)
        {
            tempObj.transform.Rotate(Vector3.up * -20 * Time.deltaTime);
        }
    }
    public void RotateObjRight()
    {
        if (placingObj)
        {
            tempObj.transform.Rotate(Vector3.up*20*Time.deltaTime);
        }
    }
}
