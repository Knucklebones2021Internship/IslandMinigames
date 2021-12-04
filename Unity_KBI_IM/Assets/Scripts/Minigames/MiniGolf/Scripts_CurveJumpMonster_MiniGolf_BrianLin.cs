using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scripts_CurveJumpMonster_MiniGolf_BrianLin : MonoBehaviour
{
    // Boolean for whether the jump monster is permitted to move 
    public bool moving = false;
    // Amount of time the jump monster pauses for   
    public float pause = 3f;  

    void Start() {
        StartCoroutine(JumpMonsterWait()); 
    }
    
    // Update is called once per frame
    void Update()
    {
        //animation += Time.deltaTime; 
        //animation = animation % 5f; 

        //transform.position = Scripts_MathParabola_MiniGolf_BrianLin.Parabola(Vector3.zero, Vector3.forward * 10f, 5f, animation / 5f);

        //if (Input.GetMouseButtonDown(0)) {
        //    GetComponent<Scripts_ParabolaController_MiniGolf_BrianLin>().FollowParabola(); 
        //}
        /**/

        if (moving) {
            GetComponent<Scripts_ParabolaController_MiniGolf_BrianLin>().FollowParabola(); 
            moving = false;
        }
        
    }

    // <summary> 
    // Wait for a few seconds before starting to move 
    // </summary>
    IEnumerator JumpMonsterWait() {
        moving = false; 
        yield return new WaitForSeconds(3); 
        moving = true; 
    }    
}
