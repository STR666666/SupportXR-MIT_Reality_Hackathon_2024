using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Oculus.Interaction;
using Normal.Realtime;
public class yeagWeld : MonoBehaviour
{

    public GameObject yeagOne;
    public GameObject yeagTwo;
    public GameObject mainThing;
    bool Grabbed = false;
    // Start is called before the first frame update
    void Start()
    {
        var GrabbableC = yeagOne.GetComponent<Grabbable>();
        var RealtimeTransformC = yeagTwo.GetComponent<RealtimeTransform>();
        GrabbableC.WhenPointerEventRaised += OnPointer;
    }

    void OnPointer(PointerEvent evt)
    {
        switch (evt.Type)
        {
            case PointerEventType.Select:
                Grabbed = true;
                yeagTwo.GetComponent<RealtimeTransform>().RequestOwnership();
                break;
            case PointerEventType.Unselect:
                Grabbed = false;
                break;
            case PointerEventType.Cancel:
                Grabbed = false;
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
            yeagOne.transform.position = yeagTwo.transform.position;
            yeagOne.transform.rotation = yeagTwo.transform.rotation;
        }
    }
}
