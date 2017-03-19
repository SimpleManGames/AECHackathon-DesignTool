using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerHandler : MonoBehaviour
{
    public bool triggerButtonDown = false;
    public bool dLeftDown = false;
    public bool dRightDown = false;
    public bool gripped = false;

    private Valve.VR.EVRButtonId triggerButton = Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger;
    private Valve.VR.EVRButtonId dLeft = Valve.VR.EVRButtonId.k_EButton_DPad_Left;
    private Valve.VR.EVRButtonId dRight = Valve.VR.EVRButtonId.k_EButton_DPad_Right;
    private Valve.VR.EVRButtonId grip = Valve.VR.EVRButtonId.k_EButton_Grip;

    public PlaceObject po;
    public GameObject hoveredObj;
    public LineRenderer lr;
    float triggerBuffer = .5f;
    public bool canTrigger;

    private SteamVR_Controller.Device controller { get { return SteamVR_Controller.Input((int)trackedObj.index); } }
    private SteamVR_TrackedObject trackedObj;

    void Start()
    {

        trackedObj = GetComponent<SteamVR_TrackedObject>();
    }

    void Update()
    {
        triggerBuffer -= Time.deltaTime;
        if (triggerBuffer<=0)
        {
            canTrigger = true;
        }
        else { canTrigger = false; }
        //lr.SetPositions(transform.position, po.objToPlace.transform.position);
        if (controller == null)
        {
            Debug.Log("Controller not initialized");
            return;
        }
        triggerButtonDown = controller.GetPressDown(triggerButton);
        dLeftDown = controller.GetPressDown(dLeft);
        dRightDown = controller.GetPressDown(dRight);
        gripped = controller.GetPressDown(grip);
        if (Input.GetKey(KeyCode.A))
        {
            po.RotateObjRight();
        }
        if (Input.GetKey(KeyCode.D))
        {
            po.RotateObjLeft();
        }
        //MIGHT NEED TO FIND "GETKEY" EQUIVILENT FOR VIVE CONTROLS
        if (dLeftDown)
        {
            po.RotateObjLeft();
        }
        if (dRightDown)
        {
            po.RotateObjRight();
        }
        if (gripped)
        {
            if (RoomManager.selectedRoom == 0)
            {
                RoomManager.selectedRoom = -1;
            }

            else { RoomManager.selectedRoom = 0; }
        }
        //if (Input.GetMouseButtonDown(0))
        //{
        //    triggerButtonDown = true;
        //    triggerBuffer = .5f;
        //}
        //else { triggerButtonDown = false; }
        if (triggerButtonDown)
        {
            Debug.Log("Fire");
        }
        RaycastHit hit;
        if (Physics.Raycast(transform.position,transform.forward, out hit))
        {
            if (lr!= null)
            {
                lr.SetPosition(0, transform.position);
                lr.SetPosition(1, hit.point);
            }
            if (!hit.collider.gameObject.GetComponent(typeof(MenuObject)) && !hit.collider.gameObject.GetComponent(typeof(PlaceableObject)))
            {
                if (hoveredObj != null)
                {
                    if(hoveredObj.GetComponent<IHighlightable>() != null)
                        hoveredObj.GetComponent<IHighlightable>().StoppedHighlighting();
                    hoveredObj = null;
                }
            }
            else if (hit.collider.gameObject.GetComponent(typeof(MenuObject)))
            {
                hoveredObj = hit.collider.gameObject.GetComponent<MenuObject>().obj;
                if (hoveredObj.GetComponent<IHighlightable>() != null)
                    hoveredObj.GetComponent<IHighlightable>().OnHighlighted();
            }
            else if (hit.collider.gameObject.GetComponent(typeof(PlaceableObject)))
            {
                hoveredObj = hit.collider.gameObject;
                if (hoveredObj.GetComponent<IHighlightable>() != null)
                    hoveredObj.GetComponent<IHighlightable>().OnHighlighted();
            }
        }
        if (triggerButtonDown)
        {
            if (hoveredObj!= null)
            {
                if (!po.placingObj)
                {
                    if (canTrigger)
                    {
                        Debug.Log("tried to set");
                        po.objToPlace = hoveredObj;
                    }
                }
            }
        }
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, transform.forward);
    }
}

