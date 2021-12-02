using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scripts_TestBall_MiniGolf_BrianLin : MonoBehaviour
{

    public bool inWindArea = false; 
    public GameObject windArea; 
    // Respawn point of the ball 
    public GameObject respawnPoint; 
    private Rigidbody rb; 

    void Start() {
        rb = GetComponent<Rigidbody>();
    }

    // <summary> 
    // If the ball is currently in a wind area, apply a wind force to 
    // the ball to make it move a certain direction 
    // </summary>
    void FixedUpdate() {
        if(inWindArea) {
            Vector3 dir = windArea.GetComponent<Scripts_WindArea_MiniGolf_BrianLin>().direction;
            float windStrength = windArea.GetComponent<Scripts_WindArea_MiniGolf_BrianLin>().windForce;
            rb.AddForce(dir * windStrength);
        }

        // Respawn the ball if it has fallen below the course 
        if (transform.position.y <= -20f) {
            transform.position = respawnPoint.transform.position; 
            rb.velocity = Vector3.zero; 
            rb.angularVelocity = Vector3.zero; 
        }
    }

    // <summary> 
    // Checks if the ball is currently in a wind area 
    // </summary>
    // <param name="other"></param>
    void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "WindArea") {
            windArea = other.gameObject; 
            inWindArea = true; 
        }
    }

    // <summary> 
    // Checks if the ball is no longer in a wind area 
    // </summary>
    // <param name="other"></param>
    void OnTriggerExit(Collider other) {
        if (other.gameObject.tag == "WindArea") {
            inWindArea = false; 
        }        
    }

    // <summary> 
    // Respawns the ball to its respawn point
    // </summary>
    public void BallRespawn() {
        transform.position = respawnPoint.transform.position; 
        rb.velocity = Vector3.zero; 
        rb.angularVelocity = Vector3.zero; 
    }
}
