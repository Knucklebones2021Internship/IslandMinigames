using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scripts_CurveJumpMonster_MiniGolf_BrianLin : MonoBehaviour
{
    //protected float animation;  
    public bool moving = false;
    public float pause = 3f;  

    void Start() {
        JumpMonsterWait(); 
    }

    // Update is called once per frame
    void Update()
    {
        //animation += Time.deltaTime; 
        //animation = animation % 5f; 

        //transform.position = Scripts_MathParabola_MiniGolf_BrianLin.Parabola(Vector3.zero, Vector3.forward * 10f, 5f, animation / 5f);

        if (Input.GetMouseButtonDown(0)) {
            GetComponent<Scripts_ParabolaController_MiniGolf_BrianLin>().FollowParabola(); 
        }
        /*
        if (pause > 0) {
            pause -= Time.deltaTime; 
        }
        
        else if (pause <= 0) {
            pause = 0; 
            moving = true;
        }

        if (moving) {
            GetComponent<Scripts_ParabolaController_MiniGolf_BrianLin>().FollowParabola(); 
        }
        */
    }

    IEnumerator JumpMonsterWait() {
        moving = false; 
        yield return new WaitForSeconds(3); 
        moving = true; 
    }    
}
