using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scripts_WindArea_MiniGolf_BrianLin : MonoBehaviour
{
    public float windForce; 
    public Vector3 direction; 

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float x = Mathf.PingPong(Time.time, 2) * 3 - 24;
        transform.position = new Vector3(x, transform.position.y, transform.position.z);        
    }
}
