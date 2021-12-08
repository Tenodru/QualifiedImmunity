using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class ObjectiveObjects
{
    public ObjectiveInfo objective;
    public GameObject[] objects;
    public float timeToWaitIn = 0;
    public float timeToWaitOut = 0;
}

public class ObjectiveShowHide : MonoBehaviour
{
    public List<ObjectiveObjects> objectiveObjects;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(LateStart(0.1f));
    }

    IEnumerator LateStart(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        GameEvents.current.onAssignObjective += ObjectiveAssigned;
    }

    public IEnumerator Timer(Action<bool> assigner, float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        assigner(true);
    }

    public void ObjectiveAssigned(ObjectiveInfo objectiveInfo)
    {
        Debug.Log("ran");
        foreach (ObjectiveObjects objectiveObject in objectiveObjects)
        {
            if (objectiveObject.objective != objectiveInfo)
            {
                Debug.Log(objectiveInfo.name);
                foreach (GameObject obj in objectiveObject.objects)
                {
                    Debug.Log("Disabling " + obj.name);
                    StartCoroutine(Timer(x => obj.SetActive(false), objectiveObject.timeToWaitOut));
                }
            }
        }
        foreach (ObjectiveObjects objectiveObject in objectiveObjects)
        {
            if (objectiveObject.objective == objectiveInfo)
            {
                foreach (GameObject obj in objectiveObject.objects)
                {
                    StartCoroutine(Timer(x => obj.SetActive(true), objectiveObject.timeToWaitIn));
                }
            }
        }
    }

    public void OnDestroy()
    {
        GameEvents.current.onAssignObjective -= ObjectiveAssigned;
    }
}
