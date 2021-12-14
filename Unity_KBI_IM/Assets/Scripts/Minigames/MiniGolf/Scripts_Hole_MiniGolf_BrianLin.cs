using UnityEngine;

public class Scripts_Hole_MiniGolf_BrianLin : MonoBehaviour {

    // Boolean for whether the ball has went into this hole 
    public bool holeIn = false; 

    // <summary> 
    // Destroys a ball once it has entered the whole 
    // </summary>
    // <param name="other"></param>
    void OnTriggerEnter(Collider other) {
        Scripts_MiniGolf_BallController_Zach ball = other.GetComponent<Scripts_MiniGolf_BallController_Zach>();

        if (ball != null) {
            // delay this for a moment for any celebration particles and sfx
            ball.CompleteHole();

            // legacy
            //other.gameObject.SetActive(false);
            //holeIn = true; 
		}
    }
}
