using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WwiseEventTest : MonoBehaviour
{
    public AK.Wwise.Event testEvent;

    // Start is called before the first frame update
    void Start()
    {
        testEvent.Post(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
