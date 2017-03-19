using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    public GameObject defaultRoom;

    GameObject parent;

    List<GameObject> rooms = new List<GameObject>();
    public static int selectedRoom;

    public float smallScale = 0.05f;
    public float largeScale = 1;

    float inactiveY = -10;
    float activeY = 0;

    float currentScale
    {
        get
        {
            return rooms[0].transform.localScale.x;
        }
        set
        {
            float a, b;
            float activePos, inactivePos;
            if (selectedRoom != -1)
            {
                rooms[selectedRoom].transform.localScale = new Vector3(a = Mathf.SmoothStep(smallScale, largeScale, value), a, a);
                rooms[selectedRoom].transform.position = new Vector3(rooms[selectedRoom].transform.position.x, 
                                                                     activePos = Mathf.SmoothStep(inactiveY, activeY, value), 
                                                                     rooms[selectedRoom].transform.position.z);
            }

            if (selectedRoom == -1 || rooms.Count > 1)
            {
                b = Mathf.SmoothStep(largeScale, smallScale, value);
                if (selectedRoom == -1)
                    inactivePos = Mathf.SmoothStep(inactiveY, activeY, value);
                else
                    inactivePos = Mathf.SmoothStep(activeY, inactiveY, value);

                    for (int i = 0; i < rooms.Count; ++i)
                        if (i != selectedRoom)
                        {
                            rooms[i].transform.localScale = new Vector3(b, b, b);
                            rooms[i].transform.position = new Vector3(rooms[i].transform.position.x, inactivePos, rooms[i].transform.position.z);
                        }
            }
        }
    }

    public float scaleTimer = 0.25f;
    float currentScalingTime;

    bool initScaling = false;
    bool needsToScale
    { get { return selectedRoom == -1 ? rooms[0].transform.localScale.x != smallScale : rooms[selectedRoom].transform.localScale.x != largeScale; } }

	// Use this for initialization
	void Start ()
    {
        CreateRoom();
	}
	
	// Update is called once per frame
	void Update ()
    {
        CheckScale();
    }

    public void CreateRoom()
    {
        rooms.Add(new GameObject("Room" + (rooms.Count + 1)));
        GameObject test = Instantiate(defaultRoom);
        test.transform.parent = rooms[rooms.Count - 1].transform;
        test.transform.localPosition = Vector3.zero;
        SetParent(rooms.Count - 1);
    }

    void SetParent(int id)
    {
        parent = GameObject.Find("_Rooms");
        if (parent == null) parent = new GameObject("_Rooms");
        rooms[id].transform.parent = parent.transform;
    }

    void CheckScale()
    {
        if (needsToScale)
        {
            if (!initScaling)
            {
                currentScalingTime = 0;
                initScaling = true;
            }
            currentScalingTime += Time.deltaTime;
            currentScale = Mathf.SmoothStep(0, 1, Mathf.Clamp01(currentScalingTime / scaleTimer));
        }
        else
            initScaling = false;
    }
}
