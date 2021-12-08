using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scripts_Camera_MiniGolf_BrianLin : MonoBehaviour
{
    public GameObject ball; 
    public Vector3 offset; 

    void Start() {
        offset = transform.position - ball.transform.position; 
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = ball.transform.position + offset;
    }
}
