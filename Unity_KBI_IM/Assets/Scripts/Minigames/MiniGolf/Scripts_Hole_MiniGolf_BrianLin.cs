using UnityEngine;

public class Scripts_Hole_MiniGolf_BrianLin : MonoBehaviour {

    // Boolean for whether the ball has went into this hole 
    public bool holeIn = false; 

    // <summary> 
    // Destroys a ball once it has entered the whole 
    // </summary>
    // <param name="other"></param>
    void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Ball") {
            other.gameObject.SetActive(false);
            holeIn = true; 
        }
    }
}
