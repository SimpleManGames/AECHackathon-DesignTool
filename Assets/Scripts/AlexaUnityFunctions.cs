using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public class AlexaUnityFunctions : MonoBehaviour
{

    public Light env_light;

    Dictionary<string, Color> colorDictionay = new Dictionary<string, Color>()
        {
            { "red", Color.red }, { "blue", Color.blue }, { "green", Color.green }
        };

    public List<Action> queueAlexaCommands = new List<Action>();

    Action processQueueAction;

    public void ChangeLightColor(string m)
    {
        var color = colorDictionay.Where(c => c.Key == m).First().Value;
        Debug.Log(color.ToString());
        env_light.color = Color.red;
    }

    public void Update()
    {
        Debug.Log(queueAlexaCommands.Count());
        if (queueAlexaCommands.Count() > 0)
        {
            processQueueAction = queueAlexaCommands.First();
            queueAlexaCommands.RemoveAt(0);
        }
        if (processQueueAction != null)
        {
            processQueueAction();
            processQueueAction = null;
        }
    }
}
