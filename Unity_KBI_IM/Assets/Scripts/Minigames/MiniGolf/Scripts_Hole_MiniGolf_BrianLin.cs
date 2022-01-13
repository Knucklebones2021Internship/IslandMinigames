using System.Collections;
using UnityEngine;

public class Scripts_Hole_MiniGolf_BrianLin : MonoBehaviour {

    // Boolean for whether the ball has went into this hole 
    public bool holeIn = false;
    public bool correctHole;

    private GameObject holeFX;

    private void Awake()
    {
        holeFX = transform.GetChild(0).gameObject;
    }

    // <summary> 
    // Destroys a ball once it has entered the whole 
    // </summary>
    // <param name="other"></param>
    void OnTriggerEnter(Collider other) {
        Scripts_MiniGolf_BallController_Zach ball = other.GetComponent<Scripts_MiniGolf_BallController_Zach>();

        if (ball != null) {
            StartCoroutine(CompleteHole(ball));
		}
    }

    IEnumerator CompleteHole(Scripts_MiniGolf_BallController_Zach ball) {
        GameObject effects;
        if (correctHole) {
            effects = Instantiate(holeFX, transform.position, Quaternion.identity);
        } else {
            // play incorrect effect :((
            effects = new GameObject();
        }

        float particleEffectDuration = 1.2f;
        yield return new WaitForSeconds(particleEffectDuration);

        Destroy(effects);

        // delay this for a moment for any celebration particles and sfx
        ball.CompleteHole();
    }
}
