using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Oculus.Interaction;
using Normal.Realtime;
using System.Net.Sockets;
public class yeagWeld : MonoBehaviour
{
    public GameObject yeagOne;
    public GameObject yeagTwo;
    public GameObject mainThing;
    public GameObject LockedTo;
    List<GameObject> currentCollisions = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        var GrabbableC = yeagOne.GetComponent<Grabbable>();
        var RealtimeTransformC = yeagTwo.GetComponent<RealtimeTransform>();
        GrabbableC.WhenPointerEventRaised += OnPointer;
    }
    void OnCollisionEnter(Collision col)
    {

        // Add the GameObject collided with to the list.
        currentCollisions.Add(col.gameObject);
    }

    void OnCollisionExit(Collision col)
    {

        // Remove the GameObject collided with from the list.
        currentCollisions.Remove(col.gameObject);
    }
    void OnPointer(PointerEvent evt)
    {
        switch (evt.Type)
        {
            case PointerEventType.Select:
                if (LockedTo)
                {
                    var Locked = LockedTo.GetComponent<Lock>();
                    Locked.Locked = false;
                    Locked.ClonedPrefab.SetActive(true);
                    LockedTo = null;
                }
                yeagTwo.GetComponent<RealtimeTransform>().RequestOwnership();
                break;
            case PointerEventType.Unselect:
                foreach (var obj in currentCollisions)
                {
                    Lock lockedc;
                    obj.TryGetComponent<Lock>(out lockedc);

                    if (lockedc != null)
                    {
                        if (!lockedc.Locked && lockedc.CollisionTag == yeagTwo.tag)
                        {
                            lockedc.Locked = true;
                            yeagOne.transform.position = obj.transform.position;
                            yeagOne.transform.rotation = obj.transform.rotation;
                            lockedc.ClonedPrefab.SetActive(false);
                            LockedTo = obj;
                        }
                    }
                }
                break;
            case PointerEventType.Cancel:
                foreach (var obj in currentCollisions)
                {
                    Lock lockedc;
                    obj.TryGetComponent<Lock>(out lockedc);
                    Debug.Log(obj);
                    if (lockedc != null)
                    {
                        if (!lockedc.Locked && lockedc.CollisionTag == yeagTwo.tag)
                        {
                            lockedc.Locked = true;
                            transform.position = obj.transform.position;
                            transform.rotation = obj.transform.rotation;
                            lockedc.ClonedPrefab.SetActive(false);
                            LockedTo = obj;
                        }
                    }
                }
                break;
        }
    }
    // Update is called once per frame
    void Update()
    {
        var MainThing = mainThing.GetComponent<Realtime>().clientID;
        if (yeagTwo.GetComponent<RealtimeTransform>().ownerIDSelf == MainThing)
        {
            yeagTwo.transform.position = yeagOne.transform.position;
            yeagTwo.transform.rotation = yeagOne.transform.rotation;
        }
        else
        {
            if (LockedTo)
            {
                var Locked = LockedTo.GetComponent<Lock>();
                Locked.Locked = false;
                Locked.ClonedPrefab.SetActive(false);
                LockedTo = null;
            }
            yeagOne.transform.position = yeagTwo.transform.position;
            yeagOne.transform.rotation = yeagTwo.transform.rotation;
        }
    }
}
