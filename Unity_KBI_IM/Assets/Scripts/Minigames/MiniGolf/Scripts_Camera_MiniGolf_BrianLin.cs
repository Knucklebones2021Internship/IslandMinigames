using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scripts_Camera_MiniGolf_BrianLin : MonoBehaviour
{
    public GameObject ball; 
    public Vector3 offset; 

    // <summary> 
    // Calculate the distance from the camera to the ball 
    // </summary>
    void Start() {
        offset = transform.position - ball.transform.position; 
    }

    // <summary> 
    // Follow the ball at a set amount of distance away from it 
    // </summary>
    void LateUpdate()
    {
        transform.position = ball.transform.position + offset;
    }
}
