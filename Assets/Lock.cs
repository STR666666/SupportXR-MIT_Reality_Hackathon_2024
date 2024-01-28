using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lock : MonoBehaviour
{
    // Start is called before the first frame update
    public string CollisionTag = "";
    public GameObject HighlightPrefab;
    public bool Locked;
    public GameObject ClonedPrefab;

    void OnTriggerEnter()
    {
        Debug.Log("YEAHG AND??");
    }
    void Start()
    {
        Debug.Log("high?");
        ClonedPrefab = Object.Instantiate(HighlightPrefab);
        ClonedPrefab.transform.parent = transform;
        ClonedPrefab.SetActive(false);
        Debug.Log(ClonedPrefab.name);
    }
    void OnCollisionEnter(Collision collision)
    {
        //Check for a match with the specified name on any GameObject that collides with your GameObject
        if (!(Locked) && collision.gameObject.tag == CollisionTag)
        {
            ClonedPrefab.SetActive(true);
        }
    }

    void OnCollisionExit(Collision collision)
    {

        //Check for a match with the specified name on any GameObject that collides with your GameObject
        if (!(Locked) && collision.gameObject.tag == CollisionTag)
        {
            ClonedPrefab.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
