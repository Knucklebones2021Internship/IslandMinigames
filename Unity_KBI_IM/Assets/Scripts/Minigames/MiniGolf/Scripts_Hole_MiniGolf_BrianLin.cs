using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scripts_Hole_MiniGolf_BrianLin : MonoBehaviour
{
    // <summary> 
    // Destroys a ball once it has entered the whole 
    // </summary>
    // <param name="other"></param>
    void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Ball") {
            other.gameObject.SetActive(false);
            //ball.transform.position = respawnPoint.transform.position; 
            //other.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;  
            //int currScore = int.Parse(scoreText.text);
            //currScore += points;
            //scoreText.text = currScore.ToString();   

        }
    }
}
